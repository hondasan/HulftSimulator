using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Runtime.InteropServices;
using HulftSchedulerApp.Logging;

namespace HulftSchedulerApp.Services
{
    public class RealHulftSender : IHulftSender
    {
        private readonly LogService _logService;
        private readonly string _hostName;

        public RealHulftSender(LogService logService)
        {
            _logService = logService;
            _hostName = (ConfigurationManager.AppSettings["HulftHostName"] ?? string.Empty).Trim();
        }

        public HulftSendResult SendFile(string hulftFileId, string filePath)
        {
            if (string.IsNullOrWhiteSpace(_hostName))
            {
                return new HulftSendResult
                {
                    Success = false,
                    Message = "App.config の HulftHostName を設定してください。"
                };
            }

            if (!File.Exists(filePath))
            {
                return new HulftSendResult
                {
                    Success = false,
                    Message = "送信対象ファイルが存在しません。"
                };
            }

            var native = new HulftNative(msg => _logService?.Write(msg));
            var sendResult = native.Send(hulftFileId, filePath, _hostName);

            return new HulftSendResult
            {
                Success = sendResult == HulftNative.SendResult.OK,
                Message = BuildResultMessage(sendResult)
            };
        }

        private static string BuildResultMessage(HulftNative.SendResult result)
        {
            switch (result)
            {
                case HulftNative.SendResult.OK:
                    return "HULFT送信に成功しました";
                case HulftNative.SendResult.HulftrtLoadError:
                    return "hulftrt.dll の読み込みに失敗しました";
                case HulftNative.SendResult.HulapiLoadError:
                    return "hulapi.dll の読み込みに失敗しました";
                case HulftNative.SendResult.ProcAddressError:
                    return "utlsendex API のアドレス取得に失敗しました";
                default:
                    return "HULFT API 呼び出しでエラーが発生しました";
            }
        }

        public static bool IsEnvironmentReady(out string reason)
        {
            reason = string.Empty;

            IntPtr hHulDll = NativeMethods.LoadLibrary("hulftrt.dll");
            if (hHulDll == IntPtr.Zero)
            {
                reason = FormatWin32Error("hulftrt.dll を読み込めませんでした");
                return false;
            }
            NativeMethods.FreeLibrary(hHulDll);

            IntPtr hApiDll = NativeMethods.LoadLibrary("hulapi.dll");
            if (hApiDll == IntPtr.Zero)
            {
                reason = FormatWin32Error("hulapi.dll を読み込めませんでした");
                return false;
            }
            NativeMethods.FreeLibrary(hApiDll);

            reason = "hulftrt.dll / hulapi.dll を正常に読み込みました";
            return true;
        }

        private static string FormatWin32Error(string prefix)
        {
            var code = Marshal.GetLastWin32Error();
            var message = new Win32Exception(code).Message;
            return $"{prefix} (Win32Error: {code} {message})";
        }

        private class HulftNative
        {
            private readonly Action<string> _log;

            public HulftNative(Action<string> log)
            {
                _log = log;
            }

            public enum SendResult
            {
                OK,
                SendError,
                HulftrtLoadError,
                HulapiLoadError,
                ProcAddressError,
            }

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int LPUTLSENDEX(
                string lpszFileID,
                string lpszHostName,
                [MarshalAs(UnmanagedType.I1)] bool bResend,
                short nPriority,
                [MarshalAs(UnmanagedType.I1)] bool bSync,
                int nWait,
                string lpszFileName,
                string lpszGroup,
                [MarshalAs(UnmanagedType.I1)] bool bNp,
                IntPtr lpMsg,
                int nTransMode);

            public SendResult Send(
                string fileId,
                string fileName,
                string hostName,
                bool resend = false,
                short priority = 50,
                bool sync = true,
                int wait = 10,
                string group = "",
                bool np = false)
            {
                _log?.Invoke($"ファイルID:{fileId}");
                _log?.Invoke($"ファイル名:{fileName}");
                _log?.Invoke($"ホスト名:{hostName}");

                IntPtr hHulDll = IntPtr.Zero;
                IntPtr hApiDll = IntPtr.Zero;

                try
                {
                    hHulDll = NativeMethods.LoadLibrary("hulftrt.dll");
                    if (hHulDll == IntPtr.Zero)
                    {
                        _log?.Invoke(FormatWin32Error("hulftrt.dll の読み込みに失敗しました"));
                        return SendResult.HulftrtLoadError;
                    }

                    hApiDll = NativeMethods.LoadLibrary("hulapi.dll");
                    if (hApiDll == IntPtr.Zero)
                    {
                        _log?.Invoke(FormatWin32Error("hulapi.dll の読み込みに失敗しました"));
                        return SendResult.HulapiLoadError;
                    }

                    IntPtr funcPtr = NativeMethods.GetProcAddress(hApiDll, "utlsendex");
                    if (funcPtr == IntPtr.Zero)
                    {
                        _log?.Invoke(FormatWin32Error("utlsendex のアドレス取得に失敗しました"));
                        return SendResult.ProcAddressError;
                    }

                    var lpUtlsendex = (LPUTLSENDEX)Marshal.GetDelegateForFunctionPointer(funcPtr, typeof(LPUTLSENDEX));
                    var status = lpUtlsendex(fileId, hostName, resend, priority, sync, wait, fileName, group, np, IntPtr.Zero, 0);

                    if (status != 0)
                    {
                        _log?.Invoke($"HULFT API 呼び出しでエラーが発生しました (コード: {status})");
                        return SendResult.SendError;
                    }

                    _log?.Invoke("HULFT API 呼び出しが正常終了しました。");
                    return SendResult.OK;
                }
                finally
                {
                    if (hApiDll != IntPtr.Zero)
                    {
                        NativeMethods.FreeLibrary(hApiDll);
                    }

                    if (hHulDll != IntPtr.Zero)
                    {
                        NativeMethods.FreeLibrary(hHulDll);
                    }
                }
            }
        }

        private static class NativeMethods
        {
            [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
            public static extern IntPtr LoadLibrary(string lpFileName);

            [DllImport("kernel32", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool FreeLibrary(IntPtr hModule);

            [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = false)]
            public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);
        }
    }
}

using System;
using System.IO;

namespace HulftSchedulerApp.Services
{
    public class RealHulftSender : IHulftSender
    {
        private readonly bool _isInstalled;

        public RealHulftSender()
        {
            _isInstalled = IsHulftInstalled();
        }

        public HulftSendResult SendFile(string hulftFileId, string filePath)
        {
            if (!_isInstalled)
            {
                return new HulftSendResult
                {
                    Success = false,
                    Message = "HULFT未インストール"
                };
            }

            // 実際の HULFT 連携処理はここに実装する。
            // 現状はスタブとして成功のみを返す。
            var fileName = Path.GetFileName(filePath);
            return new HulftSendResult
            {
                Success = true,
                Message = $"{fileName} をHULFTに送信済み"
            };
        }

        public static bool IsHulftInstalled()
        {
            var env = Environment.GetEnvironmentVariable("HULFT_INSTALLED");
            if (!string.IsNullOrWhiteSpace(env) && env.Trim().Equals("1", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            var defaultPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "HULFT");
            return Directory.Exists(defaultPath);
        }
    }
}

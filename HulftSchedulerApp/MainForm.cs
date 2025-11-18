using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using HulftSchedulerApp.Logging;
using HulftSchedulerApp.Services;

namespace HulftSchedulerApp
{
    public partial class MainForm : Form
    {
        private readonly LogService _logService;
        private readonly IHulftSender _hulftSender;
        private readonly Regex _fileNameRegex = new Regex(@"^\d{12}\.csv$", RegexOptions.Compiled);

        private Queue<string> _pendingFiles = new Queue<string>();
        private string _currentFolder = string.Empty;
        private readonly Timer _sendTimer;
        private bool _isRunning;

        public MainForm()
        {
            InitializeComponent();

            _logService = new LogService();
            _logService.LogWritten += LogService_LogWritten;

            _hulftSender = HulftSenderFactory.Create(_logService);

            _sendTimer = new Timer();
            _sendTimer.Tick += SendTimer_Tick;
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "送信対象ファイルのフォルダを選択してください";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtFolderPath.Text = dialog.SelectedPath;
                    _currentFolder = dialog.SelectedPath;
                    LoadFilesFromFolder();
                }
            }
        }

        private void LoadFilesFromFolder()
        {
            cboStartFile.Items.Clear();

            if (string.IsNullOrWhiteSpace(_currentFolder) || !Directory.Exists(_currentFolder))
            {
                return;
            }

            var availableFiles = Directory.GetFiles(_currentFolder, "*.csv", SearchOption.TopDirectoryOnly)
                .Select(Path.GetFileName)
                .Where(name => name != null && _fileNameRegex.IsMatch(name))
                .OrderBy(name => name, StringComparer.Ordinal)
                .ToList();

            if (!availableFiles.Any())
            {
                _logService.Write("指定フォルダに送信対象ファイルがありません。");
                MessageBox.Show("対象ファイルが存在しません。yyyymmddHHMM.csv 形式のファイルを配置してください。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            cboStartFile.Items.AddRange(availableFiles.Cast<object>().ToArray());
            cboStartFile.SelectedIndex = 0;
            _logService.Write($"{availableFiles.Count} 件の送信対象ファイルを読み込みました。");
        }

        private void BtnStartStop_Click(object sender, EventArgs e)
        {
            if (_isRunning)
            {
                StopSending("ユーザー操作により送信を停止しました。");
            }
            else
            {
                StartSending();
            }
        }

        private void StartSending()
        {
            if (string.IsNullOrWhiteSpace(txtHulftId.Text))
            {
                MessageBox.Show("HULFTファイルIDを入力してください。", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(_currentFolder) || !Directory.Exists(_currentFolder))
            {
                MessageBox.Show("送信フォルダを選択してください。", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cboStartFile.Items.Count == 0 || cboStartFile.SelectedItem == null)
            {
                MessageBox.Show("開始ファイルを選択してください。", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (numInterval.Value <= 0)
            {
                MessageBox.Show("送信周期（秒）は1以上を指定してください。", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedFile = cboStartFile.SelectedItem.ToString();
            var files = Directory.GetFiles(_currentFolder, "*.csv", SearchOption.TopDirectoryOnly)
                .Select(Path.GetFileName)
                .Where(name => name != null && _fileNameRegex.IsMatch(name))
                .OrderBy(name => name, StringComparer.Ordinal)
                .ToList();

            var startIndex = files.FindIndex(f => f.Equals(selectedFile, StringComparison.OrdinalIgnoreCase));
            if (startIndex < 0)
            {
                MessageBox.Show("開始ファイルが見つかりません。", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _pendingFiles = new Queue<string>(files.Skip(startIndex));
            if (_pendingFiles.Count == 0)
            {
                MessageBox.Show("送信対象ファイルがありません。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _isRunning = true;
            btnStartStop.Text = "停止";
            btnBrowse.Enabled = false;
            cboStartFile.Enabled = false;
            numInterval.Enabled = false;
            txtHulftId.Enabled = false;

            var intervalMs = (int)numInterval.Value * 1000;
            _sendTimer.Interval = intervalMs;
            _sendTimer.Start();

            _logService.Write($"送信開始 (ファイルID: {txtHulftId.Text}, フォルダ: {_currentFolder}, 開始ファイル: {selectedFile}, 周期: {numInterval.Value}秒)");

            // 最初の送信を即時実行
            SendNextFile();
        }

        private void SendTimer_Tick(object sender, EventArgs e)
        {
            SendNextFile();
        }

        private void SendNextFile()
        {
            if (_pendingFiles == null || _pendingFiles.Count == 0)
            {
                StopSending("送信対象ファイルがなくなったため処理を終了します。");
                return;
            }

            var fileName = _pendingFiles.Dequeue();
            var fullPath = Path.Combine(_currentFolder, fileName);

            try
            {
                var result = _hulftSender.SendFile(txtHulftId.Text, fullPath);
                var status = result.Success ? "送信成功" : "送信失敗";
                _logService.Write($"{fileName} → {status} ({result.Message})");
            }
            catch (Exception ex)
            {
                _logService.Write($"{fileName} → 例外発生: {ex.Message}");
            }

            if (_pendingFiles.Count == 0)
            {
                StopSending("すべてのファイル送信が完了しました。");
            }
        }

        private void StopSending(string message)
        {
            if (_isRunning)
            {
                _sendTimer.Stop();
                _isRunning = false;
                btnStartStop.Text = "開始";
                btnBrowse.Enabled = true;
                cboStartFile.Enabled = true;
                numInterval.Enabled = true;
                txtHulftId.Enabled = true;
            }

            if (!string.IsNullOrWhiteSpace(message))
            {
                _logService.Write(message);
            }
        }

        private void LogService_LogWritten(object sender, string e)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.BeginInvoke(new Action(() => AppendLog(e)));
            }
            else
            {
                AppendLog(e);
            }
        }

        private void AppendLog(string log)
        {
            txtLog.AppendText(log + Environment.NewLine);
            txtLog.ScrollToCaret();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _sendTimer.Stop();
        }
    }
}

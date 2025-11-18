using System;
using System.IO;

namespace HulftSchedulerApp.Logging
{
    public class LogService
    {
        private readonly string _logFilePath;

        public event EventHandler<string> LogWritten;

        public LogService()
        {
            var logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            Directory.CreateDirectory(logDirectory);
            _logFilePath = Path.Combine(logDirectory, $"hulft_{DateTime.Now:yyyyMMdd}.log");
        }

        public void Write(string message)
        {
            var entry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
            LogWritten?.Invoke(this, entry);
            File.AppendAllText(_logFilePath, entry + Environment.NewLine);
        }
    }
}

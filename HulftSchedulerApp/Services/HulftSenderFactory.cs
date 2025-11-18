using System;
using System.Configuration;
using HulftSchedulerApp.Logging;

namespace HulftSchedulerApp.Services
{
    public static class HulftSenderFactory
    {
        public static IHulftSender Create(LogService logService)
        {
            var mode = (ConfigurationManager.AppSettings["HulftSenderMode"] ?? "Auto").Trim();

            switch (mode.ToUpperInvariant())
            {
                case "REAL":
                    logService?.Write("App.config で REAL が指定されたため RealHulftSender を利用します。");
                    return new RealHulftSender(logService);
                case "DUMMY":
                    logService?.Write("App.config で DUMMY が指定されたため DummyHulftSender を利用します。");
                    return new DummyHulftSender();
                default:
                    if (RealHulftSender.IsEnvironmentReady(out var reason))
                    {
                        logService?.Write($"HULFT 環境を検出したため RealHulftSender を利用します。({reason})");
                        return new RealHulftSender(logService);
                    }

                    logService?.Write($"HULFT 環境が整っていないため DummyHulftSender を利用します。({reason})");
                    return new DummyHulftSender();
            }
        }
    }
}

using HulftSchedulerApp.Logging;

namespace HulftSchedulerApp.Services
{
    public static class HulftSenderFactory
    {
        public static IHulftSender Create(LogService logService)
        {
            if (RealHulftSender.IsHulftInstalled())
            {
                logService?.Write("HULFTが検出されたため RealHulftSender を利用します。");
                return new RealHulftSender();
            }

            logService?.Write("HULFT未インストールのため DummyHulftSender で送信をスキップします。");
            return new DummyHulftSender();
        }
    }
}

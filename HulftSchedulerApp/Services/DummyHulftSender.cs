namespace HulftSchedulerApp.Services
{
    public class DummyHulftSender : IHulftSender
    {
        public HulftSendResult SendFile(string hulftFileId, string filePath)
        {
            return new HulftSendResult
            {
                Success = false,
                Message = "HULFT未インストールのため送信をスキップ"
            };
        }
    }
}

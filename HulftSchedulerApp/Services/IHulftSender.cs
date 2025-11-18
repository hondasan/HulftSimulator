namespace HulftSchedulerApp.Services
{
    public interface IHulftSender
    {
        HulftSendResult SendFile(string hulftFileId, string filePath);
    }
}

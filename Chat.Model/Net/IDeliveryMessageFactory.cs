namespace Chat.Model.Net
{
    public interface IDeliveryMessageFactory
    {
        IChatMessageListener CreateListener();
        IChatMessageSender CreateSender();
    }
}

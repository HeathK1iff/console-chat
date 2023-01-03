using System.Threading.Tasks;

namespace Chat.Model.Net
{
    public interface IChatMessageSender
    {
        Task SendMessageAsync(ChatMessage message);
    }
}

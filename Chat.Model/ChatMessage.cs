using System.Net;

namespace Chat.Model
{
    public struct ChatMessage 
    {
        public ChatMessage(EndPoint sender, string message)
        {
            Sender = sender;
            Message = message;
        }

        public EndPoint Sender { get; }
        public string Message { get; }
    }
}

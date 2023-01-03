using Chat.Model.Exceptions;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Model.Net.Sockets
{
    public class SocketChatMessageSender : IChatMessageSender
    {
        int _clientPort;

        public SocketChatMessageSender(int clientPort)
        {
            _clientPort = clientPort;
        }

        public async Task SendMessageAsync(ChatMessage message)
        {
            if (message.Sender is null)
                throw new UserNotDefinedException();

            using (var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                IPEndPoint clientEndPoint = message.Sender as IPEndPoint;
                var task = client.ConnectAsync(clientEndPoint.Address, _clientPort);
                if (await Task.WhenAny(task, Task.Delay(2000)) != task)
                {
                    throw new MessageNotDeliveredException(message.Sender);
                }
                
                byte[] buffer = Encoding.UTF8.GetBytes(message.Message);
                await client.SendAsync(new ArraySegment<byte>(buffer), SocketFlags.None);
                client.Shutdown(SocketShutdown.Send);
            }
        }
    }
}

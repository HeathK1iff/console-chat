using Console.Model.Net.Sockets;

namespace Chat.Console
{
    public class Program
    {
        static void Main()
        {
            new Chat.Model.Chat(new UserConsoleProvider(), 
                new SocketDeliveryMessageFactory(new GetIPAddressByHostProvider(), 8090)).Run();         
        }
    }
}

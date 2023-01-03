using Chat.Model;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Chat.Console
{
    public class GetIPAddressByHostProvider : IGetIPAddressByHostProvider
    {
        public IPAddress[] GetIpAddressByHost(string hostName)
        {
            return Dns.GetHostAddresses(Dns.GetHostName())
                                   .Where(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToArray();
        }
    }
}

using System.Net;

namespace Chat.Model
{
    public interface IGetIPAddressByHostProvider
    {
        IPAddress[] GetIpAddressByHost(string hostName);
    }
}

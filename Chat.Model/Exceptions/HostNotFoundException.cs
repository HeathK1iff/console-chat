using System.Net;

namespace Chat.Model.Exceptions
{
    public class HostNotFoundException : ChatBaseException
    {
        public HostNotFoundException(IPAddress address) : base($"{address.ToString()} not found")
        {
        }
    }
}

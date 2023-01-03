using Chat.Model;
using Chat.Model.Net;
using Chat.Model.Net.Sockets;
using System;

namespace Console.Model.Net.Sockets
{
    public class SocketDeliveryMessageFactory : IDeliveryMessageFactory
    {
        IGetIPAddressByHostProvider _getIpByHostProvider;
        int _listenPort;
        public SocketDeliveryMessageFactory(IGetIPAddressByHostProvider getIpByHostProvider, int listenPort)
        {
            _getIpByHostProvider = getIpByHostProvider;
            _listenPort = listenPort;
        }

        public IChatMessageListener CreateListener()
        {
            return new SocketChatMessageListener(_listenPort, _getIpByHostProvider);
        }

        public IChatMessageSender CreateSender()
        {
            return new SocketChatMessageSender(_listenPort);
        }
    }
}

using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;
using Chat.Model.Exceptions;
using System.Collections.Generic;

namespace Chat.Model.Net.Sockets
{
    public delegate void ReceiveMessagesDlg(ConcurrentQueue<ChatMessage> messages);
    public class SocketChatMessageListener: IChatMessageListener
    {
        static int DefaultMaxConnections = 10;
        static int DefaultMaxMessageBuffer = 255;

        IGetIPAddressByHostProvider _getIpByHostProvider;
        List<Thread> _threads;
        ConcurrentQueue<ChatMessage> _messageList;
        bool _isStop;
        int _listenPort;
        static object _lockList = new object();

        public SocketChatMessageListener(int listenPort, IGetIPAddressByHostProvider getIpByHostProvider)
        {
            _getIpByHostProvider = getIpByHostProvider;
            _messageList = new ConcurrentQueue<ChatMessage>();
            _threads = new List<Thread>();
            _listenPort = listenPort;
        }

        public bool IsStop { get => _isStop; }

        public void Start(Action<ConcurrentQueue<ChatMessage>> messageReceiveAction)
        {
            IPAddress[] ipAddrs = _getIpByHostProvider.GetIpAddressByHost(Dns.GetHostName());
            if (!ipAddrs.Any())
                throw new LocalIPNotFoundException();

            foreach (IPAddress addr in ipAddrs)
            {
                Thread thread = new Thread(async (args) =>
                {
                    var inArgs = args as Tuple<SocketChatMessageListener, IPEndPoint>;
                    EndPoint listenAddress = new IPEndPoint(inArgs.Item2.Address, _listenPort);
                    var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    listener.Bind(listenAddress);
                    listener.Listen(SocketChatMessageListener.DefaultMaxConnections);

                    while (!inArgs.Item1.IsStop)
                    {
                        using (var inSocket = await listener.AcceptAsync())
                        {
                            var buffer = new byte[SocketChatMessageListener.DefaultMaxMessageBuffer];
                            var received = await inSocket.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None);

                            var incomeEndPoint = inSocket.RemoteEndPoint as IPEndPoint;

                            _messageList.Enqueue(new ChatMessage(new IPEndPoint(incomeEndPoint.Address, _listenPort), Encoding.UTF8.GetString(buffer, 0, received)));

                            if (Monitor.IsEntered(_lockList))
                                continue;

                            lock (_lockList)
                            {
                                messageReceiveAction?.Invoke(_messageList);
                            }

                            inSocket.Shutdown(SocketShutdown.Receive);
                        }
                    }
                });

                _threads.Add(thread);
                thread.Start(new Tuple<SocketChatMessageListener, IPEndPoint>(this, new IPEndPoint(addr, _listenPort)));
            }
        }

        public void Stop()
        {
            _isStop = true;
        }

        public EndPoint[] GetHostEndPoints()
        {
            return _getIpByHostProvider.GetIpAddressByHost(Dns.GetHostName())
                .Select(f=> new IPEndPoint(f, _listenPort)).ToArray();
        }
    }
}

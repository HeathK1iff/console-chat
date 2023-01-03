using System;
using System.Collections.Concurrent;

namespace Chat.Model.Net
{
    public interface IChatMessageListener : IHostEndPoints
    {
        void Start(Action<ConcurrentQueue<ChatMessage>> messageReceiveAction);
        void Stop();
    }
}

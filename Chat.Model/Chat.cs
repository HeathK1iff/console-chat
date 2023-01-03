using Chat.Model.Commands;
using Chat.Model.Exceptions;
using Chat.Model.Net;
using Chat.Model.Net.Sockets;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;

namespace Chat.Model
{
    public class Chat
    {
        static string QuitCommand = "quit";

        Dictionary<EndPoint, string> _users;
        IUserCommandProvider _userCommandProvider;
        IDeliveryMessageFactory _deliveryMessageFactor;
        int _listenerPort;

        public Chat(IUserCommandProvider userCommandProvider,
            IDeliveryMessageFactory deliveryMessageFactor, 
            int port)
        {
            _deliveryMessageFactor = deliveryMessageFactor;
            _userCommandProvider = userCommandProvider;
            _users = new Dictionary<EndPoint, string>();
            _listenerPort = port;
        }

        private void ReceiveMessages(ConcurrentQueue<ChatMessage> messages)
        {
            while (messages.TryDequeue(out ChatMessage message))
            {
                var endPoint = message.Sender as IPEndPoint;

                string userName = _users.FirstOrDefault(f => (f.Key as IPEndPoint).Address.Equals((message.Sender as IPEndPoint).Address)).Value;

                if (userName is null)
                {
                    userName = $"user_{_users.Count}";
                    _users.Add(endPoint, userName);
                }

                if (userName != null)
                    _userCommandProvider.WriteLine($"{userName}<{message.Message}");
            }
        }

        private bool IsEnteredQuit(string commnad)
        {
            return string.Compare(commnad, Chat.QuitCommand, true) == 0;
        }

        public void Run()
        {
            var listener = _deliveryMessageFactor.CreateListener();
            var sender = _deliveryMessageFactor.CreateSender();
            try
            {
                listener.Start(ReceiveMessages);

                var hosts = string.Join(", ", listener.GetHostEndPoints().Select(f => f.ToString()));
                _userCommandProvider.WriteLine($"Listeners were started on: {hosts}");

                string textCommand = null;
                do
                {
                    textCommand = _userCommandProvider.ReadLine();
                    
                    if (IsEnteredQuit(textCommand))
                        break;

                    if (!string.IsNullOrEmpty(textCommand))
                    {
                        var command = BaseCommand.Create(_users, sender, textCommand);
                        if (command is null)
                        {
                            _userCommandProvider.WriteLine("Command not found");
                            continue;
                        }
                        try
                        {
                            var task = command.ExecuteAsync(textCommand);
                            task.ContinueWith((prevTask) =>
                            {
                                if (prevTask.IsFaulted)
                                {
                                    _userCommandProvider.WriteLine(prevTask.Exception.InnerException?.Message);
                                }
                            });
                        } 
                        catch (ChatBaseException e)
                        {
                            _userCommandProvider.WriteLine(e.Message);
                        }
                    }                    
                } while (true);
            }
            finally
            {
                listener.Stop();
            }
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chat.Model.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Model.Net;
using System.Net;

namespace Chat.Model.Commands.Tests
{
    [TestClass()]
    public class SendMessageCommandTests
    {
        class DummyChatMessageSender : IChatMessageSender
        {
            ChatMessage _message;
            public Task SendMessageAsync(ChatMessage message)
            {
                _message = message;
                return Task.Delay(1);
            }

            public ChatMessage Message { get => _message; }
        }


        [TestMethod()]
        public void SendMessageCommandTest()
        {
            Assert.ThrowsException<ArgumentNullException>(
               () => new SendMessageCommand(null, new DummyChatMessageSender())
               );
            Assert.ThrowsException<ArgumentNullException>(
                () => new SendMessageCommand(new Dictionary<System.Net.EndPoint, string>(), null)
                );
        }

        [TestMethod()]
        public void IsCommandTest()
        {
            Assert.IsTrue(SendMessageCommand.IsCommand("test>Hello World"));
        }

        [TestMethod()]
        public void ExecuteAsyncTest()
        {
            var users = new Dictionary<System.Net.EndPoint, string>();
            var sender = new DummyChatMessageSender();

            users.Add(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9090), "john");
            var command = new SendMessageCommand(users, sender);

            Assert.ThrowsExceptionAsync<ArgumentNullException>(() => command.ExecuteAsync(String.Empty));
            Assert.ThrowsExceptionAsync<ArgumentNullException>(() => command.ExecuteAsync("   "));
            Assert.ThrowsExceptionAsync<ArgumentNullException>(() => command.ExecuteAsync(null));

            command.ExecuteAsync("john>Hello World").Wait();
            Assert.IsTrue(sender.Message.Message.Equals("Hello World") && 
                sender.Message.Sender.Equals(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9090)));
        }
    }
}
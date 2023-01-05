using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chat.Model.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Model.Net;
using Chat.Model.Exceptions;
using System.Net;

namespace Chat.Model.Commands.Tests
{
    [TestClass()]
    public class AddUserCommandTests
    {
        class DummyChatMessageSender : IChatMessageSender
        {
            public Task SendMessageAsync(ChatMessage message)
            {
                throw new NotImplementedException();
            }
        }


        [TestMethod()]
        public void AddUserCommandTest()
        {
            Assert.ThrowsException<ArgumentNullException>(
                () => new AddUserCommand(null, new DummyChatMessageSender())
                );
            Assert.ThrowsException<ArgumentNullException>(
                () => new AddUserCommand(new Dictionary<System.Net.EndPoint, string>(), null)
                );
        }

        [TestMethod()]
        public void IsCommandTest()
        {
            Assert.IsTrue(AddUserCommand.IsCommand("test=4.21.5.6"));
            Assert.IsTrue(AddUserCommand.IsCommand("test=4.21.5.6:9000"));
        }

        [TestMethod()]
        public void ExecuteAsyncTest()
        {
            var users = new Dictionary<System.Net.EndPoint, string>();
            var command = new AddUserCommand(users, new DummyChatMessageSender());

            Assert.ThrowsExceptionAsync<ArgumentNullException>(() => command.ExecuteAsync(String.Empty));
            Assert.ThrowsExceptionAsync<ArgumentNullException>(() => command.ExecuteAsync("   "));
            Assert.ThrowsExceptionAsync<ArgumentNullException>(() => command.ExecuteAsync(null));
            Assert.ThrowsExceptionAsync<InvalidParceIPAddressException>(() => command.ExecuteAsync("test=234.122.114"));
            Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(() => command.ExecuteAsync("test=127.0.0.1:1"));
            Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(() => command.ExecuteAsync("test=127.0.0.1:999999"));

            command.ExecuteAsync("test=127.0.0.1:9090").Wait();
            Assert.IsTrue(users.TryGetValue(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9090), out string data));
        }
    }
}
using Chat.Model.Net;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Chat.Model.Commands
{
    public abstract class BaseCommand
    {
        protected Dictionary<EndPoint, string> _users;
        protected IChatMessageSender _sender;

        public BaseCommand(Dictionary<EndPoint, string> users, IChatMessageSender sender)
        {
            _users = users;
            _sender = sender;
        }

        public virtual async Task ExecuteAsync(string command) { }

        public static BaseCommand Create(Dictionary<EndPoint, string> users,
            IChatMessageSender sender, string textCommand)
        {
            BaseCommand command = null;
            if (SendMessageCommand.IsCommand(textCommand))
            {
                command = new SendMessageCommand(users, sender);
            } else
            if (AddUserCommand.IsCommand(textCommand))
            {
                command = new AddUserCommand(users, sender);
            };

            return command;
        }
    }
}

using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using Chat.Model.Net;
using System.Threading.Tasks;

namespace Chat.Model.Commands
{
    public class SendMessageCommand : BaseCommand
    {
        static string CommandPattern = @"^(\w+)>(.+)$";
        
        public SendMessageCommand(Dictionary<EndPoint, string> users, IChatMessageSender sender) : base(users, sender) {}

        static public bool IsCommand(string command)
        {
            return Regex.IsMatch(command, SendMessageCommand.CommandPattern);
        }

        private IPEndPoint GetEndPointByUserName(string userName)
        {
            if (_users.Values.Any(f => f == userName))
            {
                return _users.FirstOrDefault(f => f.Value == userName).Key as IPEndPoint;
            }

            return null;
        }

        private bool Split(string command, out EndPoint endPoint, out string message)
        {
            message = string.Empty;
            endPoint = null;

            var match = Regex.Match(command, SendMessageCommand.CommandPattern);
            if (!match.Success)
                return false;

            endPoint = GetEndPointByUserName(match.Groups[1].Value);
            message = match.Groups[2].Value;

            return true;
        }

        public override async Task ExecuteAsync(string command)
        {
            if (Split(command, out EndPoint endPoint, out string message))
            {
               await _sender.SendMessageAsync(new ChatMessage(endPoint, message));
            }
        }
    }
}

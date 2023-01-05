using Chat.Model.Exceptions;
using Chat.Model.Net;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Chat.Model.Commands
{
    public class AddUserCommand : BaseCommand
    {
        static string CommandPattern = @"^(\w+)=((?:[0-9]{1,3}\.){3}[0-9]{1,3})(:\d+)?$";
        public AddUserCommand(Dictionary<EndPoint, string> users, IChatMessageSender sender) : base(users, sender)
        {
        }

        static public bool IsCommand(string command)
        {
            return Regex.IsMatch(command, AddUserCommand.CommandPattern);
        }

        private bool Split(string command, out IPAddress ip, out string userName, out int port)
        {
            if (string.IsNullOrEmpty(command))
                throw new ArgumentNullException("Command can be null/empty");

            ip = null;
            port = 8090;
            userName = string.Empty;

            var match = Regex.Match(command, AddUserCommand.CommandPattern);
            if (!match.Success)
                return false;

            if (!IPAddress.TryParse(match.Groups[2].Value, out ip))
                throw new InvalidParceIPAddressException();

            userName = match.Groups[1].Value;

            if (!string.IsNullOrEmpty(match.Groups[3].Value.Trim()))
            {
                port = int.Parse(match.Groups[3].Value.Substring(1));
                if ((port < 1000) || (port > 65536))
                    throw new ArgumentOutOfRangeException($"Port should be between 1000 and 65536");
            }
            
            return true;
        }

        public override async Task ExecuteAsync(string command)
        {
            Split(command, out IPAddress ip, out string userName, out int port);
            
            var endPoint = new IPEndPoint(ip, port);

            if (_users.ContainsKey(endPoint))
                return;

           _users.Add(new IPEndPoint(ip, port), userName);   
        }
    }
}

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
        static string CommandPattern = @"^(\w+)=((?:[0-9]{1,3}\.){3}[0-9]{1,3})$";
        public AddUserCommand(Dictionary<EndPoint, string> users, IChatMessageSender sender) : base(users, sender)
        {
        }

        static public bool IsCommand(string command)
        {
            return Regex.IsMatch(command, AddUserCommand.CommandPattern);
        }

        private bool Split(string command, out IPAddress ip, out string userName)
        {
            ip = null;
            userName = string.Empty;

            var match = Regex.Match(command, AddUserCommand.CommandPattern);
            if (!match.Success)
                return false;

            if (!IPAddress.TryParse(match.Groups[2].Value, out ip))
                throw new InvalidParceIPAddressException();

            userName = match.Groups[1].Value;
            
            return true;
        }

        public override async Task ExecuteAsync(string command)
        {
            if (!Split(command, out IPAddress ip, out string userName))
                throw new ArgumentException();

            _users.Add(new IPEndPoint(ip, 8090), userName);
        }
    }
}

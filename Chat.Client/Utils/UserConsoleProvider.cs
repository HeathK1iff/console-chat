using Chat.Model.Net.Sockets;

namespace Chat.Console
{
    public struct UserConsoleProvider : IUserCommandProvider
    { 
        public string ReadLine()
        {
            return System.Console.ReadLine();
        }

        public void Write(string text)
        {
            System.Console.Write(text);
        }

        public void WriteLine(string text)
        {
            System.Console.WriteLine(text);
        }
    }
}

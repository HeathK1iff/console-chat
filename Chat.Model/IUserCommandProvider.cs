namespace Chat.Model.Net.Sockets
{
    public interface IUserCommandProvider
    {
        string ReadLine();
        void WriteLine(string text);
        void Write(string text);
    }
}

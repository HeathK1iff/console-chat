using System.Net;

namespace Chat.Model.Net
{
    public interface IHostEndPoints
    {
        EndPoint[] GetHostEndPoints();
    }
}

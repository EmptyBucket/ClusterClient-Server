using System.Net;

namespace ClusterServer.RequestHandler
{
    public interface IRequestHandler
    {
        void HandleAsync(HttpListenerContext listenerContext);
    }
}
using System.Net;

namespace ClusterClient.Clients
{
    public interface IRequestBuilder
    {
        HttpWebRequest Build(string uri);
    }
}
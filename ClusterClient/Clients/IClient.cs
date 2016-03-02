using System.Threading.Tasks;

namespace ClusterClient.Clients
{
    public interface IClient
    {
        Task<string> ProcessRequestAsync(string uri);
    }
}
using System.Threading.Tasks;

namespace ClusterClient.ClusterClient
{
    public interface IClusterClient
    {
        Task<string> ProcessRequestAsync(string query);
    }
}
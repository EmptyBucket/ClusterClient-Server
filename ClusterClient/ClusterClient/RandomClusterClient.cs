using System;
using System.Threading.Tasks;
using ClusterClient.Clients;

namespace ClusterClient.ClusterClient
{
    public class RandomClusterClient : IClusterClient
    {
        private readonly IClient _client;
        private readonly Random _random = new Random();

        protected string[] ReplicaAddresses { get; set; }

        public RandomClusterClient(IClient client, string[] replicaAddresses)
        {
            _client = client;
            ReplicaAddresses = replicaAddresses;
        }

        public async Task<string> ProcessRequestAsync(string query)
        {
            var randomUri = ReplicaAddresses[_random.Next(ReplicaAddresses.Length)];
            var uri = $"{randomUri}?query={query}";

            var resultTask = await _client.ProcessRequestAsync(uri);
            return resultTask;
        }
    }
}
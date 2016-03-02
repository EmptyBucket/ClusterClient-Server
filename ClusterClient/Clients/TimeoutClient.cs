using System;
using System.Threading.Tasks;

namespace ClusterClient.Clients
{
    public class TimeoutClient : Client
    {
        private readonly TimeSpan _timeout;

        public TimeoutClient(TimeSpan timeout)
        {
            _timeout = timeout;
        }

        public override async Task<string> ProcessRequestAsync(string uri)
        {
            var processRequest = base.ProcessRequestAsync(uri);
            await Task.WhenAny(processRequest, Task.Delay(_timeout));
            if (!processRequest.IsCompleted)
                throw new TimeoutException();
            return processRequest.Result;
        }
    }
}
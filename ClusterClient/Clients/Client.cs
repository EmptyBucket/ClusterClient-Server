using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ClusterClient.ClusterClient;
using log4net;

namespace ClusterClient.Clients
{
    public class Client : IClient
    {
        private readonly IRequestBuilder _requestBuilder;

        public Client(IRequestBuilder requestBuilder)
        {
            _requestBuilder = requestBuilder;
        }

        protected ILog Log => LogManager.GetLogger(typeof (RandomClusterClient));

        public virtual async Task<string> ProcessRequestAsync(string uri)
        {
            var request = _requestBuilder.Build(uri);
            var stopwatch = Stopwatch.StartNew();
            using (var response = await request.GetResponseAsync())
            using (var stream = response.GetResponseStream())
                if (stream != null)
                    using (var streamReader = new StreamReader(stream, Encoding.UTF8))
                    {
                        var result = await streamReader.ReadToEndAsync();
                        Log.InfoFormat(
                            $"Response from {request.RequestUri} received in {stopwatch.ElapsedMilliseconds} ms");
                        return result;
                    }
                else
                    return string.Empty;
        }
    }
}
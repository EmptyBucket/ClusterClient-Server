using System;

namespace ClusterServer.Processor
{
    public interface IHttpProcessor : IDisposable
    {
        void StartProcessingRequestsAsync();
        void StopProcessingRequests();
    }
}
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ClusterServer.ResponseBuilder;

namespace ClusterServer.RequestHandler
{
    public class RequestHandler : IRequestHandler
    {
        private static int _requestId;
        private readonly IResponseBuilder _hashResponseBuilder;
        private readonly int _methodDuration;

        public RequestHandler(IResponseBuilder hashResponseBuilder, int methodDuration)
        {
            _hashResponseBuilder = hashResponseBuilder;
            _methodDuration = methodDuration;
        }

        public async void HandleAsync(HttpListenerContext listenerContext)
        {
            var currentRequestId = Interlocked.Increment(ref _requestId);
            Console.WriteLine(
                $"Thread #{Thread.CurrentThread.ManagedThreadId} received request #{currentRequestId} at {DateTime.Now.TimeOfDay}");

            Thread.Sleep(_methodDuration);
            var queryString = listenerContext.Request.QueryString["query"];
            var encryptedBytes = await Task.Run(() => _hashResponseBuilder.Build(queryString));
            await listenerContext.Response.OutputStream.WriteAsync(encryptedBytes, 0, encryptedBytes.Length);

            Console.WriteLine(
                $"Thread #{Thread.CurrentThread.ManagedThreadId} sent response #{currentRequestId} at {DateTime.Now.TimeOfDay}");
        }
    }
}
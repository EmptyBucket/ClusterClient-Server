using System;
using System.Net;
using ClusterServer.RequestHandler;
using log4net;

namespace ClusterServer.Processor
{
    public class HttpProcessor : IHttpProcessor
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (HttpProcessor));

        private readonly HttpListener _listener;
        private readonly IRequestHandler _requestHandler;

        public HttpProcessor(HttpListener listener, IRequestHandler requestHandler)
        {
            _listener = listener;
            _requestHandler = requestHandler;
        }

        public async void StartProcessingRequestsAsync()
        {
            _listener.Start();
            Console.WriteLine($"Server started listening prefixes: {string.Join(";", _listener.Prefixes)}");

            while (true)
            {
                try
                {
                    var context = await _listener.GetContextAsync();

                    try
                    {
                       _requestHandler.HandleAsync(context);
                    }
                    catch (Exception e)
                    {
                        Log.Error(e);
                    }
                    finally
                    {
                        context.Response.Close();
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        public void StopProcessingRequests() => _listener.Stop();

        public void Dispose() => StopProcessingRequests();
    }
}
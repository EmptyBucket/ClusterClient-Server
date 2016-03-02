using System;
using System.Net;

namespace ClusterClient.Clients
{
    public class RequestBuilder : IRequestBuilder
    {
        public HttpWebRequest Build(string uriStr)
        {
            var request = WebRequest.CreateHttp(Uri.EscapeUriString(uriStr));
            request.Proxy = null;
            request.KeepAlive = true;
            request.ServicePoint.UseNagleAlgorithm = false;
            request.ServicePoint.ConnectionLimit = 100500;
            return request;
        } 
    }
}
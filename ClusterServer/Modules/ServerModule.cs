using System.Net;
using System.Text;
using ClusterServer.Processor;
using ClusterServer.RequestHandler;
using ClusterServer.ResponseBuilder;
using Ninject.Modules;

namespace ClusterServer.Modules
{
    public class ServerModule : NinjectModule
    {
        private readonly ServerArguments _serverArguments;

        public ServerModule(ServerArguments serverArguments)
        {
            _serverArguments = serverArguments;
        }

        public override void Load()
        {
            var listener = new HttpListener
            {
                Prefixes =
                {
                    $"http://+:{_serverArguments.Port}/{_serverArguments.MethodName}/"
                }
            };
            var key = Encoding.UTF8.GetBytes("Контур.Шпора");

            Bind<IHttpProcessor>().To<HttpProcessor>()
                .WithConstructorArgument("listener", listener);
            Bind<IRequestHandler>().To<RequestHandler.RequestHandler>();
            Bind<IResponseBuilder>().To<HashResponseBuilder>()
                .WithConstructorArgument("encoding", Encoding.UTF8)
                .WithConstructorArgument("key", key);
        }
    }
}
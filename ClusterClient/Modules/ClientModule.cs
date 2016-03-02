using ClusterClient.Clients;
using ClusterClient.ClusterClient;
using Ninject.Modules;

namespace ClusterClient.Modules
{
    public class ClientModule : NinjectModule
    {
        private readonly ClientArguments _clientArguments;

        public ClientModule(ClientArguments clientArguments)
        {
            _clientArguments = clientArguments;
        }

        public override void Load()
        {
            Bind<IClient>().To<TimeoutClient>()
                .WithConstructorArgument("timeout", _clientArguments.Timeout);
            Bind<IClusterClient>().To<RandomClusterClient>()
                .WithConstructorArgument("replicaAddresses", _clientArguments.ReplicaAddresses);
            Bind<IRequestBuilder>().To<RequestBuilder>();
        }
    }
}
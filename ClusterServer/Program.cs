using System;
using ClusterServer.Modules;
using ClusterServer.Processor;
using log4net;
using log4net.Config;
using Ninject;

namespace ClusterServer
{
    public static class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (Program));

        public static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            try
            {
                ServerArguments parsedArguments;
                if (!ServerArguments.TryGetArguments(args, out parsedArguments))
                    return;

                var kernel = new StandardKernel(new ServerModule(parsedArguments));
                using (var httpProcessor = kernel.Get<IHttpProcessor>())
                    httpProcessor.StartProcessingRequestsAsync();
            }
            catch (Exception e)
            {
                Log.Fatal(e);
            }
        }
    }
}
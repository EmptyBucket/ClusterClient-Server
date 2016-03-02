using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ClusterClient.ClusterClient;
using ClusterClient.Modules;
using log4net;
using log4net.Config;
using Ninject;

namespace ClusterClient
{
    public class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (Program));

        private static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            ClientArguments clientArguments;
            if (!ClientArguments.TryGetArguments(args, out clientArguments))
                return;

            TestClients(clientArguments);
        }

        private static void TestClients(ClientArguments clientArguments)
        {
            try
            {
                var kernel = new StandardKernel(new ClientModule(clientArguments));
                const int countClient = 10;
                var clients = Enumerable.Repeat(kernel.Get<IClusterClient>(), countClient);
                var queries = new[]
                {"От", "топота", "копыт", "пыль", "по", "полю", "летит", "На", "дворе", "трава", "на", "траве", "дрова"};

                foreach (var client in clients)
                    TestClient(client, queries);
            }
            catch (Exception e)
            {
                Log.Fatal(e);
            }
        }

        private static void TestClient(IClusterClient client, IEnumerable<string> queries)
        {
            Console.WriteLine("Testing {0} started", client.GetType());
            var tasks = queries.Select(async query =>
                {
                    var timer = Stopwatch.StartNew();
                    try
                    {
                        await client.ProcessRequestAsync(query);
                        Console.WriteLine("Processed query \"{0}\" in {1} ms", query, timer.ElapsedMilliseconds);
                    }
                    catch (TimeoutException)
                    {
                        Console.WriteLine("Query \"{0}\" timeout ({1} ms)", query, timer.ElapsedMilliseconds);
                    }
                }).ToArray();
            Task.WaitAll(tasks);
            Console.WriteLine("Testing {0} finished", client.GetType());
        }
    }
}
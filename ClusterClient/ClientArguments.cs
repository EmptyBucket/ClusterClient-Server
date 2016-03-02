using System;
using System.IO;
using Fclp;

namespace ClusterClient
{
    public class ClientArguments
    {
        public string[] ReplicaAddresses { get; set; }

        public TimeSpan Timeout { get; set; }

        public static bool TryGetArguments(string[] args, out ClientArguments clientArguments)
        {
            var argumentsParser = new FluentCommandLineParser();
            var tmpClientArguments = new ClientArguments();

            argumentsParser.Setup<string>('f', "file")
                .WithDescription("Path to the file with replica addresses")
                .Callback(fileName => tmpClientArguments.ReplicaAddresses = File.ReadAllLines(fileName))
                .Required();

            argumentsParser.Setup<TimeSpan>('t', "timeout")
                .WithDescription("Timeout")
                .Callback(timeout => tmpClientArguments.Timeout = timeout)
                .SetDefault(TimeSpan.FromSeconds(6));

            argumentsParser.SetupHelp("?", "h", "help")
                .Callback(text => Console.WriteLine(text));

            var parsingResult = argumentsParser.Parse(args);

            if (parsingResult.HasErrors)
            {
                argumentsParser.HelpOption.ShowHelp(argumentsParser.Options);
                clientArguments = null;
                return false;
            }

            clientArguments = tmpClientArguments;
            return !parsingResult.HasErrors;
        }
    }
}
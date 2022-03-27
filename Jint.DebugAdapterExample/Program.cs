using Jint.DebugAdapter;
using Jither.DebugAdapter;
using Jither.DebugAdapter.Helpers;

namespace Jint.DebugAdapterExample
{
    public class Program
    {
        private static Dictionary<string, Action<Options>> DemosById = new()
        {
            ["files"] = DemoFiles,
            ["internal"] = DemoInternal
        };

        private class Options
        {
            public string DemoId { get; } = "files";
            public string? Endpoint { get; }

            public Options(string[] args)
            {
                foreach (var arg in args)
                {
                    if (DemosById.ContainsKey(arg))
                    {
                        DemoId = arg;
                        continue;
                    }
                    Endpoint = arg;
                }
            }
        }

        public static void Main(string[] args)
        {
            LogManager.Level = LogLevel.Verbose;
            LogManager.Provider = new ConsoleLogProvider();

            var options = new Options(args);
            var demo = DemosById[options.DemoId];

            demo(options);
        }

        private static Endpoint CreateEndpoint(string? endpoint)
        {
            if (endpoint == null)
            {
                return new StdInOutEndpoint();
            }
            if (Int32.TryParse(endpoint, out int port))
            {
                return new TcpEndpoint(port);
            }
            return new NamedPipeEndpoint(endpoint);
        }

        private static void DemoFiles(Options options)
        {
            var endpoint = CreateEndpoint(options.Endpoint);

            var host = new FilesScriptHost();
            var adapter = new JintAdapter(host, host.Engine, endpoint);
            host.RegisterConsole(adapter.Console);

            adapter.StartListening();
        }

        private static void DemoInternal(Options options)
        {
            var endpoint = CreateEndpoint(options.Endpoint);

            var host = new InternalScriptHost();
            var adapter = new JintAdapter(host, host.Engine, endpoint);
            host.RegisterConsole(adapter.Console);

            adapter.StartListening();
        }
    }
}
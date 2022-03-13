using Jint.DebugAdapter;
using Jither.DebugAdapter;
using Jither.DebugAdapter.Helpers;

namespace Jint.DebugAdapterExample
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            LogManager.Level = LogLevel.Verbose;
            LogManager.Provider = new ConsoleLogProvider();

            var logger = LogManager.GetLogger();

            logger.Info("Started");
            Endpoint endpoint;
            var host = new ScriptHost();
            var adapter = new JintAdapter(host.Debugger, host, registerConsole: true);

            if (args.Length > 0)
            {
                if (Int32.TryParse(args[0], out int port))
                {
                    endpoint = new TcpEndpoint(adapter, port);
                }
                else
                {
                    endpoint = new NamedPipeEndpoint(adapter, args[0]);
                }
            }
            else
            {
                endpoint = new StdInOutEndpoint(adapter);
            }

            endpoint.Initialize();
            await endpoint.StartAsync();
        }
    }
}
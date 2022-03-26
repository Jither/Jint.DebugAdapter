using Jint.DebugAdapter;
using Jither.DebugAdapter;
using Jither.DebugAdapter.Helpers;

namespace Jint.DebugAdapterExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LogManager.Level = LogLevel.Verbose;
            LogManager.Provider = new ConsoleLogProvider();

            var logger = LogManager.GetLogger();

            logger.Info("Started");
            var endpoint = CreateEndpoint(args);

            var host = new ScriptHost();
            var adapter = new JintAdapter(host, host.Engine, endpoint);
            host.RegisterConsole(adapter.Console);

            adapter.StartListening();

            // TODO: This is for testing script execution after disconnect. Remove when working.
            System.Console.ReadKey();
        }

        private static Endpoint CreateEndpoint(string[] args)
        {
            if (args.Length > 0)
            {
                if (Int32.TryParse(args[0], out int port))
                {
                    return new TcpEndpoint(port);
                }
                
                return new NamedPipeEndpoint(args[0]);
            }
            
            return new StdInOutEndpoint();
        }
    }
}
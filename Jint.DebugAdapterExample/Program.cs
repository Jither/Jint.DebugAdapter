using Jint.DebugAdapter;
using Jint.DebugAdapter.Endpoints;

namespace Jint.DebugAdapterExample
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Logger.Log("Started");
            Endpoint endpoint;
            var host = new ScriptHost();
            var adapter = new JintAdapter(host.Debugger);

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
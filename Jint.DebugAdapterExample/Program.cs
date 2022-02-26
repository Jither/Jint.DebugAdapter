using Jint.DebugAdapter;
using Jint.DebugAdapter.Protocol;

namespace Jint.DebugAdapterExample
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Logger.Log("Started");
            Endpoint endpoint;
            var adapter = new JintAdapter();

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
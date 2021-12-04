using Jint.DebugAdapter;

namespace Jint.DebugAdapterExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Adapter adapter;
            if (args.Length > 0)
            {
                if (Int32.TryParse(args[0], out int port))
                {
                    adapter = new TcpAdapter(port);
                }
                else
                {
                    adapter = new NamedPipeAdapter(args[0]);
                }
            }
            else
            {
                adapter = new StdInOutAdapter();
            }

            adapter.Start();
        }
    }
}
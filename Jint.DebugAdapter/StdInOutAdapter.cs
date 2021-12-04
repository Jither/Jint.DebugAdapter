using Jint.DebugAdapter.Protocol;

namespace Jint.DebugAdapter
{
    public class StdInOutAdapter : Adapter
    {
        public StdInOutAdapter()
        {
        }

        public override void Start()
        {
            Stream input = Console.OpenStandardInput();
            Stream output = Console.OpenStandardOutput();
            var session = new DebugAdapterSession(input, output);
            session.Start();
        }
    }
}
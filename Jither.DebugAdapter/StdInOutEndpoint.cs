namespace Jither.DebugAdapter
{
    public class StdInOutEndpoint : Endpoint
    {
        public StdInOutEndpoint()
        {
        }

        protected override void StartListening(Adapter adapter)
        {
            var input = Console.OpenStandardInput();
            var output = Console.OpenStandardOutput();
            InitializeStreams(adapter, input, output);
        }
    }
}

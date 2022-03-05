namespace Jither.DebugAdapter
{
    public class StdInOutEndpoint : Endpoint
    {
        public StdInOutEndpoint(Adapter adapter) : base(adapter)
        {
        }

        protected override void StartListening()
        {
            var input = Console.OpenStandardInput();
            var output = Console.OpenStandardOutput();
            InitializeStreams(input, output);
        }
    }
}

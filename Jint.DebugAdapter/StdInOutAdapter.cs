namespace Jint.DebugAdapter
{
    public class StdInOutAdapter : Adapter
    {
        public StdInOutAdapter()
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

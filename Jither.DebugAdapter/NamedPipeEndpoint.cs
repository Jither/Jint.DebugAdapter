using System.IO.Pipes;

namespace Jither.DebugAdapter
{
    public class NamedPipeEndpoint : Endpoint
    {
        private readonly string name;

        public NamedPipeEndpoint(string name)
        {
            this.name = name;
        }

        protected override void StartListening(Adapter adapter)
        {
            using (var namedPipe = new NamedPipeServerStream(name, PipeDirection.InOut))
            {
                namedPipe.WaitForConnection();
                InitializeStreams(adapter, namedPipe, namedPipe);
            }
        }
    }
}

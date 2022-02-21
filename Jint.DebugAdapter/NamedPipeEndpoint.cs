using System.IO.Pipes;

namespace Jint.DebugAdapter
{
    public class NamedPipeEndpoint : Endpoint
    {
        private readonly string name;

        public NamedPipeEndpoint(Adapter adapter, string name) : base(adapter)
        {
            this.name = name;
        }

        protected override void StartListening()
        {
            using (var namedPipe = new NamedPipeServerStream(name, PipeDirection.InOut))
            {
                namedPipe.WaitForConnection();
                InitializeStreams(namedPipe, namedPipe);
            }
        }
    }
}

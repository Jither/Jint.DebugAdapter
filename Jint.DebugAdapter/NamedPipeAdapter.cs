using System.IO.Pipes;

namespace Jint.DebugAdapter
{
    public class NamedPipeAdapter : Adapter
    {
        private readonly string name;
        public NamedPipeAdapter(string name)
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

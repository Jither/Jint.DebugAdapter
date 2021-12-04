using System.IO.Pipes;
using Jint.DebugAdapter.Protocol;

namespace Jint.DebugAdapter
{
    public class NamedPipeAdapter : Adapter
    {
        private readonly string name;

        public NamedPipeAdapter(string name)
        {
            this.name = name;
        }

        public override void Start()
        {
            var pipeServer = new NamedPipeServerStream(name, PipeDirection.InOut);
            var session = new DebugAdapterSession(pipeServer, pipeServer);
            session.Start();
        }
    }
}
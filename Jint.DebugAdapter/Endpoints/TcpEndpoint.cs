using System.Net;
using System.Net.Sockets;

namespace Jint.DebugAdapter.Endpoints
{
    public class TcpEndpoint : Endpoint
    {
        private readonly int port;

        public TcpEndpoint(Adapter adapter, int port = 4711) : base(adapter)
        {
            this.port = port;
        }

        protected override void StartListening()
        {
            var listener = new TcpListener(IPAddress.Loopback, port);
            listener.Start();
            Logger.Log($"Listening on {listener.LocalEndpoint}");
            var client = listener.AcceptTcpClient();
            var stream = client.GetStream();
            InitializeStreams(stream, stream);
        }
    }
}

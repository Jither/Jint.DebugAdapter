using System.Net;
using System.Net.Sockets;

namespace Jither.DebugAdapter
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
            logger.Info($"Listening on {listener.LocalEndpoint}");
            var client = listener.AcceptTcpClient();
            var stream = client.GetStream();
            InitializeStreams(stream, stream);
        }
    }
}

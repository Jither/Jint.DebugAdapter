using System.Net;
using System.Net.Sockets;
using Jint.DebugAdapter.Protocol;

namespace Jint.DebugAdapter
{
    public class TcpAdapter : Adapter
    {
        private readonly TcpListener listener;

        public TcpAdapter(int port)
        {
            listener = new TcpListener(IPAddress.Loopback, port);
        }

        public override void Start()
        {
            listener.Start();
            while (true)
            {
                var client = listener.AcceptTcpClient();
                var stream = client.GetStream();
                var session = new DebugAdapterSession(stream, stream);
                session.Start();
                client.Close();
            }
        }
    }
}
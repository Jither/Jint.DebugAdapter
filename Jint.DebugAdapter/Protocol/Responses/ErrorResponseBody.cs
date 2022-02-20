using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    internal class ErrorResponseBody : ProtocolResponseBody
    {
        public Message Error { get; set; }

        public ErrorResponseBody()
        {

        }

        public ErrorResponseBody(Exception ex)
        {

        }
    }
}

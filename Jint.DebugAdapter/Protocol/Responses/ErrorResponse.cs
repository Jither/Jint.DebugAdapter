using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    public class ErrorResponse : ProtocolResponseBody
    {
        public Message Error { get; set; }

        public ErrorResponse()
        {

        }

        public ErrorResponse(Exception ex)
        {

        }
    }
}

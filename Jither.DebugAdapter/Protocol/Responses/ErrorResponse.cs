using Jither.DebugAdapter.Protocol.Types;

namespace Jither.DebugAdapter.Protocol.Responses
{
    public class ErrorResponse : ProtocolResponseBody
    {
        public Message Error { get; set; }

        public ErrorResponse()
        {

        }

        public ErrorResponse(Exception ex)
        {
            // TODO: Implement proper error response
        }
    }
}

using Jither.DebugAdapter.Protocol.Types;

namespace Jither.DebugAdapter.Protocol.Responses
{
    public class ErrorResponse : ProtocolResponseBody
    {
        private int _nextId;

        private int NextId => _nextId++;

        public Message Error { get; set; }

        public ErrorResponse()
        {

        }

        public ErrorResponse(Exception ex)
        {
            Error = new Message(NextId, ex.Message);
        }
    }
}

namespace Jint.DebugAdapter.Protocol.Responses
{
    public class ContinueResponse : ProtocolResponseBody
    {
        public bool? AllThreadsContinued { get; set; }
    }
}

namespace Jint.DebugAdapter.Protocol.Responses
{
    public class RunInTerminalResponse : ProtocolResponseBody
    {
        public int? ProcessId { get; set; }
        public int? ShellProcessId { get; set; }
    }
}

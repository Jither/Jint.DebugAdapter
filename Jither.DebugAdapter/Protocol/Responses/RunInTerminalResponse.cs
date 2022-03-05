namespace Jither.DebugAdapter.Protocol.Responses
{
    /// <summary>
    /// Response to ‘runInTerminal’ request.
    /// </summary>
    public class RunInTerminalResponse : ProtocolResponseBody
    {
        /// <summary>
        /// The process ID. The value should be less than or equal to 2147483647 (2^31-1).
        /// </summary>
        public int? ProcessId { get; set; }

        /// <summary>
        /// The process ID of the terminal shell. The value should be less than or equal to 2147483647 (2^31-1).
        /// </summary>
        public int? ShellProcessId { get; set; }
    }
}

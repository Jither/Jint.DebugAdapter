using Jint.DebugAdapter.Helpers;
using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Requests
{
    /// <summary>
    /// This optional request is sent from the debug adapter to the client to run a command in a terminal.
    /// </summary>
    /// <remarks>
    /// This is typically used to launch the debuggee in a terminal provided by the client.
    /// This request should only be called if the client has passed the value true for the 
    /// ‘supportsRunInTerminalRequest’ capability of the ‘initialize’ request.
    /// </remarks>
    public class RunInTerminalArguments : ProtocolArguments
    {
        /// <summary>
        /// What kind of terminal to launch.
        /// </summary>
        public StringEnum<TerminalKind>? Kind { get; set; }

        /// <summary>
        /// Optional title of the terminal.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Working directory for the command.
        /// </summary>
        /// <remarks>
        /// For non-empty, valid paths this typically results in execution of a change directory command.
        /// </remarks>
        public string Cwd { get; set; }

        /// <summary>
        /// List of arguments. The first argument is the command to run.
        /// </summary>
        public List<string> Args { get; set; }

        /// <summary>
        /// Environment key-value pairs that are added to or removed from the default environment.
        /// </summary>
        public Dictionary<string, string> Env { get; set; }
    }
}

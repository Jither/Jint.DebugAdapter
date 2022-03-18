using System.Text.Json.Serialization;

namespace Jither.DebugAdapter.Protocol.Requests
{
    /// <summary>
    /// This launch request is sent from the client to the debug adapter to start the debuggee with or without
    /// debugging (if ‘noDebug’ is true).
    /// </summary>
    /// <remarks>
    /// Since launching is debugger/runtime specific, the arguments for this request are
    /// not part of this specification.
    /// </remarks>
    public class LaunchArguments : ConfigurationArguments
    {

    }
}

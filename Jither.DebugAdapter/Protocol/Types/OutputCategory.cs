using Jither.DebugAdapter.Helpers;

namespace Jither.DebugAdapter.Protocol.Types
{
    /// <completionlist cref="OutputCategory"/>
    public class OutputCategory : StringEnum<OutputCategory>
    {
        /// <summary>
        /// Show the output in the client's default message UI, e.g. a 'debug console'.
        /// </summary>
        /// <remarks>
        /// This category should only be used for informational output from the debugger (as opposed to the debuggee).
        /// </remarks>
        public static readonly OutputCategory Console = Create("console");

        /// <summary>
        /// A hint for the client to show the ouput in the client's UI for important and highly visible information,
        /// e.g. as a popup notification.
        /// </summary>
        /// <remarks>
        /// This category should only be used for important messages from the debugger (as opposed to the debuggee).
        /// Since this category value is a hint, clients might ignore the hint and assume the 'console' category.
        /// </summary>
        public static readonly OutputCategory Important = Create("important");

        /// <summary>
        /// Show the output as normal program output from the debuggee.
        /// </summary>
        public static readonly OutputCategory Stdout = Create("stdout");

        /// <summary>
        /// Show the output as error program output from the debuggee.
        /// </summary>
        public static readonly OutputCategory Stderr = Create("stderr");

        /// <summary>
        /// Send the output to telemetry instead of showing it to the user.
        /// </summary>
        public static readonly OutputCategory Telemetry = Create("telemetry");
    }
}

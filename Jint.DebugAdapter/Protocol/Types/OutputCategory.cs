namespace Jint.DebugAdapter.Protocol.Types
{
    public enum OutputCategory
    {
        Other,
        /// <summary>
        /// Show the output in the client's default message UI, e.g. a 'debug console'.
        /// </summary>
        /// <remarks>
        /// This category should only be used for informational output from the debugger (as opposed to the debuggee).
        /// </remarks>
        Console,

        /// <summary>
        /// A hint for the client to show the ouput in the client's UI for important and highly visible information,
        /// e.g. as a popup notification.
        /// </summary>
        /// <remarks>
        /// This category should only be used for important messages from the debugger (as opposed to the debuggee).
        /// Since this category value is a hint, clients might ignore the hint and assume the 'console' category.
        /// </summary>
        Important,

        /// <summary>
        /// Show the output as normal program output from the debuggee.
        /// </summary>
        Stdout,

        /// <summary>
        /// Show the output as error program output from the debuggee.
        /// </summary>
        Stderr,

        /// <summary>
        /// Send the output to telemetry instead of showing it to the user.
        /// </summary>
        Telemetry
    }
}

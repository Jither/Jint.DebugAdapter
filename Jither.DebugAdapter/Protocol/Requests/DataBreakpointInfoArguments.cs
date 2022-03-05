namespace Jither.DebugAdapter.Protocol.Requests
{
    /// <summary>
    /// Obtains information on a possible data breakpoint that could be set on an expression or variable.
    /// </summary>
    /// <remarks>
    /// Clients should only call this request if the capability ‘supportsDataBreakpoints’ is true.
    /// </remarks>
    public class DataBreakpointInfoArguments : ProtocolArguments
    {
        /// <summary>
        /// Reference to the Variable container if the data breakpoint is requested for a child of the container.
        /// </summary>
        public int? VariablesReference { get; set; }

        /// <summary>
        /// The name of the Variable's child to obtain data breakpoint information for.
        /// If variablesReference isn't provided, this can be an expression.
        /// </summary>
        public string Name { get; set; }
    }
}

using System.Text.Json.Serialization;

namespace Jither.DebugAdapter.Protocol.Types
{
    /// <summary>
    /// Properties of a breakpoint or logpoint passed to the setBreakpoints request.
    /// </summary>
    public class SourceBreakpoint
    {
        /// <param name="line">The source line of the breakpoint or logpoint.</param>
        /// <param name="column">An optional source column of the breakpoint.</param>
        [JsonConstructor]
        public SourceBreakpoint(int line, int? column = null)
        {
            Line = line;
            Column = column;
        }

        /// <summary>
        /// The source line of the breakpoint or logpoint.
        /// </summary>
        public int Line { get; set; }

        /// <summary>
        /// An optional source column of the breakpoint.
        /// </summary>
        public int? Column { get; set; }

        /// <summary>
        /// An optional expression for conditional breakpoints. It is only honored by a debug adapter if the
        /// capability 'supportsConditionalBreakpoints' is true.
        /// </summary>
        public string Condition { get; set; }

        /// <summary>
        /// An optional expression that controls how many hits of the breakpoint are ignored.
        /// </summary>
        /// <remarks>
        /// The backend is expected to interpret the expression as needed. The attribute is only honored by a debug
        /// adapter if the capability 'supportsHitConditionalBreakpoints' is true.
        /// </remarks>
        public string HitCondition { get; set; }

        /// <summary>
        /// If this attribute exists and is non-empty, the backend must not 'break' (stop) but log the message
        /// instead. Expressions within { } are interpolated.
        /// </summary>
        /// <remarks>
        /// The attribute is only honored by a debug adapter if the capability 'supportsLogPoints' is true.
        /// </remarks>
        public string LogMessage { get; set; }
    }
}

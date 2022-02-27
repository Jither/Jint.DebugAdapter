using System.Text.Json.Serialization;

namespace Jint.DebugAdapter.Protocol.Types
{
    /// <summary>
    /// Properties of a breakpoint location returned from the ‘breakpointLocations’ request.
    /// </summary>
    public class BreakpointLocation
    {
        /// <param name="line">Start line of breakpoint location.</param>
        /// <param name="column">Optional start column of breakpoint location.</param>
        /// <param name="endLine">Optional end line of breakpoint location if the location covers a range.</param>
        /// <param name="endColumn">Optional end column of breakpoint location if the location covers a range.</param>
        [JsonConstructor]
        public BreakpointLocation(int line, int? column = null, int? endLine = null, int? endColumn = null)
        {
            Line = line;
            Column = column;
            EndLine = endLine;
            EndColumn = endColumn;
        }

        /// <summary>
        /// Start line of breakpoint location.
        /// </summary>
        public int Line { get; set; }

        /// <summary>
        /// Optional start column of breakpoint location.
        /// </summary>
        public int? Column { get; set; }

        /// <summary>
        /// Optional end line of breakpoint location if the location covers a range.
        /// </summary>
        public int? EndLine { get; set; }

        /// <summary>
        /// Optional end column of breakpoint location if the location covers a range.
        /// </summary>
        public int? EndColumn { get; set; }
    }
}

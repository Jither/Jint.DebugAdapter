namespace Jint.DebugAdapter.Protocol.Types
{
    /// <summary>
    /// Information about a Breakpoint created in setBreakpoints, setFunctionBreakpoints, setInstructionBreakpoints,
    /// or setDataBreakpoints.
    /// </summary>
    public class Breakpoint
    {
        /// <summary>
        /// An optional identifier for the breakpoint. It is needed if breakpoint events are used to update or remove
        /// breakpoints.
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// If true breakpoint could be set (but not necessarily at the desired location).
        /// </summary>
        public bool Verified { get; set; }

        /// <summary>
        /// An optional message about the state of the breakpoint. This is shown to the user and can be used to
        /// explain why a breakpoint could not be verified.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The source where the breakpoint is located.
        /// </summary>
        public Source Source { get; set; }

        /// <summary>
        /// The start line of the actual range covered by the breakpoint.
        /// </summary>
        public int? Line { get; set; }

        /// <summary>
        /// An optional start column of the actual range covered by the breakpoint.
        /// </summary>
        public int? Column { get; set; }

        /// <summary>
        /// An optional end line of the actual range covered by the breakpoint.
        /// </summary>
        public int? EndLine { get; set; }

        /// <summary>
        /// An optional end column of the actual range covered by the breakpoint. If no end line is given, then the
        /// end column is assumed to be in the start line.
        /// </summary>
        public int? EndColumn { get; set; }

        /// <summary>
        /// An optional memory reference to where the breakpoint is set.
        /// </summary>
        public string InstructionReference { get; set; }

        /// <summary>
        /// An optional offset from the instruction reference. This can be negative.
        /// </summary>
        public int? Offset { get; set; }
    }
}

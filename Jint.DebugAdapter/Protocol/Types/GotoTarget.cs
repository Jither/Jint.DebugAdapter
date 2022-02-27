namespace Jint.DebugAdapter.Protocol.Types
{
    /// <summary>
    /// A GotoTarget describes a code location that can be used as a target in the ‘goto’ request. The possible
    /// goto targets can be determined via the ‘gotoTargets’ request.
    /// </summary>
    public class GotoTarget
    {
        /// <param name="id">Unique identifier for a goto target. This is used in the goto request.</param>
        /// <param name="label">The name of the goto target (shown in the UI).</param>
        /// <param name="line">The line of the goto target.</param>
        /// <param name="column">An optional column of the goto target.</param>
        /// <param name="endLine"> An optional end line of the range covered by the goto target.</param>
        /// <param name="endColumn">An optional end column of the range covered by the goto target.</param>
        /// <param name="instructionPointerReference">Optional memory reference for the instruction pointer value
        /// represented by this target.</param>
        public GotoTarget(int id, string label, int line, int? column = null, int? endLine = null, 
            int? endColumn = null, string instructionPointerReference = null)
        {
            Id = id;
            Label = label;
            Line = line;
            Column = column;
            EndLine = endLine;
            EndColumn = endColumn;
            InstructionPointerReference = instructionPointerReference;
        }

        /// <summary>
        /// Unique identifier for a goto target. This is used in the goto request.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the goto target (shown in the UI).
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// The line of the goto target.
        /// </summary>
        public int Line { get; set; }

        /// <summary>
        /// An optional column of the goto target.
        /// </summary>
        public int? Column { get; set; }

        /// <summary>
        /// An optional end line of the range covered by the goto target.
        /// </summary>
        public int? EndLine { get; set; }

        /// <summary>
        /// An optional end column of the range covered by the goto target.
        /// </summary>
        public int? EndColumn { get; set; }

        /// <summary>
        /// Optional memory reference for the instruction pointer value represented by this target.
        /// </summary>
        public string InstructionPointerReference { get; set; }
    }
}

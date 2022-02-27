using System.Text.Json.Serialization;

namespace Jint.DebugAdapter.Protocol.Types
{
    /// <summary>
    /// Properties of a breakpoint passed to the setInstructionBreakpoints request.
    /// </summary>
    public class InstructionBreakpoint
    {
        /// <param name="instructionReference">The instruction reference of the breakpoint.</param>
        [JsonConstructor]
        public InstructionBreakpoint(string instructionReference)
        {
            InstructionReference = instructionReference;
        }

        /// <summary>
        /// The instruction reference of the breakpoint. This should be a memory or instruction pointer reference
        /// from an EvaluateResponse, Variable, StackFrame, GotoTarget, or Breakpoint.
        /// </summary>
        public string InstructionReference { get; set; }

        /// <summary>
        /// An optional offset from the instruction reference. This can be negative.
        /// </summary>
        public long? Offset { get; set; }

        /// <summary>
        /// An optional expression for conditional breakpoints. It is only honored by a debug adapter if the
        /// capability 'supportsConditionalBreakpoints' is true.
        /// </summary>
        public string Condition { get; set; }

        /// <summary>
        /// An optional expression that controls how many hits of the breakpoint are ignored. The backend is expected
        /// to interpret the expression as needed.
        /// </summary>
        /// <remarks>
        /// The attribute is only honored by a debug adapter if the capability 'supportsHitConditionalBreakpoints'
        /// is true.
        /// </remarks>
        public string HitCondition { get; set; }
    }
}

using Jint.DebugAdapter.Helpers;

namespace Jint.DebugAdapter.Protocol.Types
{
    /// <completionlist cref="SteppingGranularity"/>
    public class SteppingGranularity : StringEnum<SteppingGranularity>
    {
        /// <summary>
        /// The step should allow the program to run until the current statement has finished executing. 
        /// The meaning of a statement is determined by the adapter and it may be considered equivalent to a line.
        /// For example ‘for (int i = 0; i < 10; i++) could be considered to have 3 statements
        /// ‘int i = 0’, ‘i < 10’, and ‘i++’.
        /// </summary>
        public static readonly SteppingGranularity Statement = Create("statement");

        /// <summary>
        /// The step should allow the program to run until the current source line has executed.
        /// </summary>
        public static readonly SteppingGranularity Line = Create("line");

        /// <summary>
        /// The step should allow one instruction to execute (e.g. one x86 instruction).
        /// </summary>
        public static readonly SteppingGranularity Instruction = Create("instruction");
    }
}

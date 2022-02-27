namespace Jint.DebugAdapter.Protocol.Types
{
    public enum SteppingGranularity
    {
        Other,

        /// <summary>
        /// The step should allow the program to run until the current statement has finished executing. 
        /// The meaning of a statement is determined by the adapter and it may be considered equivalent to a line.
        /// For example ‘for (int i = 0; i < 10; i++) could be considered to have 3 statements
        /// ‘int i = 0’, ‘i < 10’, and ‘i++’.
        /// </summary>
        Statement,

        /// <summary>
        /// The step should allow the program to run until the current source line has executed.
        /// </summary>
        Line,

        /// <summary>
        /// The step should allow one instruction to execute (e.g. one x86 instruction).
        /// </summary>
        Instruction
    }
}

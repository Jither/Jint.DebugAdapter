using Jither.DebugAdapter.Helpers;

namespace Jither.DebugAdapter.Protocol.Types
{
    /// <completionlist cref="VariableAttribute"/>
    public class VariableAttribute : StringEnum<VariableAttribute>
    {
        /// <summary>
        /// Indicates that the object is static.
        /// </summary>
        public static readonly VariableAttribute Static = Create("static");

        /// <summary>
        /// Indicates that the object is a constant.
        /// </summary>
        public static readonly VariableAttribute Constant = Create("constant");

        /// <summary>
        /// Indicates that the object is read only.
        /// </summary>
        public static readonly VariableAttribute ReadOnly = Create("readOnly");

        /// <summary>
        /// Indicates that the object is a raw string.
        /// </summary>
        public static readonly VariableAttribute RawString = Create("rawString");

        /// <summary>
        /// Indicates that the object has an Object ID associated with it.
        /// </summary>
        public static readonly VariableAttribute HasObjectId = Create("hasObjectId");

        /// <summary>
        /// Indicates that the object can have an Object ID created for it.
        /// </summary>
        public static readonly VariableAttribute CanHaveObjectId = Create("canHaveObjectId");

        /// <summary>
        /// Indicates that the evaluation had side effects.
        /// </summary>
        public static readonly VariableAttribute HasSideEffects = Create("hasSideEffects");

        /// <summary>
        /// Indicates that the object has its value tracked by a data breakpoint.
        /// </summary>
        public static readonly VariableAttribute HasDataBreakpoint = Create("hasDataBreakpoint");
    }
}

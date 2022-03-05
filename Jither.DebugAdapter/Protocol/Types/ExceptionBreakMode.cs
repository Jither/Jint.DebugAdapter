using Jither.DebugAdapter.Helpers;

namespace Jither.DebugAdapter.Protocol.Types
{
    /// <completionlist cref="ExceptionBreakMode"/>
    public class ExceptionBreakMode : StringEnum<ExceptionBreakMode>
    {
        /// <summary>
        /// Never breaks.
        /// </summary>
        public static readonly ExceptionBreakMode Never = Create("never");

        /// <summary>
        /// Always breaks.
        /// </summary>
        public static readonly ExceptionBreakMode Always = Create("always");

        /// <summary>
        /// Breaks when exception unhandled.
        /// </summary>
        public static readonly ExceptionBreakMode Unhandled = Create("unhandled");

        /// <summary>
        /// Breaks if the exception is not handled by user code.
        /// </summary>
        public static readonly ExceptionBreakMode UserUnhandled = Create("userUnhandled");
    }
}

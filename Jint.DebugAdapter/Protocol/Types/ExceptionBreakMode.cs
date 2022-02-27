namespace Jint.DebugAdapter.Protocol.Types
{
    public enum ExceptionBreakMode
    {
        Other,

        /// <summary>
        /// Never breaks.
        /// </summary>
        Never,

        /// <summary>
        /// Always breaks.
        /// </summary>
        Always,

        /// <summary>
        /// Breaks when exception unhandled.
        /// </summary>
        Unhandled,

        /// <summary>
        /// Breaks if the exception is not handled by user code.
        /// </summary>
        UserUnhandled
    }
}

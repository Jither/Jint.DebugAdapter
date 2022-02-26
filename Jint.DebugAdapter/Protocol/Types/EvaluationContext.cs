namespace Jint.DebugAdapter.Protocol.Types
{
    public enum EvaluationContext
    {
        Other,
        /// <summary>
        /// Evaluate is run in a watch.
        /// </summary>
        Watch,

        /// <summary>
        /// Evaluate is run from REPL console.
        /// </summary>
        Repl,

        /// <summary>
        /// Evaluate is run from a data hover.
        /// </summary>
        Hover,

        /// <summary>
        /// Evaluate is run to generate the value that will be stored in the clipboard.
        /// </summary>
        /// <remarks>
        /// The attribute is only honored by a debug adapter if the capability 'supportsClipboardContext' is true.
        /// </remarks>
        Clipboard
    }
}

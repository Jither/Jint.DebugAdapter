using Jint.DebugAdapter.Helpers;

namespace Jint.DebugAdapter.Protocol.Types
{
    /// <completionlist cref="EvaluationContext"/>
    public class EvaluationContext : StringEnum<EvaluationContext>
    {
        /// <summary>
        /// Evaluate is run in a watch.
        /// </summary>
        public static readonly EvaluationContext Watch = Create("watch");

        /// <summary>
        /// Evaluate is run from REPL console.
        /// </summary>
        public static readonly EvaluationContext Repl = Create("repl");

        /// <summary>
        /// Evaluate is run from a data hover.
        /// </summary>
        public static readonly EvaluationContext Hover = Create("hover");

        /// <summary>
        /// Evaluate is run to generate the value that will be stored in the clipboard.
        /// </summary>
        /// <remarks>
        /// The attribute is only honored by a debug adapter if the capability 'supportsClipboardContext' is true.
        /// </remarks>
        public static readonly EvaluationContext Clipboard = Create("clipboard");
    }
}

using Jither.DebugAdapter.Helpers;
using Jither.DebugAdapter.Protocol.Types;

namespace Jither.DebugAdapter.Protocol.Requests
{
    /// <summary>
    /// Evaluates the given expression in the context of the top most stack frame.
    /// The expression has access to any variables and arguments that are in scope.
    /// </summary>
    public class EvaluateArguments : ProtocolArguments
    {
        /// <summary>
        /// The expression to evaluate.
        /// </summary>
        public string Expression { get; set; }

        /// <summary>
        /// Evaluate the expression in the scope of this stack frame. If not specified, the expression is evaluated
        /// in the global scope.
        /// </summary>
        public int? FrameId { get; set; }

        /// <summary>
        /// The context in which the evaluate request is run.
        /// </summary>
        public EvaluationContext Context { get; set; }

        /// <summary>
        /// Specifies details on how to format the Evaluate result.
        /// </summary>
        /// <remarks>
        /// The attribute is only honored by a debug adapter if the capability
        /// 'supportsValueFormattingOptions' is true.
        /// </remarks>
        public ValueFormat Format { get; set; }
    }
}

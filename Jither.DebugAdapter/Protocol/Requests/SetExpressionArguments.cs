using Jither.DebugAdapter.Protocol.Types;

namespace Jither.DebugAdapter.Protocol.Requests
{
    /// <summary>
    /// Evaluates the given ‘value’ expression and assigns it to the ‘expression’ which must be a modifiable l-value.
    /// </summary>
    /// <remarks>
    /// The expressions have access to any variables and arguments that are in scope of the specified frame.
    /// Clients should only call this request if the capability ‘supportsSetExpression’ is true.
    /// If a debug adapter implements both setExpression and setVariable, a client will only use setExpression if
    /// the variable has an evaluateName property.
    /// </summary>
    public class SetExpressionArguments : ProtocolArguments
    {
        /// <summary>
        /// The l-value expression to assign to.
        /// </summary>
        public string Expression { get; set; }

        /// <summary>
        /// The value expression to assign to the l-value expression.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Evaluate the expressions in the scope of this stack frame. If not specified, the expressions are
        /// evaluated in the global scope.
        /// </summary>
        public int? FrameId { get; set; }

        /// <summary>
        /// Specifies how the resulting value should be formatted.
        /// </summary>
        public ValueFormat Format { get; set; }
    }
}

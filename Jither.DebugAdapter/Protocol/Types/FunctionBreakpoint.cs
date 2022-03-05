using System.Text.Json.Serialization;

namespace Jither.DebugAdapter.Protocol.Types
{
    /// <summary>
    /// Properties of a breakpoint passed to the setFunctionBreakpoints request.
    /// </summary>
    public class FunctionBreakpoint
    {
        /// <param name="name">The name of the function.</param>
        [JsonConstructor]
        public FunctionBreakpoint(string name)
        {
            Name = name;
        }

        /// <summary>
        /// The name of the function.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// An optional expression for conditional breakpoints. It is only honored by a debug adapter if the
        /// capability 'supportsConditionalBreakpoints' is true.
        /// </summary>
        public string Condition { get; set; }

        /// <summary>
        /// An optional expression that controls how many hits of the breakpoint are ignored.
        /// </summary>
        /// <remarks>
        /// The backend is expected to interpret the expression as needed. The attribute is only honored by a debug
        /// adapter if the capability 'supportsHitConditionalBreakpoints' is true.
        /// </remarks>
        public string HitCondition { get; set; }
    }
}

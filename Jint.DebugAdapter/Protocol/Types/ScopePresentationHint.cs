using Jint.DebugAdapter.Helpers;

namespace Jint.DebugAdapter.Protocol.Types
{
    /// <completionlist cref="ScopePresentationHint"/>
    public class ScopePresentationHint : StringEnum<ScopePresentationHint>
    {
        /// <summary>
        /// Scope contains method arguments.
        /// </summary>
        public static readonly ScopePresentationHint Arguments = Create("arguments");

        /// <summary>
        /// Scope contains local variables.
        /// </summary>
        public static readonly ScopePresentationHint Locals = Create("locals");

        /// <summary>
        /// Scope contains registers. Only a single 'registers' scope should be returned from a 'scopes' request.
        /// </summary>
        public static readonly ScopePresentationHint Registers = Create("registers");
    }
}

using Jither.DebugAdapter.Helpers;

namespace Jither.DebugAdapter.Protocol.Types
{
    /// <completionlist cref="StartMethod"/>
    public class StartMethod : StringEnum<StartMethod>
    {
        /// <summary>
        /// Process was launched under the debugger.
        /// </summary>
        public static readonly StartMethod Launch = Create("launch");

        /// <summary>
        /// Debugger attached to an existing process.
        /// </summary>
        public static readonly StartMethod Attach = Create("attach");

        /// <summary>
        /// A project launcher component has launched a new process in a suspended state and then asked the debugger to attach.
        /// </summary>
        public static readonly StartMethod AttachForSuspendedLaunch = Create("attachForSuspendedLaunch");
    }
}

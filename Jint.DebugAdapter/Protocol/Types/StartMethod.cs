namespace Jint.DebugAdapter.Protocol.Types
{
    public enum StartMethod
    {
        Other,
        /// <summary>
        /// Process was launched under the debugger.
        /// </summary>
        Launch,

        /// <summary>
        /// Debugger attached to an existing process.
        /// </summary>
        Attach,

        /// <summary>
        /// A project launcher component has launched a new process in a suspended state and then asked the debugger to attach.
        /// </summary>
        AttachForSuspendedLaunch
    }
}

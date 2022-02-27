using Jint.DebugAdapter.Helpers;
using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Requests
{
    /// <summary>
    /// The request resumes the given thread to step out (return) from a function/method and allows all other threads
    /// to run freely by resuming them.
    /// </summary>
    /// <remarks>
    /// If the debug adapter supports single thread execution(see capability ‘supportsSingleThreadExecutionRequests’)
    /// setting the ‘singleThread’ argument to true prevents other suspended threads from resuming.
    /// 
    /// The debug adapter first sends the response and then a ‘stopped’ event (with reason ‘step’) after
    /// the step has completed.
    /// </remarks>
    public class StepOutArguments : ProtocolArguments
    {
        /// <summary>
        /// Specifies the thread for which to resume execution for one step-out (of the given granularity).
        /// </summary>
        public int ThreadId { get; set; }

        /// <summary>
        /// If this optional flag is true, all other suspended threads are not resumed.
        /// </summary>
        public bool? SingleThread { get; set; }

        /// <summary>
        /// Optional granularity to step. If no granularity is specified, a granularity of 'statement' is assumed.
        /// </summary>
        public SteppingGranularity Granularity { get; set; }
    }
}

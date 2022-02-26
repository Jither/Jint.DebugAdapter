using Jint.DebugAdapter.Helpers;
using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Requests
{
    /// <summary>
    /// The request executes one backward step (in the given granularity) for the specified thread and
    /// allows all other threads to run backward freely by resuming them.
    /// </summary>
    /// <remarks>
    /// If the debug adapter supports single thread execution(see capability ‘supportsSingleThreadExecutionRequests’)
    /// setting the ‘singleThread’ argument to true prevents other suspended threads from resuming.
    /// 
    /// The debug adapter first sends the response and then a ‘stopped’ event (with reason ‘step’) after the step has completed.
    /// 
    /// Clients should only call this request if the capability ‘supportsStepBack’ is true.
    /// </remarks>
    public class StepBackArguments : ProtocolArguments
    {
        /// <summary>
        /// Specifies the thread for which to resume execution for one step backwards (of the given granularity).
        /// </summary>
        public int ThreadId { get; set; }

        /// <summary>
        /// If this optional flag is true, all other suspended threads are not resumed.
        /// </summary>
        public bool? SingleThread { get; set; }

        /// <summary>
        /// Optional granularity to step. If no granularity is specified, a granularity of 'statement' is assumed.
        /// </summary>
        public StringEnum<SteppingGranularity>? Granularity { get; set; }
    }
}

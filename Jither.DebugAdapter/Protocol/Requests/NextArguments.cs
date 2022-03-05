using Jither.DebugAdapter.Helpers;
using Jither.DebugAdapter.Protocol.Types;

namespace Jither.DebugAdapter.Protocol.Requests
{
    /// <summary>
    /// The request executes one step (in the given granularity) for the specified thread and allows all
    /// other threads to run freely by resuming them.
    /// </summary>
    /// <remarks>
    /// If the debug adapter supports single thread execution(see capability ‘supportsSingleThreadExecutionRequests’)
    /// setting the ‘singleThread’ argument to true prevents other suspended threads from resuming.
    /// The debug adapter first sends the response and then a ‘stopped’ event (with reason ‘step’)
    /// after the step has completed.
    /// </remarks>
    public class NextArguments : ProtocolArguments
    {
        /// <summary>
        /// Specifies the thread for which to resume execution for one step (of the given granularity).
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

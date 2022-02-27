using Jint.DebugAdapter.Helpers;
using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Requests
{
    /// <summary>
    /// The request resumes the given thread to step into a function/method and allows all other threads to run freely by resuming them.
    /// </summary>
    /// <remarks>
    /// If the debug adapter supports single thread execution(see capability ‘supportsSingleThreadExecutionRequests’) setting 
    /// the ‘singleThread’ argument to true prevents other suspended threads from resuming.
    /// 
    /// If the request cannot step into a target, ‘stepIn’ behaves like the ‘next’ request.
    /// 
    /// The debug adapter first sends the response and then a ‘stopped’ event (with reason ‘step’) after the step has completed.
    /// 
    /// If there are multiple function/method calls(or other targets) on the source line, the optional argument ‘targetId’ can
    /// be used to control into which target the ‘stepIn’ should occur.
    /// 
    /// The list of possible targets for a given source line can be retrieved via the ‘stepInTargets’ request.
    /// </remarks>
    public class StepInArguments : ProtocolArguments
    {
        /// <summary>
        /// Specifies the thread for which to resume execution for one step-into (of the given granularity).
        /// </summary>
        public int ThreadId { get; set; }

        /// <summary>
        /// If this optional flag is true, all other suspended threads are not resumed.
        /// </summary>
        public bool? SingleThread { get; set; }

        /// <summary>
        /// Optional id of the target to step into.
        /// </summary>
        public int? TargetId { get; set; }

        /// <summary>
        /// Optional granularity to step. If no granularity is specified, a granularity of 'statement' is assumed.
        /// </summary>
        public SteppingGranularity Granularity { get; set; }
    }
}

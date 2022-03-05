namespace Jither.DebugAdapter.Protocol.Requests
{
    /// <summary>
    /// This request retrieves the possible stepIn targets for the specified stack frame.
    /// </summary>
    /// <remarks>
    /// These targets can be used in the ‘stepIn’ request.
    /// 
    /// Clients should only call this request if the capability ‘supportsStepInTargetsRequest’ is true.
    /// </summary>
    public class StepInTargetsArguments : ProtocolArguments
    {
        /// <summary>
        /// The stack frame for which to retrieve the possible stepIn targets.
        /// </summary>
        public int FrameId { get; set; }
    }
}

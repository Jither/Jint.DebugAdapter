﻿namespace Jither.DebugAdapter.Protocol.Types
{
    /// <summary>
    /// Information about the capabilities of a debug adapter.
    /// </summary>
    /// <inheritdoc/>
    public class Capabilities : ICapabilities
    {
        public bool? SupportsConfigurationDoneRequest { get; set; }
        public bool? SupportsFunctionBreakpoints { get; set; }
        public bool? SupportsConditionalBreakpoints { get; set; }
        public bool? SupportsHitConditionalBreakpoints { get; set; }
        public bool? SupportsEvaluateForHovers { get; set; }
        public IEnumerable<ExceptionBreakpointsFilter> ExceptionBreakpointFilters { get; set; }
        public bool? SupportsStepBack { get; set; }
        public bool? SupportsSetVariable { get; set; }
        public bool? SupportsRestartFrame { get; set; }
        public bool? SupportsGotoTargetsRequest { get; set; }
        public bool? SupportsStepInTargetsRequest { get; set; }
        public bool? SupportsCompletionsRequest { get; set; }
        public IEnumerable<string> CompletionTriggerCharacters { get; set; }
        public bool? SupportsModulesRequest { get; set; }
        public IEnumerable<ColumnDescriptor> AdditionalModuleColumns { get; set; }
        public IEnumerable<ChecksumAlgorithm> SupportedChecksumAlgorithms { get; set; }
        public bool? SupportsRestartRequest { get; set; }
        public bool? SupportsExceptionOptions { get; set; }
        public bool? SupportsValueFormattingOptions { get; set; }
        public bool? SupportsExceptionInfoRequest { get; set; }
        public bool? SupportTerminateDebuggee { get; set; }
        public bool? SupportSuspendDebuggee { get; set; }
        public bool? SupportsDelayedStackTraceLoading { get; set; }
        public bool? SupportsLoadedSourcesRequest { get; set; }
        public bool? SupportsLogPoints { get; set; }
        public bool? SupportsTerminateThreadsRequest { get; set; }
        public bool? SupportsSetExpression { get; set; }
        public bool? SupportsTerminateRequest { get; set; }
        public bool? SupportsDataBreakpoints { get; set; }
        public bool? SupportsReadMemoryRequest { get; set; }
        public bool? SupportsWriteMemoryRequest { get; set; }
        public bool? SupportsDisassembleRequest { get; set; }
        public bool? SupportsCancelRequest { get; set; }
        public bool? SupportsBreakpointLocationsRequest { get; set; }
        public bool? SupportsClipboardContext { get; set; }
        public bool? SupportsSteppingGranularity { get; set; }
        public bool? SupportsInstructionBreakpoints { get; set; }
        public bool? SupportsExceptionFilterOptions { get; set; }
        public bool? SupportsSingleThreadExecutionRequests { get; set; }
    }
}

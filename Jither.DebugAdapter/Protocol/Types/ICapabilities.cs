namespace Jither.DebugAdapter.Protocol.Types
{
    /// <summary>
    /// Information about the capabilities of a debug adapter.
    /// </summary>
    // Because InitializeResponseBody is == the Capabilities type, the interface enforces the same
    // properties on that ProtocolResponseBody and the actual Capabilities type.
    interface ICapabilities
    {
        /// <summary>
        /// The debug adapter supports the 'configurationDone' request.
        /// </summary>
        bool? SupportsConfigurationDoneRequest { get; set; }

        /// <summary>
        /// The debug adapter supports function breakpoints.
        /// </summary>
        bool? SupportsFunctionBreakpoints { get; set; }

        /// <summary>
        /// The debug adapter supports conditional breakpoints.
        /// </summary>
        bool? SupportsConditionalBreakpoints { get; set; }

        /// <summary>
        /// The debug adapter supports breakpoints that break execution after a specified number of hits.
        /// </summary>
        bool? SupportsHitConditionalBreakpoints { get; set; }

        /// <summary>
        /// The debug adapter supports a (side effect free) evaluate request for data hovers.
        /// </summary>
        bool? SupportsEvaluateForHovers { get; set; }

        /// <summary>
        /// Available exception filter options for the 'setExceptionBreakpoints' request.
        /// </summary>
        IEnumerable<ExceptionBreakpointsFilter> ExceptionBreakpointFilters { get; set; }

        /// <summary>
        /// The debug adapter supports stepping back via the 'stepBack' and 'reverseContinue' requests.
        /// </summary>
        bool? SupportsStepBack { get; set; }

        /// <summary>
        /// The debug adapter supports setting a variable to a value.
        /// </summary>
        bool? SupportsSetVariable { get; set; }

        /// <summary>
        /// The debug adapter supports restarting a frame.
        /// </summary>
        bool? SupportsRestartFrame { get; set; }

        /// <summary>
        /// The debug adapter supports the 'gotoTargets' request.
        /// </summary>
        bool? SupportsGotoTargetsRequest { get; set; }

        /// <summary>
        /// The debug adapter supports the 'stepInTargets' request.
        /// </summary>
        bool? SupportsStepInTargetsRequest { get; set; }

        /// <summary>
        /// The debug adapter supports the 'completions' request.
        /// </summary>
        bool? SupportsCompletionsRequest { get; set; }

        /// <summary>
        /// The set of characters that should trigger completion in a REPL. If not specified, the UI should
        /// assume the '.' character.
        /// </summary>
        IEnumerable<string> CompletionTriggerCharacters { get; set; }

        /// <summary>
        /// The debug adapter supports the 'modules' request.
        /// </summary>
        bool? SupportsModulesRequest { get; set; }

        /// <summary>
        /// The set of additional module information exposed by the debug adapter.
        /// </summary>
        IEnumerable<ColumnDescriptor> AdditionalModuleColumns { get; set; }

        /// <summary>
        /// Checksum algorithms supported by the debug adapter.
        /// </summary>
        IEnumerable<ChecksumAlgorithm> SupportedChecksumAlgorithms { get; set; }

        /// <summary>
        /// The debug adapter supports the 'restart' request. In this case a client should not implement 'restart'
        /// by terminating and relaunching the adapter but by calling the RestartRequest.
        /// </summary>
        bool? SupportsRestartRequest { get; set; }

        /// <summary>
        /// The debug adapter supports 'exceptionOptions' on the setExceptionBreakpoints request.
        /// </summary>
        bool? SupportsExceptionOptions { get; set; }

        /// <summary>
        /// The debug adapter supports a 'format' attribute on the stackTraceRequest, variablesRequest,
        /// and evaluateRequest.
        /// </summary>
        bool? SupportsValueFormattingOptions { get; set; }

        /// <summary>
        /// The debug adapter supports the 'exceptionInfo' request.
        /// </summary>
        bool? SupportsExceptionInfoRequest { get; set; }

        /// <summary>
        /// The debug adapter supports the 'terminateDebuggee' attribute on the 'disconnect' request.
        /// </summary>
        bool? SupportTerminateDebuggee { get; set; }

        /// <summary>
        /// The debug adapter supports the 'suspendDebuggee' attribute on the 'disconnect' request.
        /// </summary>
        bool? SupportSuspendDebuggee { get; set; }

        /// <summary>
        /// The debug adapter supports the delayed loading of parts of the stack, which requires that both the 
        /// 'startFrame' and 'levels' arguments and an optional 'totalFrames' result of the 'StackTrace' request are
        /// supported.
        /// </summary>
        bool? SupportsDelayedStackTraceLoading { get; set; }

        /// <summary>
        /// The debug adapter supports the 'loadedSources' request.
        /// </summary>
        bool? SupportsLoadedSourcesRequest { get; set; }

        /// <summary>
        /// The debug adapter supports logpoints by interpreting the 'logMessage' attribute of the SourceBreakpoint.
        /// </summary>
        bool? SupportsLogPoints { get; set; }

        /// <summary>
        /// The debug adapter supports the 'terminateThreads' request.
        /// </summary>
        bool? SupportsTerminateThreadsRequest { get; set; }

        /// <summary>
        /// 
        /// </summary>
        bool? SupportsSetExpression { get; set; }

        /// <summary>
        /// The debug adapter supports the 'terminate' request.
        /// </summary>
        bool? SupportsTerminateRequest { get; set; }

        /// <summary>
        /// The debug adapter supports data breakpoints.
        /// </summary>
        bool? SupportsDataBreakpoints { get; set; }

        /// <summary>
        /// The debug adapter supports the 'readMemory' request.
        /// </summary>
        bool? SupportsReadMemoryRequest { get; set; }

        /// <summary>
        /// The debug adapter supports the 'writeMemory' request.
        /// </summary>
        bool? SupportsWriteMemoryRequest { get; set; }

        /// <summary>
        /// The debug adapter supports the 'disassemble' request.
        /// </summary>
        bool? SupportsDisassembleRequest { get; set; }

        /// <summary>
        /// The debug adapter supports the 'cancel' request.
        /// </summary>
        bool? SupportsCancelRequest { get; set; }

        /// <summary>
        /// The debug adapter supports the 'breakpointLocations' request.
        /// </summary>
        bool? SupportsBreakpointLocationsRequest { get; set; }

        /// <summary>
        /// The debug adapter supports the 'clipboard' context value in the 'evaluate' request.
        /// </summary>
        bool? SupportsClipboardContext { get; set; }

        /// <summary>
        /// The debug adapter supports stepping granularities (argument 'granularity') for the stepping requests.
        /// </summary>
        bool? SupportsSteppingGranularity { get; set; }

        /// <summary>
        /// The debug adapter supports adding breakpoints based on instruction references.
        /// </summary>
        bool? SupportsInstructionBreakpoints { get; set; }

        /// <summary>
        /// The debug adapter supports 'filterOptions' as an argument on the 'setExceptionBreakpoints' request.
        /// </summary>
        bool? SupportsExceptionFilterOptions { get; set; }

        /// <summary>
        /// The debug adapter supports the 'singleThread' property on the execution requests
        /// ('continue', 'next', 'stepIn', 'stepOut', 'reverseContinue', 'stepBack').
        /// </summary>
        bool? SupportsSingleThreadExecutionRequests { get; set; }
    }
}

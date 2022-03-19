using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jither.DebugAdapter.Protocol;
using Jither.DebugAdapter.Protocol.Events;
using Jither.DebugAdapter.Protocol.Requests;
using Jither.DebugAdapter.Protocol.Responses;
using Jither.DebugAdapter.Protocol.Types;

namespace Jither.DebugAdapter
{
    public abstract class Adapter
    {
        public DebugProtocol Protocol { get; set; }

        internal Task<ProtocolResponseBody> InternalHandleRequest(BaseProtocolRequest request)
        {
            return Task.FromResult(HandleRequest(request));
        }

        private ProtocolResponseBody HandleRequest(BaseProtocolRequest request)
        {
            switch (request.UntypedArguments)
            {
                case AttachArguments args: AttachRequest(args); return null;
                case CancelArguments args: CancelRequest(args); return null;
                case ConfigurationDoneArguments: ConfigurationDoneRequest(); return null;
                case DisconnectArguments args: DisconnectRequest(args); return null;
                case GotoArguments args: GotoRequest(args); return null;
                case LaunchArguments args: LaunchRequest(args); return null;
                case NextArguments args: NextRequest(args); return null;
                case PauseArguments args: PauseRequest(args); return null;
                case RestartArguments args: RestartRequest(args); return null;
                case RestartFrameArguments args: RestartFrameRequest(args); return null;
                case ReverseContinueArguments args: ReverseContinueRequest(args); return null;
                case StepBackArguments args: StepBackRequest(args); return null;
                case StepInArguments args: StepInRequest(args); return null;
                case StepOutArguments args: StepOutRequest(args); return null;
                case TerminateArguments args: TerminateRequest(args); return null;
            }

            return request.UntypedArguments switch
            {
                BreakpointLocationsArguments args => BreakpointLocationsRequest(args),
                CompletionsArguments args => CompletionsRequest(args),
                ContinueArguments args => ContinueRequest(args),
                DataBreakpointInfoArguments args => DataBreakpointInfoRequest(args),
                DisassembleArguments args => DisassembleRequest(args),
                EvaluateArguments args => EvaluateRequest(args),
                ExceptionInfoArguments args => ExceptionInfoRequest(args),
                GotoTargetsArguments args => GotoTargetsRequest(args),
                InitializeArguments args => InitializeRequest(args),
                LoadedSourcesArguments => LoadedSourcesRequest(),
                ModulesArguments args => ModulesRequest(args),
                ReadMemoryArguments args => ReadMemoryRequest(args),
                //RunInTerminalArguments args => RunInTerminalRequest(args), // Note: Reverse Request (DebugAdapter to host)
                ScopesArguments args => ScopesRequest(args),
                SetBreakpointsArguments args => SetBreakpointsRequest(args),
                SetDataBreakpointsArguments args => SetDataBreakpointsRequest(args),
                SetExceptionBreakpointsArguments args => SetExceptionBreakpointsRequest(args),
                SetExpressionArguments args => SetExpressionRequest(args),
                SetFunctionBreakpointsArguments args => SetFunctionBreakpointsRequest(args),
                SetInstructionBreakpointsArguments args => SetInstructionBreakpointsRequest(args),
                SetVariableArguments args => SetVariableRequest(args),
                SourceArguments args => SourceRequest(args),
                StackTraceArguments args => StackTraceRequest(args),
                StepInTargetsArguments args => StepInTargetsRequest(args),
                ThreadsArguments => ThreadsRequest(),
                VariablesArguments args => VariablesRequest(args),
                WriteMemoryArguments args => WriteMemoryRequest(args),

                _ => throw new NotImplementedException($"Request command '{request.Command}' not implemented.")
            };
        }

        protected void Stop()
        {
            Protocol.Stop();
        }

        public void SendEvent(ProtocolEventBody body)
        {
            var evt = new ProtocolEvent(body.EventName, body);
            Protocol.SendEvent(evt);
        }

        protected virtual void AttachRequest(AttachArguments arguments) => throw new NotImplementedException();
        protected virtual BreakpointLocationsResponse BreakpointLocationsRequest(BreakpointLocationsArguments arguments) => throw new NotImplementedException();
        protected virtual void CancelRequest(CancelArguments arguments) => throw new NotImplementedException();
        protected virtual CompletionsResponse CompletionsRequest(CompletionsArguments arguments) => throw new NotImplementedException();
        protected virtual void ConfigurationDoneRequest() => throw new NotImplementedException();
        protected virtual ContinueResponse ContinueRequest(ContinueArguments arguments) => throw new NotImplementedException();
        protected virtual DataBreakpointInfoResponse DataBreakpointInfoRequest(DataBreakpointInfoArguments arguments) => throw new NotImplementedException();
        protected virtual DisassembleResponse DisassembleRequest(DisassembleArguments arguments) => throw new NotImplementedException();
        protected virtual void DisconnectRequest(DisconnectArguments arguments) => throw new NotImplementedException();
        protected virtual EvaluateResponse EvaluateRequest(EvaluateArguments arguments) => throw new NotImplementedException();
        protected virtual ExceptionInfoResponse ExceptionInfoRequest(ExceptionInfoArguments arguments) => throw new NotImplementedException();
        protected virtual void GotoRequest(GotoArguments arguments) => throw new NotImplementedException();
        protected virtual GotoTargetsResponse GotoTargetsRequest(GotoTargetsArguments arguments) => throw new NotImplementedException();
        protected virtual InitializeResponse InitializeRequest(InitializeArguments arguments) => throw new NotImplementedException();
        protected virtual void LaunchRequest(LaunchArguments arguments) => throw new NotImplementedException();
        protected virtual LoadedSourcesResponse LoadedSourcesRequest() => throw new NotImplementedException();
        protected virtual ModulesResponse ModulesRequest(ModulesArguments arguments) => throw new NotImplementedException();
        protected virtual void NextRequest(NextArguments arguments) => throw new NotImplementedException();
        protected virtual void PauseRequest(PauseArguments arguments) => throw new NotImplementedException();
        protected virtual ReadMemoryResponse ReadMemoryRequest(ReadMemoryArguments arguments) => throw new NotImplementedException();
        protected virtual void RestartRequest(RestartArguments arguments) => throw new NotImplementedException();
        protected virtual void RestartFrameRequest(RestartFrameArguments arguments) => throw new NotImplementedException();
        protected virtual void ReverseContinueRequest(ReverseContinueArguments arguments) => throw new NotImplementedException();
        protected virtual ScopesResponse ScopesRequest(ScopesArguments arguments) => throw new NotImplementedException();
        protected virtual SetBreakpointsResponse SetBreakpointsRequest(SetBreakpointsArguments arguments) => throw new NotImplementedException();
        protected virtual SetDataBreakpointsResponse SetDataBreakpointsRequest(SetDataBreakpointsArguments arguments) => throw new NotImplementedException();
        protected virtual SetExceptionBreakpointsResponse SetExceptionBreakpointsRequest(SetExceptionBreakpointsArguments arguments) => throw new NotImplementedException();
        protected virtual SetExpressionResponse SetExpressionRequest(SetExpressionArguments arguments) => throw new NotImplementedException();
        protected virtual SetFunctionBreakpointsResponse SetFunctionBreakpointsRequest(SetFunctionBreakpointsArguments arguments) => throw new NotImplementedException();
        protected virtual SetInstructionBreakpointsResponse SetInstructionBreakpointsRequest(SetInstructionBreakpointsArguments arguments) => throw new NotImplementedException();
        protected virtual SetVariableResponse SetVariableRequest(SetVariableArguments arguments) => throw new NotImplementedException();
        protected virtual SourceResponse SourceRequest(SourceArguments arguments) => throw new NotImplementedException();
        protected virtual StackTraceResponse StackTraceRequest(StackTraceArguments arguments) => throw new NotImplementedException();
        protected virtual void StepBackRequest(StepBackArguments arguments) => throw new NotImplementedException();
        protected virtual void StepInRequest(StepInArguments arguments) => throw new NotImplementedException();
        protected virtual StepInTargetsResponse StepInTargetsRequest(StepInTargetsArguments arguments) => throw new NotImplementedException();
        protected virtual void StepOutRequest(StepOutArguments arguments) => throw new NotImplementedException();
        protected virtual void TerminateRequest(TerminateArguments arguments) => throw new NotImplementedException();
        protected virtual void TerminateThreadsRequest(TerminateThreadsArguments arguments) => throw new NotImplementedException();
        protected virtual ThreadsResponse ThreadsRequest() => throw new NotImplementedException();
        protected virtual VariablesResponse VariablesRequest(VariablesArguments arguments) => throw new NotImplementedException();
        protected virtual WriteMemoryResponse WriteMemoryRequest(WriteMemoryArguments arguments) => throw new NotImplementedException();
    }
}

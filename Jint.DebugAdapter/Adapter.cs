using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint.DebugAdapter.Protocol;
using Jint.DebugAdapter.Protocol.Events;
using Jint.DebugAdapter.Protocol.Requests;
using Jint.DebugAdapter.Protocol.Responses;
using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter
{
    public abstract class Adapter
    {
        public DebugProtocol Protocol { get; set; }

        public ProtocolResponseBody HandleRequest(BaseProtocolRequest request)
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

        protected void SendEvent(ProtocolEventBody body)
        {
            var evt = new ProtocolEvent(body.EventName, body);
            Protocol.SendEvent(evt);
        }

        protected abstract void AttachRequest(AttachArguments arguments);
        protected abstract BreakpointLocationsResponse BreakpointLocationsRequest(BreakpointLocationsArguments arguments);
        protected abstract void CancelRequest(CancelArguments arguments);
        protected abstract CompletionsResponse CompletionsRequest(CompletionsArguments arguments);
        protected abstract void ConfigurationDoneRequest();
        protected abstract ContinueResponse ContinueRequest(ContinueArguments arguments);
        protected abstract DataBreakpointInfoResponse DataBreakpointInfoRequest(DataBreakpointInfoArguments arguments);
        protected abstract DisassembleResponse DisassembleRequest(DisassembleArguments arguments);
        protected abstract void DisconnectRequest(DisconnectArguments arguments);
        protected abstract EvaluateResponse EvaluateRequest(EvaluateArguments arguments);
        protected abstract ExceptionInfoResponse ExceptionInfoRequest(ExceptionInfoArguments arguments);
        protected abstract void GotoRequest(GotoArguments arguments);
        protected abstract GotoTargetsResponse GotoTargetsRequest(GotoTargetsArguments arguments);
        protected abstract InitializeResponse InitializeRequest(InitializeArguments arguments);
        protected abstract void LaunchRequest(LaunchArguments arguments);
        protected abstract LoadedSourcesResponse LoadedSourcesRequest();
        protected abstract ModulesResponse ModulesRequest(ModulesArguments arguments);
        protected abstract void NextRequest(NextArguments arguments);
        protected abstract void PauseRequest(PauseArguments arguments);
        protected abstract ReadMemoryResponse ReadMemoryRequest(ReadMemoryArguments arguments);
        protected abstract void RestartRequest(RestartArguments arguments);
        protected abstract void RestartFrameRequest(RestartFrameArguments arguments);
        protected abstract void ReverseContinueRequest(ReverseContinueArguments arguments);
        protected abstract ScopesResponse ScopesRequest(ScopesArguments arguments);
        protected abstract SetBreakpointsResponse SetBreakpointsRequest(SetBreakpointsArguments arguments);
        protected abstract SetDataBreakpointsResponse SetDataBreakpointsRequest(SetDataBreakpointsArguments arguments);
        protected abstract SetExceptionBreakpointsResponse SetExceptionBreakpointsRequest(SetExceptionBreakpointsArguments arguments);
        protected abstract SetExpressionResponse SetExpressionRequest(SetExpressionArguments arguments);
        protected abstract SetFunctionBreakpointsResponse SetFunctionBreakpointsRequest(SetFunctionBreakpointsArguments arguments);
        protected abstract SetInstructionBreakpointsResponse SetInstructionBreakpointsRequest(SetInstructionBreakpointsArguments arguments);
        protected abstract SetVariableResponse SetVariableRequest(SetVariableArguments arguments);
        protected abstract SourceResponse SourceRequest(SourceArguments arguments);
        protected abstract StackTraceResponse StackTraceRequest(StackTraceArguments arguments);
        protected abstract void StepBackRequest(StepBackArguments arguments);
        protected abstract void StepInRequest(StepInArguments arguments);
        protected abstract StepInTargetsResponse StepInTargetsRequest(StepInTargetsArguments arguments);
        protected abstract void StepOutRequest(StepOutArguments arguments);
        protected abstract void TerminateRequest(TerminateArguments arguments);
        protected abstract void TerminateThreadsRequest(TerminateThreadsArguments arguments);
        protected abstract ThreadsResponse ThreadsRequest();
        protected abstract VariablesResponse VariablesRequest(VariablesArguments arguments);
        protected abstract WriteMemoryResponse WriteMemoryRequest(WriteMemoryArguments arguments);
    }
}

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
        private readonly Endpoint endpoint;

        public DebugProtocol Protocol { get; set; }

        protected Adapter(Endpoint endpoint)
        {
            this.endpoint = endpoint;
        }

        public void StartListening()
        {
            endpoint.Initialize(this);
            Task.Run(() => endpoint.StartAsync());
            OnListening();
        }

        protected abstract void OnListening();

        public void SendEvent(ProtocolEventBody body)
        {
            var evt = new ProtocolEvent(body.EventName, body);
            Protocol.SendEvent(evt);
        }

        internal async Task<ProtocolResponseBody> HandleRequest(BaseProtocolRequest request)
        {
            switch (request.UntypedArguments)
            {
                case AttachArguments args: await AttachRequest(args); return null;
                case CancelArguments args: await CancelRequest(args); return null;
                case ConfigurationDoneArguments: await ConfigurationDoneRequest(); return null;
                case DisconnectArguments args: await DisconnectRequest(args); return null;
                case GotoArguments args: await GotoRequest(args); return null;
                case LaunchArguments args: await LaunchRequest(args); return null;
                case NextArguments args: await NextRequest(args); return null;
                case PauseArguments args: await PauseRequest(args); return null;
                case RestartArguments args: await RestartRequest(args); return null;
                case RestartFrameArguments args: await RestartFrameRequest(args); return null;
                case ReverseContinueArguments args: await ReverseContinueRequest(args); return null;
                case StepBackArguments args: await StepBackRequest(args); return null;
                case StepInArguments args: await StepInRequest(args); return null;
                case StepOutArguments args: await StepOutRequest(args); return null;
                case TerminateArguments args: await TerminateRequest(args); return null;
            }

            return request.UntypedArguments switch
            {
                BreakpointLocationsArguments args => await BreakpointLocationsRequest(args),
                CompletionsArguments args => await CompletionsRequest(args),
                ContinueArguments args => await ContinueRequest(args),
                DataBreakpointInfoArguments args => await DataBreakpointInfoRequest(args),
                DisassembleArguments args => await DisassembleRequest(args),
                EvaluateArguments args => await EvaluateRequest(args),
                ExceptionInfoArguments args => await ExceptionInfoRequest(args),
                GotoTargetsArguments args => await GotoTargetsRequest(args),
                InitializeArguments args => await InitializeRequest(args),
                LoadedSourcesArguments => await LoadedSourcesRequest(),
                ModulesArguments args => await ModulesRequest(args),
                ReadMemoryArguments args => await ReadMemoryRequest(args),
                //RunInTerminalArguments args => RunInTerminalRequest(args), // Note: Reverse Request (DebugAdapter to host)
                ScopesArguments args => await ScopesRequest(args),
                SetBreakpointsArguments args => await SetBreakpointsRequest(args),
                SetDataBreakpointsArguments args => await SetDataBreakpointsRequest(args),
                SetExceptionBreakpointsArguments args => await SetExceptionBreakpointsRequest(args),
                SetExpressionArguments args => await SetExpressionRequest(args),
                SetFunctionBreakpointsArguments args => await SetFunctionBreakpointsRequest(args),
                SetInstructionBreakpointsArguments args => await SetInstructionBreakpointsRequest(args),
                SetVariableArguments args => await SetVariableRequest(args),
                SourceArguments args => await SourceRequest(args),
                StackTraceArguments args => await StackTraceRequest(args),
                StepInTargetsArguments args => await StepInTargetsRequest(args),
                ThreadsArguments => await ThreadsRequest(),
                VariablesArguments args => await VariablesRequest(args),
                WriteMemoryArguments args => await WriteMemoryRequest(args),

                _ => throw new NotImplementedException($"Request command '{request.Command}' not implemented.")
            };
        }

        protected void Stop()
        {
            Protocol.Stop();
        }

        protected virtual Task AttachRequest(AttachArguments arguments) => throw new NotImplementedException();
        protected virtual Task<BreakpointLocationsResponse> BreakpointLocationsRequest(BreakpointLocationsArguments arguments) => throw new NotImplementedException();
        protected virtual Task CancelRequest(CancelArguments arguments) => throw new NotImplementedException();
        protected virtual Task<CompletionsResponse> CompletionsRequest(CompletionsArguments arguments) => throw new NotImplementedException();
        protected virtual Task ConfigurationDoneRequest() => throw new NotImplementedException();
        protected virtual Task<ContinueResponse> ContinueRequest(ContinueArguments arguments) => throw new NotImplementedException();
        protected virtual Task<DataBreakpointInfoResponse> DataBreakpointInfoRequest(DataBreakpointInfoArguments arguments) => throw new NotImplementedException();
        protected virtual Task<DisassembleResponse> DisassembleRequest(DisassembleArguments arguments) => throw new NotImplementedException();
        protected virtual Task DisconnectRequest(DisconnectArguments arguments) => throw new NotImplementedException();
        protected virtual Task<EvaluateResponse> EvaluateRequest(EvaluateArguments arguments) => throw new NotImplementedException();
        protected virtual Task<ExceptionInfoResponse> ExceptionInfoRequest(ExceptionInfoArguments arguments) => throw new NotImplementedException();
        protected virtual Task GotoRequest(GotoArguments arguments) => throw new NotImplementedException();
        protected virtual Task<GotoTargetsResponse> GotoTargetsRequest(GotoTargetsArguments arguments) => throw new NotImplementedException();
        protected virtual Task<InitializeResponse> InitializeRequest(InitializeArguments arguments) => throw new NotImplementedException();
        protected virtual Task LaunchRequest(LaunchArguments arguments) => throw new NotImplementedException();
        protected virtual Task<LoadedSourcesResponse> LoadedSourcesRequest() => throw new NotImplementedException();
        protected virtual Task<ModulesResponse> ModulesRequest(ModulesArguments arguments) => throw new NotImplementedException();
        protected virtual Task NextRequest(NextArguments arguments) => throw new NotImplementedException();
        protected virtual Task PauseRequest(PauseArguments arguments) => throw new NotImplementedException();
        protected virtual Task<ReadMemoryResponse> ReadMemoryRequest(ReadMemoryArguments arguments) => throw new NotImplementedException();
        protected virtual Task RestartRequest(RestartArguments arguments) => throw new NotImplementedException();
        protected virtual Task RestartFrameRequest(RestartFrameArguments arguments) => throw new NotImplementedException();
        protected virtual Task ReverseContinueRequest(ReverseContinueArguments arguments) => throw new NotImplementedException();
        protected virtual Task<ScopesResponse> ScopesRequest(ScopesArguments arguments) => throw new NotImplementedException();
        protected virtual Task<SetBreakpointsResponse> SetBreakpointsRequest(SetBreakpointsArguments arguments) => throw new NotImplementedException();
        protected virtual Task<SetDataBreakpointsResponse> SetDataBreakpointsRequest(SetDataBreakpointsArguments arguments) => throw new NotImplementedException();
        protected virtual Task<SetExceptionBreakpointsResponse> SetExceptionBreakpointsRequest(SetExceptionBreakpointsArguments arguments) => throw new NotImplementedException();
        protected virtual Task<SetExpressionResponse> SetExpressionRequest(SetExpressionArguments arguments) => throw new NotImplementedException();
        protected virtual Task<SetFunctionBreakpointsResponse> SetFunctionBreakpointsRequest(SetFunctionBreakpointsArguments arguments) => throw new NotImplementedException();
        protected virtual Task<SetInstructionBreakpointsResponse> SetInstructionBreakpointsRequest(SetInstructionBreakpointsArguments arguments) => throw new NotImplementedException();
        protected virtual Task<SetVariableResponse> SetVariableRequest(SetVariableArguments arguments) => throw new NotImplementedException();
        protected virtual Task<SourceResponse> SourceRequest(SourceArguments arguments) => throw new NotImplementedException();
        protected virtual Task<StackTraceResponse> StackTraceRequest(StackTraceArguments arguments) => throw new NotImplementedException();
        protected virtual Task StepBackRequest(StepBackArguments arguments) => throw new NotImplementedException();
        protected virtual Task StepInRequest(StepInArguments arguments) => throw new NotImplementedException();
        protected virtual Task<StepInTargetsResponse> StepInTargetsRequest(StepInTargetsArguments arguments) => throw new NotImplementedException();
        protected virtual Task StepOutRequest(StepOutArguments arguments) => throw new NotImplementedException();
        protected virtual Task TerminateRequest(TerminateArguments arguments) => throw new NotImplementedException();
        protected virtual Task TerminateThreadsRequest(TerminateThreadsArguments arguments) => throw new NotImplementedException();
        protected virtual Task<ThreadsResponse> ThreadsRequest() => throw new NotImplementedException();
        protected virtual Task<VariablesResponse> VariablesRequest(VariablesArguments arguments) => throw new NotImplementedException();
        protected virtual Task<WriteMemoryResponse> WriteMemoryRequest(WriteMemoryArguments arguments) => throw new NotImplementedException();
    }
}

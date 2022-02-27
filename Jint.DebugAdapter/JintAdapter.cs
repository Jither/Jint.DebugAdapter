using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint.DebugAdapter.Protocol.Events;
using Jint.DebugAdapter.Protocol.Requests;
using Jint.DebugAdapter.Protocol.Responses;
using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter
{
    public class JintAdapter : Adapter
    {

        public void SendStoppedEvent(StopReason reason, string description)
        {
            SendEvent(new StoppedEvent(reason) { Description = description });
        }

        protected override void AttachRequest(AttachArguments arguments)
        {
        }

        protected override BreakpointLocationsResponse BreakpointLocationsRequest(BreakpointLocationsArguments arguments)
        {
            return new BreakpointLocationsResponse(new List<BreakpointLocation>());
        }

        protected override void CancelRequest(CancelArguments arguments)
        {
        }

        protected override CompletionsResponse CompletionsRequest(CompletionsArguments arguments)
        {
            throw new NotImplementedException();
        }

        protected override void ConfigurationDoneRequest()
        {
            SendStoppedEvent(StopReason.Entry, "Paused on entry");
        }

        protected override ContinueResponse ContinueRequest(ContinueArguments arguments)
        {
            SendStoppedEvent(StopReason.Breakpoint, "Hit breakpoint");
            return new ContinueResponse();
        }

        protected override DataBreakpointInfoResponse DataBreakpointInfoRequest(DataBreakpointInfoArguments arguments)
        {
            return new DataBreakpointInfoResponse(null, "Description");
        }

        protected override DisassembleResponse DisassembleRequest(DisassembleArguments arguments)
        {
            throw new NotImplementedException();
        }

        protected override void DisconnectRequest(DisconnectArguments arguments)
        {
        }

        protected override EvaluateResponse EvaluateRequest(EvaluateArguments arguments)
        {
            return new EvaluateResponse("evaluated");
        }

        protected override ExceptionInfoResponse ExceptionInfoRequest(ExceptionInfoArguments arguments)
        {
            throw new NotImplementedException();
        }

        protected override void GotoRequest(GotoArguments arguments)
        {

        }

        protected override GotoTargetsResponse GotoTargetsRequest(GotoTargetsArguments arguments)
        {
            throw new NotImplementedException();
        }

        protected override InitializeResponse InitializeRequest(InitializeArguments arguments)
        {
            SendEvent(new InitializedEvent());
            return new InitializeResponse
            {
                SupportsConditionalBreakpoints = true,
                SupportsConfigurationDoneRequest = true
            };
        }

        protected override void LaunchRequest(LaunchArguments arguments)
        {
        }

        protected override LoadedSourcesResponse LoadedSourcesRequest()
        {
            return new LoadedSourcesResponse(new List<Source>());
        }

        protected override ModulesResponse ModulesRequest(ModulesArguments arguments)
        {
            throw new NotImplementedException();
        }

        protected override void NextRequest(NextArguments arguments)
        {
            SendStoppedEvent(StopReason.Step, "Paused after step");
        }

        protected override void PauseRequest(PauseArguments arguments)
        {
        
        }

        protected override ReadMemoryResponse ReadMemoryRequest(ReadMemoryArguments arguments)
        {
            throw new NotImplementedException();
        }

        protected override void RestartFrameRequest(RestartFrameArguments arguments)
        {
            
        }

        protected override void RestartRequest(RestartArguments arguments)
        {
            
        }

        protected override void ReverseContinueRequest(ReverseContinueArguments arguments)
        {
            
        }

        protected override ScopesResponse ScopesRequest(ScopesArguments arguments)
        {
            return new ScopesResponse(new List<Scope>
            {
                new Scope("Global", 1)
            });
        }

        protected override SetBreakpointsResponse SetBreakpointsRequest(SetBreakpointsArguments arguments)
        {
            return new SetBreakpointsResponse(new List<Breakpoint>());
        }

        protected override SetDataBreakpointsResponse SetDataBreakpointsRequest(SetDataBreakpointsArguments arguments)
        {
            throw new NotImplementedException();
        }

        protected override SetExceptionBreakpointsResponse SetExceptionBreakpointsRequest(SetExceptionBreakpointsArguments arguments)
        {
            throw new NotImplementedException();
        }

        protected override SetExpressionResponse SetExpressionRequest(SetExpressionArguments arguments)
        {
            throw new NotImplementedException();
        }

        protected override SetFunctionBreakpointsResponse SetFunctionBreakpointsRequest(SetFunctionBreakpointsArguments arguments)
        {
            throw new NotImplementedException();
        }

        protected override SetInstructionBreakpointsResponse SetInstructionBreakpointsRequest(SetInstructionBreakpointsArguments arguments)
        {
            throw new NotImplementedException();
        }

        protected override SetVariableResponse SetVariableRequest(SetVariableArguments arguments)
        {
            throw new NotImplementedException();
        }

        protected override SourceResponse SourceRequest(SourceArguments arguments)
        {
            throw new NotImplementedException();
        }

        protected override StackTraceResponse StackTraceRequest(StackTraceArguments arguments)
        {
            return new StackTraceResponse(new List<StackFrame> { new StackFrame(1, "global") });
        }

        protected override void StepBackRequest(StepBackArguments arguments)
        {
            SendStoppedEvent(StopReason.Step, "Paused after step");
        }

        protected override void StepInRequest(StepInArguments arguments)
        {
            SendStoppedEvent(StopReason.Step, "Paused after step");
        }

        protected override StepInTargetsResponse StepInTargetsRequest(StepInTargetsArguments arguments)
        {
            throw new NotImplementedException();
        }

        protected override void StepOutRequest(StepOutArguments arguments)
        {
            SendStoppedEvent(StopReason.Step, "Paused after step");
        }

        protected override void TerminateRequest(TerminateArguments arguments)
        {
        }

        protected override void TerminateThreadsRequest(TerminateThreadsArguments arguments)
        {
        }

        protected override ThreadsResponse ThreadsRequest()
        {
            // "Even if a debug adapter does not support multiple threads, it must implement the threads request and return a single (dummy) thread."
            return new ThreadsResponse(new List<Protocol.Types.Thread> { new Protocol.Types.Thread(1, "Main Thread") });
        }

        protected override VariablesResponse VariablesRequest(VariablesArguments arguments)
        {
            return new VariablesResponse(new List<Variable>
                {
                    new Variable("x", "3"),
                    new Variable("test", "Hello World")
                });
        }

        protected override WriteMemoryResponse WriteMemoryRequest(WriteMemoryArguments arguments)
        {
            return new WriteMemoryResponse();
        }

    }
}

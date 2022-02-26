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
            return new DisassembleResponse();
        }

        protected override void DisconnectRequest(DisconnectArguments arguments)
        {
        }

        protected override EvaluateResponse EvaluateRequest(EvaluateArguments arguments)
        {
            return new EvaluateResponse
            {
                Result = "evaluated"
            };
        }

        protected override ExceptionInfoResponse ExceptionInfoRequest(ExceptionInfoArguments arguments)
        {
            return new ExceptionInfoResponse();
        }

        protected override void GotoRequest(GotoArguments arguments)
        {

        }

        protected override GotoTargetsResponse GotoTargetsRequest(GotoTargetsArguments arguments)
        {
            return new GotoTargetsResponse();
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
            return new LoadedSourcesResponse();
        }

        protected override ModulesResponse ModulesRequest(ModulesArguments arguments)
        {
            return new ModulesResponse();
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
            return new ReadMemoryResponse();
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
            return new ScopesResponse
            {
                Scopes = new List<Scope>
                {
                    new Scope
                    {
                        Name = "Global",
                        VariablesReference = 1
                    }
                }
            };
        }

        protected override SetBreakpointsResponse SetBreakpointsRequest(SetBreakpointsArguments arguments)
        {
            return new SetBreakpointsResponse();
        }

        protected override SetDataBreakpointsResponse SetDataBreakpointsRequest(SetDataBreakpointsArguments arguments)
        {
            return new SetDataBreakpointsResponse();
        }

        protected override SetExceptionBreakpointsResponse SetExceptionBreakpointsRequest(SetExceptionBreakpointsArguments arguments)
        {
            return new SetExceptionBreakpointsResponse(new List<Breakpoint>());
        }

        protected override SetExpressionResponse SetExpressionRequest(SetExpressionArguments arguments)
        {
            return new SetExpressionResponse();
        }

        protected override SetFunctionBreakpointsResponse SetFunctionBreakpointsRequest(SetFunctionBreakpointsArguments arguments)
        {
            return new SetFunctionBreakpointsResponse();
        }

        protected override SetInstructionBreakpointsResponse SetInstructionBreakpointsRequest(SetInstructionBreakpointsArguments arguments)
        {
            return new SetInstructionBreakpointsResponse(new List<Breakpoint>());
        }

        protected override SetVariableResponse SetVariableRequest(SetVariableArguments arguments)
        {
            return new SetVariableResponse();
        }

        protected override SourceResponse SourceRequest(SourceArguments arguments)
        {
            return new SourceResponse();
        }

        protected override StackTraceResponse StackTraceRequest(StackTraceArguments arguments)
        {
            return new StackTraceResponse();
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
            SendStoppedEvent(StopReason.Step, "Paused after step");
            return new StepInTargetsResponse();
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
            return new ThreadsResponse
            {
                Threads = new List<Protocol.Types.Thread> { new Protocol.Types.Thread { Id = 1, Name = "Main Thread" } }
            };
        }

        protected override VariablesResponse VariablesRequest(VariablesArguments arguments)
        {
            return new VariablesResponse
            {
                Variables = new List<Variable>
                {
                    new Variable
                    {
                        Name = "x",
                        Value = "3"
                    },
                    new Variable
                    {
                        Name = "test",
                        Value = "Hello World"
                    }
                }
            };
        }

        protected override WriteMemoryResponse WriteMemoryRequest(WriteMemoryArguments arguments)
        {
            return new WriteMemoryResponse();
        }

    }
}

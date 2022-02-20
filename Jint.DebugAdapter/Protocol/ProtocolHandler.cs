using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint.DebugAdapter.Protocol.Events;
using Jint.DebugAdapter.Protocol.Requests;
using Jint.DebugAdapter.Protocol.Responses;
using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol
{

    internal class ProtocolHandler
    {
        public DebugProtocol Protocol { get; set; }

        public ProtocolResponseBody HandleRequest(BaseProtocolRequest request)
        {
            return request.UntypedArguments switch
            {
                AttachArguments args => AttachRequest(args),
                BreakpointLocationsArguments args => BreakpointLocationsRequest(args),
                CompletionsArguments args => CompletionsRequest(args),
                CancelArguments args => CancelRequest(args),
                ContinueArguments args => ContinueRequest(args),
                DataBreakpointInfoArguments args => DataBreakpointInfoRequest(args),
                DisassembleArguments args => DisassembleRequest(args),
                DisconnectArguments args => DisconnectRequest(args),
                EvaluateArguments args => EvaluateRequest(args),
                ExceptionInfoArguments args => ExceptionInfoRequest(args),
                GotoArguments args => GotoRequest(args),
                GotoTargetsArguments args => GotoTargetsRequest(args),
                InitializeArguments args => InitializeRequest(args),
                LaunchArguments args => LaunchRequest(args),
                ModulesArguments args => ModulesRequest(args),
                NextArguments args => NextRequest(args),
                PauseArguments args => PauseRequest(args),
                ReadMemoryArguments args => ReadMemoryRequest(args),
                RestartArguments args => RestartRequest(args),
                RestartFrameArguments args => RestartFrameRequest(args),
                ReverseContinueArguments args => ReverseContinueRequest(args),
                RunInTerminalArguments args => RunInTerminalRequest(args), // Note: Reverse Request (DebugAdapter to host)
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
                StepBackArguments args => StepBackRequest(args),
                StepInArguments args => StepInRequest(args),
                StepInTargetsArguments args => StepInTargetsRequest(args),
                StepOutArguments args => StepOutRequest(args),
                TerminateArguments args => TerminateRequest(args),
                VariablesArguments args => VariablesRequest(args),
                WriteMemoryArguments args => WriteMemoryRequest(args),

                null => request.Command switch
                {
                    CommandNames.ConfigurationDone => ConfigurationDoneRequest(),
                    CommandNames.LoadedSources => LoadedSourcesRequest(),
                    CommandNames.Threads => ThreadsRequest(),
                    _ => throw new NotImplementedException($"Request command '{request.Command}' not implemented.")
                },

                _ => throw new NotImplementedException($"Request command '{request.Command}' not implemented.")
            };
        }

        public void SendStoppedEvent(StopReason reason, string description)
        {
            SendEvent(new StoppedEvent
            {
                Reason = reason,
                Description = description,
                ThreadId = 1,
            });
        }

        protected AttachResponse AttachRequest(AttachArguments arguments)
        {
            return new AttachResponse();
        }

        protected BreakpointLocationsResponse BreakpointLocationsRequest(BreakpointLocationsArguments arguments)
        {
            return new BreakpointLocationsResponse();
        }

        protected CancelResponse CancelRequest(CancelArguments arguments)
        {
            return new CancelResponse();
        }

        protected CompletionsResponse CompletionsRequest(CompletionsArguments arguments)
        {
            return new CompletionsResponse();
        }

        protected ConfigurationDoneResponse ConfigurationDoneRequest()
        {
            SendStoppedEvent(StopReason.Entry, "Paused on entry");
            return new ConfigurationDoneResponse();
        }

        protected ContinueResponse ContinueRequest(ContinueArguments arguments)
        {
            SendStoppedEvent(StopReason.Breakpoint, "Hit breakpoint");
            return new ContinueResponse();
        }

        protected DataBreakpointInfoResponse DataBreakpointInfoRequest(DataBreakpointInfoArguments arguments)
        {
            return new DataBreakpointInfoResponse();
        }

        protected DisassembleResponse DisassembleRequest(DisassembleArguments arguments)
        {
            return new DisassembleResponse();
        }

        protected DisconnectResponse DisconnectRequest(DisconnectArguments arguments)
        {
            return new DisconnectResponse();
        }

        protected EvaluateResponse EvaluateRequest(EvaluateArguments arguments)
        {
            return new EvaluateResponse
            {
                Result = "evaluated"
            };
        }

        protected ExceptionInfoResponse ExceptionInfoRequest(ExceptionInfoArguments arguments)
        {
            return new ExceptionInfoResponse();
        }

        protected GotoResponse GotoRequest(GotoArguments arguments)
        {
            return new GotoResponse();
        }

        protected GotoTargetsResponse GotoTargetsRequest(GotoTargetsArguments arguments)
        {
            return new GotoTargetsResponse();
        }

        protected InitializeResponse InitializeRequest(InitializeArguments arguments)
        {
            SendEvent(new InitializedEvent());
            return new InitializeResponse
            {
                SupportsConditionalBreakpoints = true,
                SupportsConfigurationDoneRequest = true
            };
        }

        protected LaunchResponse LaunchRequest(LaunchArguments arguments)
        {
            return new LaunchResponse();
        }

        protected LoadedSourcesResponse LoadedSourcesRequest()
        {
            return new LoadedSourcesResponse();
        }

        protected ModulesResponse ModulesRequest(ModulesArguments arguments)
        {
            return new ModulesResponse();
        }

        protected NextResponse NextRequest(NextArguments arguments)
        {
            SendStoppedEvent(StopReason.Step, "Paused after step");
            return new NextResponse();
        }

        protected PauseResponse PauseRequest(PauseArguments arguments)
        {
            return new PauseResponse();
        }

        protected ReadMemoryResponse ReadMemoryRequest(ReadMemoryArguments arguments)
        {
            return new ReadMemoryResponse();
        }

        protected RestartResponse RestartRequest(RestartArguments arguments)
        {
            return new RestartResponse();
        }

        protected RestartFrameResponse RestartFrameRequest(RestartFrameArguments arguments)
        {
            return new RestartFrameResponse();
        }

        protected ReverseContinueResponse ReverseContinueRequest(ReverseContinueArguments arguments)
        {
            return new ReverseContinueResponse();
        }

        protected RunInTerminalResponse RunInTerminalRequest(RunInTerminalArguments arguments)
        {
            return new RunInTerminalResponse();
        }

        protected ScopesResponse ScopesRequest(ScopesArguments arguments)
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

        protected SetBreakpointsResponse SetBreakpointsRequest(SetBreakpointsArguments arguments)
        {
            return new SetBreakpointsResponse()
            {
                Breakpoints = arguments.Breakpoints.Select(b => new Breakpoint { Line = b.Line, Verified = true }).ToList()
            };
        }

        protected SetDataBreakpointsResponse SetDataBreakpointsRequest(SetDataBreakpointsArguments arguments)
        {
            return new SetDataBreakpointsResponse();
        }

        protected SetExceptionBreakpointsResponse SetExceptionBreakpointsRequest(SetExceptionBreakpointsArguments arguments)
        {
            return new SetExceptionBreakpointsResponse();
        }

        protected SetExpressionResponse SetExpressionRequest(SetExpressionArguments arguments)
        {
            return new SetExpressionResponse();
        }

        protected SetFunctionBreakpointsResponse SetFunctionBreakpointsRequest(SetFunctionBreakpointsArguments arguments)
        {
            return new SetFunctionBreakpointsResponse();
        }

        protected SetInstructionBreakpointsResponse SetInstructionBreakpointsRequest(SetInstructionBreakpointsArguments arguments)
        {
            return new SetInstructionBreakpointsResponse();
        }

        protected SetVariableResponse SetVariableRequest(SetVariableArguments arguments)
        {
            return new SetVariableResponse();
        }

        protected SourceResponse SourceRequest(SourceArguments arguments)
        {
            return new SourceResponse();
        }

        protected StackTraceResponse StackTraceRequest(StackTraceArguments arguments)
        {
            return new StackTraceResponse()
            {
                StackFrames = new List<StackFrame>
                {
                    new StackFrame
                    {
                        Id = 1,
                        Name = "global"
                    }
                }
            };
        }

        protected StepBackResponse StepBackRequest(StepBackArguments arguments)
        {
            SendStoppedEvent(StopReason.Step, "Paused after step");
            return new StepBackResponse();
        }

        protected StepInResponse StepInRequest(StepInArguments arguments)
        {
            SendStoppedEvent(StopReason.Step, "Paused after step");
            return new StepInResponse();
        }

        protected StepInTargetsResponse StepInTargetsRequest(StepInTargetsArguments arguments)
        {
            SendStoppedEvent(StopReason.Step, "Paused after step");
            return new StepInTargetsResponse();
        }

        protected StepOutResponse StepOutRequest(StepOutArguments arguments)
        {
            SendStoppedEvent(StopReason.Step, "Paused after step");
            return new StepOutResponse();
        }

        protected TerminateResponse TerminateRequest(TerminateArguments arguments)
        {
            return new TerminateResponse();
        }

        protected TerminateThreadsResponse TerminateThreadsRequest(TerminateThreadsArguments arguments)
        {
            return new TerminateThreadsResponse();
        }

        protected ThreadsResponse ThreadsRequest()
        {
            // "Even if a debug adapter does not support multiple threads, it must implement the threads request and return a single (dummy) thread."
            return new ThreadsResponse
            {
                Threads = new List<Types.Thread> { new Types.Thread { Id = 1, Name = "Main Thread" } }
            };
        }

        protected VariablesResponse VariablesRequest(VariablesArguments arguments)
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

        protected WriteMemoryResponse WriteMemoryRequest(WriteMemoryArguments arguments)
        {
            return new WriteMemoryResponse();
        }

        protected void SendEvent(ProtocolEventBody body)
        {
            var evt = new ProtocolEvent(body.EventName, body);
            Protocol.SendEvent(evt);
        }
    }
}

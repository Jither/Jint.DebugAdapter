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
                AttachArguments args => HandleAttachRequest(args),
                BreakpointLocationsArguments args => HandleBreakpointLocationsRequest(args),
                CompletionsArguments args => HandleCompletionsRequest(args),
                CancelArguments args => HandleCancelRequest(args),
                ContinueArguments args => HandleContinueRequest(args),
                DataBreakpointInfoArguments args => HandleDataBreakpointInfoRequest(args),
                DisassembleArguments args => HandleDisassembleRequest(args),
                DisconnectArguments args => HandleDisconnectRequest(args),
                EvaluateArguments args => HandleEvaluateRequest(args),
                ExceptionInfoArguments args => HandleExceptionInfoRequest(args),
                GotoArguments args => HandleGotoRequest(args),
                GotoTargetsArguments args => HandleGotoTargetsRequest(args),
                InitializeArguments args => HandleInitializeRequest(args),
                LaunchArguments args => HandleLaunchRequest(args),
                ModulesArguments args => HandleModulesRequest(args),
                NextArguments args => HandleNextRequest(args),
                PauseArguments args => HandlePauseRequest(args),
                ReadMemoryArguments args => HandleReadMemoryRequest(args),
                RestartArguments args => HandleRestartRequest(args),
                RestartFrameArguments args => HandleRestartFrameRequest(args),
                ReverseContinueArguments args => HandleReverseContinueRequest(args),
                RunInTerminalArguments args => HandleRunInTerminalRequest(args), // Note: Reverse request (DebugAdapter to host)
                ScopesArguments args => HandleScopesRequest(args),
                SetBreakpointsArguments args => HandleSetBreakpointsRequest(args),
                SetDataBreakpointsArguments args => HandleSetDataBreakpointsRequest(args),
                SetExceptionBreakpointsArguments args => HandleSetExceptionBreakpointsRequest(args),
                SetExpressionArguments args => HandleSetExpressionRequest(args),
                SetFunctionBreakpointsArguments args => HandleSetFunctionBreakpointsRequest(args),
                SetInstructionBreakpointsArguments args => HandleSetInstructionBreakpointsRequest(args),
                SetVariableArguments args => HandleSetVariableRequest(args),
                SourceArguments args => HandleSourceRequest(args),
                StackTraceArguments args => HandleStackTraceRequest(args),
                StepBackArguments args => HandleStepBackRequest(args),
                StepInArguments args => HandleStepInRequest(args),
                StepInTargetsArguments args => HandleStepInTargetsRequest(args),
                StepOutArguments args => HandleStepOutRequest(args),
                TerminateArguments args => HandleTerminateRequest(args),
                VariablesArguments args => HandleVariablesRequest(args),
                WriteMemoryArguments args => HandleWriteMemoryRequest(args),

                null => request.Command switch
                {
                    CommandNames.ConfigurationDone => HandleConfigurationDoneRequest(),
                    CommandNames.LoadedSources => HandleLoadedSourcesRequest(),
                    CommandNames.Threads => HandleThreadsRequest(),
                    _ => throw new NotImplementedException($"Request command '{request.Command}' not implemented.")
                },

                _ => throw new NotImplementedException($"Request command '{request.Command}' not implemented.")
            };
        }

        public void SendStoppedEvent(StopReason reason, string description)
        {
            SendEvent(new StoppedEventBody
            {
                Reason = reason,
                Description = description,
                ThreadId = 1,
            });
        }

        protected AttachResponseBody HandleAttachRequest(AttachArguments arguments)
        {
            return new AttachResponseBody();
        }

        protected BreakpointLocationsResponseBody HandleBreakpointLocationsRequest(BreakpointLocationsArguments arguments)
        {
            return new BreakpointLocationsResponseBody();
        }

        protected CancelResponseBody HandleCancelRequest(CancelArguments arguments)
        {
            return new CancelResponseBody();
        }

        protected CompletionsResponseBody HandleCompletionsRequest(CompletionsArguments arguments)
        {
            return new CompletionsResponseBody();
        }

        protected ConfigurationDoneResponseBody HandleConfigurationDoneRequest()
        {
            SendStoppedEvent(StopReason.Entry, "Paused on entry");
            return new ConfigurationDoneResponseBody();
        }

        protected ContinueResponseBody HandleContinueRequest(ContinueArguments arguments)
        {
            SendStoppedEvent(StopReason.Breakpoint, "Hit breakpoint");
            return new ContinueResponseBody();
        }

        protected DataBreakpointInfoResponseBody HandleDataBreakpointInfoRequest(DataBreakpointInfoArguments arguments)
        {
            return new DataBreakpointInfoResponseBody();
        }

        protected DisassembleResponseBody HandleDisassembleRequest(DisassembleArguments arguments)
        {
            return new DisassembleResponseBody();
        }

        protected DisconnectResponseBody HandleDisconnectRequest(DisconnectArguments arguments)
        {
            return new DisconnectResponseBody();
        }

        protected EvaluateResponseBody HandleEvaluateRequest(EvaluateArguments arguments)
        {
            return new EvaluateResponseBody
            {
                Result = "evaluated"
            };
        }

        protected ExceptionInfoResponseBody HandleExceptionInfoRequest(ExceptionInfoArguments arguments)
        {
            return new ExceptionInfoResponseBody();
        }

        protected GotoResponseBody HandleGotoRequest(GotoArguments arguments)
        {
            return new GotoResponseBody();
        }

        protected GotoTargetsResponseBody HandleGotoTargetsRequest(GotoTargetsArguments arguments)
        {
            return new GotoTargetsResponseBody();
        }

        protected InitializeResponseBody HandleInitializeRequest(InitializeArguments arguments)
        {
            SendEvent(new InitializedEventBody());
            return new InitializeResponseBody
            {
                SupportsConditionalBreakpoints = true,
                SupportsConfigurationDoneRequest = true
            };
        }

        protected LaunchResponseBody HandleLaunchRequest(LaunchArguments arguments)
        {
            return new LaunchResponseBody();
        }

        protected LoadedSourcesResponseBody HandleLoadedSourcesRequest()
        {
            return new LoadedSourcesResponseBody();
        }

        protected ModulesResponseBody HandleModulesRequest(ModulesArguments arguments)
        {
            return new ModulesResponseBody();
        }

        protected NextResponseBody HandleNextRequest(NextArguments arguments)
        {
            SendStoppedEvent(StopReason.Step, "Paused after step");
            return new NextResponseBody();
        }

        protected PauseResponseBody HandlePauseRequest(PauseArguments arguments)
        {
            return new PauseResponseBody();
        }

        protected ReadMemoryResponseBody HandleReadMemoryRequest(ReadMemoryArguments arguments)
        {
            return new ReadMemoryResponseBody();
        }

        protected RestartResponseBody HandleRestartRequest(RestartArguments arguments)
        {
            return new RestartResponseBody();
        }

        protected RestartFrameResponseBody HandleRestartFrameRequest(RestartFrameArguments arguments)
        {
            return new RestartFrameResponseBody();
        }

        protected ReverseContinueResponseBody HandleReverseContinueRequest(ReverseContinueArguments arguments)
        {
            return new ReverseContinueResponseBody();
        }

        protected RunInTerminalResponseBody HandleRunInTerminalRequest(RunInTerminalArguments arguments)
        {
            return new RunInTerminalResponseBody();
        }

        protected ScopesResponseBody HandleScopesRequest(ScopesArguments arguments)
        {
            return new ScopesResponseBody
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

        protected SetBreakpointsResponseBody HandleSetBreakpointsRequest(SetBreakpointsArguments arguments)
        {
            return new SetBreakpointsResponseBody()
            {
                Breakpoints = arguments.Breakpoints.Select(b => new Breakpoint { Line = b.Line, Verified = true }).ToList()
            };
        }

        protected SetDataBreakpointsResponseBody HandleSetDataBreakpointsRequest(SetDataBreakpointsArguments arguments)
        {
            return new SetDataBreakpointsResponseBody();
        }

        protected SetExceptionBreakpointsResponseBody HandleSetExceptionBreakpointsRequest(SetExceptionBreakpointsArguments arguments)
        {
            return new SetExceptionBreakpointsResponseBody();
        }

        protected SetExpressionResponseBody HandleSetExpressionRequest(SetExpressionArguments arguments)
        {
            return new SetExpressionResponseBody();
        }

        protected SetFunctionBreakpointsResponseBody HandleSetFunctionBreakpointsRequest(SetFunctionBreakpointsArguments arguments)
        {
            return new SetFunctionBreakpointsResponseBody();
        }

        protected SetInstructionBreakpointsResponseBody HandleSetInstructionBreakpointsRequest(SetInstructionBreakpointsArguments arguments)
        {
            return new SetInstructionBreakpointsResponseBody();
        }

        protected SetVariableResponseBody HandleSetVariableRequest(SetVariableArguments arguments)
        {
            return new SetVariableResponseBody();
        }

        protected SourceResponseBody HandleSourceRequest(SourceArguments arguments)
        {
            return new SourceResponseBody();
        }

        protected StackTraceResponseBody HandleStackTraceRequest(StackTraceArguments arguments)
        {
            return new StackTraceResponseBody()
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

        protected StepBackResponseBody HandleStepBackRequest(StepBackArguments arguments)
        {
            SendStoppedEvent(StopReason.Step, "Paused after step");
            return new StepBackResponseBody();
        }

        protected StepInResponseBody HandleStepInRequest(StepInArguments arguments)
        {
            SendStoppedEvent(StopReason.Step, "Paused after step");
            return new StepInResponseBody();
        }

        protected StepInTargetsResponseBody HandleStepInTargetsRequest(StepInTargetsArguments arguments)
        {
            SendStoppedEvent(StopReason.Step, "Paused after step");
            return new StepInTargetsResponseBody();
        }

        protected StepOutResponseBody HandleStepOutRequest(StepOutArguments arguments)
        {
            SendStoppedEvent(StopReason.Step, "Paused after step");
            return new StepOutResponseBody();
        }

        protected TerminateResponseBody HandleTerminateRequest(TerminateArguments arguments)
        {
            return new TerminateResponseBody();
        }

        protected TerminateThreadsResponseBody HandleTerminateThreadsRequest(TerminateThreadsArguments arguments)
        {
            return new TerminateThreadsResponseBody();
        }

        protected ThreadsResponseBody HandleThreadsRequest()
        {
            // "Even if a debug adapter does not support multiple threads, it must implement the threads request and return a single (dummy) thread."
            return new ThreadsResponseBody
            {
                Threads = new List<Types.Thread> { new Types.Thread { Id = 1, Name = "Main Thread" } }
            };
        }

        protected VariablesResponseBody HandleVariablesRequest(VariablesArguments arguments)
        {
            return new VariablesResponseBody
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

        protected WriteMemoryResponseBody HandleWriteMemoryRequest(WriteMemoryArguments arguments)
        {
            return new WriteMemoryResponseBody();
        }

        protected void SendEvent(ProtocolEventBody body)
        {
            var name = body switch
            {
                BreakpointEventBody => EventNames.Breakpoint,
                CapabilitiesEventBody => EventNames.Capabilities,
                ContinuedEventBody => EventNames.Continued,
                ExitedEventBody => EventNames.Exited,
                InitializedEventBody => EventNames.Initialized,
                InvalidatedEventBody => EventNames.Invalidated,
                LoadedSourceEventBody => EventNames.LoadedSource,
                MemoryEventBody => EventNames.Memory,
                ModuleEventBody => EventNames.Module,
                OutputEventBody => EventNames.Output,
                ProcessEventBody => EventNames.Process,
                ProgressEndEventBody => EventNames.ProgressEnd,
                ProgressStartEventBody => EventNames.ProgressStart,
                ProgressUpdateEventBody => EventNames.ProgressUpdate,
                StoppedEventBody => EventNames.Stopped,
                TerminatedEventBody => EventNames.Terminated,
                ThreadEventBody => EventNames.Thread,
                _ => throw new NotImplementedException($"Event for body {body.GetType()} not implemented.")
            };

            var evt = new ProtocolEvent(name, body);
            Protocol.SendEvent(evt);
        }
    }
}

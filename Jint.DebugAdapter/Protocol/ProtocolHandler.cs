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
                CancelArguments args => HandleCancelRequest(args),
                ContinueArguments args => HandleContinueRequest(args),
                DisconnectArguments args => HandleDisconnectRequest(args),
                EvaluateArguments args => HandleEvaluateRequest(args),
                InitializeArguments args => HandleInitializeRequest(args),
                LaunchArguments args => HandleLaunchRequest(args),
                NextArguments args => HandleNextRequest(args),
                ScopesArguments args => HandleScopesRequest(args),
                SetBreakpointsArguments args => HandleSetBreakpointsRequest(args),
                StackTraceArguments args => HandleStackTraceRequest(args),
                StepInArguments args => HandleStepInRequest(args),
                StepOutArguments args => HandleStepOutRequest(args),
                VariablesArguments args => HandleVariablesRequest(args),

                null => request.Command switch
                {
                    CommandNames.ConfigurationDone => HandleConfigurationDoneRequest(),
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

        protected CancelResponseBody HandleCancelRequest(CancelArguments arguments)
        {
            return new CancelResponseBody();
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

        protected NextResponseBody HandleNextRequest(NextArguments arguments)
        {
            SendStoppedEvent(StopReason.Step, "Paused after step");
            return new NextResponseBody();
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

        protected StepInResponseBody HandleStepInRequest(StepInArguments arguments)
        {
            SendStoppedEvent(StopReason.Step, "Paused after step");
            return new StepInResponseBody();
        }

        protected StepOutResponseBody HandleStepOutRequest(StepOutArguments arguments)
        {
            SendStoppedEvent(StopReason.Step, "Paused after step");
            return new StepOutResponseBody();
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

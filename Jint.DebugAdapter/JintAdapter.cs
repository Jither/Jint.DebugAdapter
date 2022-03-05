using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint.DebugAdapter.Protocol.Events;
using Jint.DebugAdapter.Protocol.Requests;
using Jint.DebugAdapter.Protocol.Responses;
using Jint.DebugAdapter.Protocol.Types;
using Jint.Native.Object;
using Jint.Runtime.Debugger;

namespace Jint.DebugAdapter
{
    public abstract class VariableContainer
    {
        public int Id { get; }
        
        protected VariableContainer(int id)
        {
            Id = id;
        }

        public IEnumerable<Variable> GetVariables()
        {
            return InternalGetVariables();
        }

        protected abstract IEnumerable<Variable> InternalGetVariables();
    }

    public class ScopeVariableContainer : VariableContainer
    {
        private readonly DebugScope scope;

        public ScopeVariableContainer(int id, DebugScope scope) : base(id)
        {
            this.scope = scope;
        }

        protected override IEnumerable<Variable> InternalGetVariables()
        {
            return scope.BindingNames.Select(n => new Variable(n, scope.GetBindingValue(n).ToString()));
        }
    }

    public class ObjectVariableContainer : VariableContainer
    {
        private readonly ObjectInstance instance;

        public ObjectVariableContainer(int id, ObjectInstance instance) : base(id)
        {
            this.instance = instance;
        }

        protected override IEnumerable<Variable> InternalGetVariables()
        {
            throw new NotImplementedException();
        }
    }

    public class VariableStore
    {
        private int nextId = 1;
        private readonly Dictionary<int, VariableContainer> containers = new();

        public int Add(DebugScope scope)
        {
            var container = new ScopeVariableContainer(nextId++, scope);
            containers.Add(container.Id, container);
            return container.Id;
        }

        public int Add(ObjectInstance instance)
        {
            var container = new ObjectVariableContainer(nextId++, instance);
            containers.Add(container.Id, container);
            return container.Id;
        }

        public VariableContainer GetContainer(int id)
        {
            return containers[id];
        }

        public void Clear()
        {
            containers.Clear();
        }
    }

    public class JintAdapter : Adapter
    {
        private readonly Debugger debugger;
        private readonly VariableStore variableStore = new();
        private InitializeArguments clientCapabilities;

        public JintAdapter(Debugger debugger)
        {
            this.debugger = debugger;
            debugger.Continue += Debugger_Continue;
            debugger.Stop += Debugger_Stop;
        }

        private void Debugger_Stop(PauseReason reason, DebugInformation info)
        {
            variableStore.Clear();

            SendEvent(new StoppedEvent(
                reason switch
                {
                    PauseReason.Breakpoint => StopReason.Breakpoint,
                    PauseReason.Entry => StopReason.Entry,
                    PauseReason.Exception => StopReason.Exception,
                    PauseReason.Pause => StopReason.Pause,
                    PauseReason.Step => StopReason.Step,
                    _ => throw new NotImplementedException($"DebugAdapter reason not implemented for {reason}")
                }
            )
            {
                Description = reason switch
                {
                    PauseReason.Breakpoint => "Hit breakpoint",
                    PauseReason.Entry => "Stopped on entry",
                    PauseReason.Exception => "An error occurred",
                    PauseReason.Pause => "Paused by user",
                    PauseReason.Step => "Stopped after step",
                    _ => throw new NotImplementedException($"DebugAdapter reason not implemented for {reason}")
                }
            });
        }

        private void Debugger_Continue()
        {
            SendEvent(new ContinuedEvent());
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

        protected override void ConfigurationDoneRequest()
        {
            // TODO: Run the debugger here?
        }

        protected override ContinueResponse ContinueRequest(ContinueArguments arguments)
        {
            return new ContinueResponse();
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

        protected override InitializeResponse InitializeRequest(InitializeArguments arguments)
        {
            Logger.Log($"Connection established from: {arguments.ClientName} ({arguments.ClientId})");
            clientCapabilities = arguments;

            SendEvent(new InitializedEvent());
            return new InitializeResponse
            {
                SupportsConditionalBreakpoints = true,
                SupportsConfigurationDoneRequest = true
            };
        }

        protected override void LaunchRequest(LaunchArguments arguments)
        {
            string path = arguments.AdditionalProperties["program"].GetString();
            bool stopOnEntry = arguments.AdditionalProperties["stopOnEntry"].GetBoolean();
            debugger.ExecuteAsync(path, noDebug: arguments.NoDebug ?? false, pauseOnEntry: stopOnEntry);
        }

        protected override LoadedSourcesResponse LoadedSourcesRequest()
        {
            return new LoadedSourcesResponse(new List<Source>());
        }

        protected override void NextRequest(NextArguments arguments)
        {
            debugger.StepOver();
        }

        protected override void PauseRequest(PauseArguments arguments)
        {
            debugger.Pause();
        }

        protected override void RestartRequest(RestartArguments arguments)
        {
            
        }

        protected override ScopesResponse ScopesRequest(ScopesArguments arguments)
        {
            var frame = debugger.CurrentDebugInformation.CallStack[arguments.FrameId];
            return new ScopesResponse(frame.ScopeChain.Select(s =>
                new Scope(s.ScopeType.ToString(), variableStore.Add(s))
            ));
        }

        protected override SetBreakpointsResponse SetBreakpointsRequest(SetBreakpointsArguments arguments)
        {
            return new SetBreakpointsResponse(new List<Breakpoint>());
        }

        protected override StackTraceResponse StackTraceRequest(StackTraceArguments arguments)
        {
            return new StackTraceResponse(debugger.CurrentDebugInformation.CallStack.Select((frame, index) =>
                new StackFrame(index, frame.FunctionName)
                {
                    Source = new Source
                    {
                        Path = frame.Location.Source
                    },
                    Line = frame.Location.Start.Line,
                    Column = frame.Location.Start.Column,
                    EndLine = frame.Location.End.Line,
                    EndColumn = frame.Location.End.Column
                }
            ));
        }

        protected override void StepInRequest(StepInArguments arguments)
        {
            debugger.StepInto();
        }

        protected override void StepOutRequest(StepOutArguments arguments)
        {
            debugger.StepOut();
        }

        protected override void TerminateRequest(TerminateArguments arguments)
        {
        }

        protected override ThreadsResponse ThreadsRequest()
        {
            // "Even if a debug adapter does not support multiple threads, it must implement the threads request and return a single (dummy) thread."
            return new ThreadsResponse(new List<Protocol.Types.Thread> { new Protocol.Types.Thread(1, "Main Thread") });
        }

        protected override VariablesResponse VariablesRequest(VariablesArguments arguments)
        {
            var container = variableStore.GetContainer(arguments.VariablesReference);
            var variables = container.GetVariables();
            return new VariablesResponse(variables);
        }
    }
}

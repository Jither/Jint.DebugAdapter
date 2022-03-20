﻿using Jither.DebugAdapter.Protocol.Events;
using Jither.DebugAdapter.Protocol.Requests;
using Jither.DebugAdapter.Protocol.Responses;
using Jither.DebugAdapter.Protocol.Types;
using Jint.Runtime.Debugger;
using Jither.DebugAdapter;
using Thread = Jither.DebugAdapter.Protocol.Types.Thread;
using Jither.DebugAdapter.Helpers;
using Esprima;
using Jint.DebugAdapter.Variables;

namespace Jint.DebugAdapter
{
    public class SourceLocation
    {
        public Position Start { get; }
        public Position End { get; }
        public Source Source { get; }

        public SourceLocation(Source source, Position start, Position end)
        {
            Source = source;
            Start = start;
            End = end;
        }
    }

    public class JintAdapter : Adapter
    {
        private readonly Logger logger = LogManager.GetLogger();
        private readonly IScriptHost host;
        private readonly Debugger debugger;
        private readonly VariableStore variableStore;

        private bool clientLinesStartAt1;
        private bool clientColumnsStartAt1;
        private bool shouldTerminateOnDisconnect;

        private bool restarting;
        private DebugInformation currentDebugInformation;
    
        public Console Console { get; }

        public SourceLocation CurrentLocation
        {
            get
            {
                var location = debugger?.CurrentLocation;
                if (location == null)
                {
                    return null;
                }
                return ToClientSourceLocation(location.Value);
            }
        }

        public JintAdapter(IScriptHost host)
        {
            this.host = host;
            debugger = host.Debugger;
            Console = new Console(this);
            variableStore = new VariableStore();

            debugger.Resumed += Debugger_Resumed;
            debugger.Paused += Debugger_Paused;
            debugger.Cancelled += Debugger_Cancelled;
            debugger.Done += Debugger_Done;
            debugger.LogPoint += Debugger_LogPoint;
            debugger.Error += Debugger_Error;
        }

        private void Debugger_Error(Exception ex)
        {
            SendEvent(new StoppedEvent(StopReason.Exception)
            {
                Description = "Fatal error",
                Text = ex.Message
            });
            // TODO: We cannot send Terminated event, because the debug session will be stopped without showing
            // the exception
            //SendEvent(new TerminatedEvent());
        }

        private void Debugger_LogPoint(string message, DebugInformation e)
        {
            // TODO: Something is messing with the stack frames (and probably other things).
            // Thread desynchronization due to outputting while running?
            Console.Send(OutputCategory.Stdout, message);
        }

        private void Debugger_Done()
        {
            // "In all situations where a debug adapter wants to end the debug session,
            // a terminated event must be fired."
            SendEvent(new TerminatedEvent());
            
            // "If the debuggee has ended (and the debug adapter is able to detect this), an optional exited
            // event can be issued to return the exit code to the development tool."
            // Do we have any need for an exit code?
        }

        private void Debugger_Cancelled()
        {
            // TODO: We can't stop protocol here, because Cancelled also happens when handling Restart.
            // Check if we *ever* need to stop the protocol
            if (!restarting)
            {
                Protocol.Stop();
            }
        }

        private void Debugger_Paused(PauseReason reason, DebugInformation info)
        {
            currentDebugInformation = info;
            variableStore.Clear();

            SendEvent(reason switch
            {
                PauseReason.Breakpoint => new StoppedEvent(StopReason.Breakpoint) { Description = "Hit breakpoint" },
                PauseReason.Entry => new StoppedEvent(StopReason.Entry) { Description = "Paused on entry" },
                PauseReason.Exception => new StoppedEvent(StopReason.Exception) { Description = "An error occurred" },
                PauseReason.Pause => new StoppedEvent(StopReason.Pause) { Description = "Paused by user" },
                PauseReason.Step => new StoppedEvent(StopReason.Step) { Description = "Paused after step" },
                PauseReason.DebuggerStatement => new StoppedEvent(StopReason.Breakpoint) { Description = "Hit debugger statement" },
                _ => throw new NotImplementedException($"DebugAdapter reason not implemented for {reason}")
            });
        }

        private void Debugger_Resumed()
        {
            // TODO: a debug adapter is not expected to send this event in response to a request
            // that implies that execution continues, e.g. ‘launch’ or ‘continue’.
            //SendEvent(new ContinuedEvent());
        }

        protected override async Task AttachRequest(AttachArguments arguments)
        {
            // "If the ‘attach’ request was used to connect to the debuggee, ‘disconnect’ does not terminate
            // the debuggee."
            shouldTerminateOnDisconnect = false;
        }

        protected override async Task<BreakpointLocationsResponse> BreakpointLocationsRequest(BreakpointLocationsArguments arguments)
        {
            string id = host.SourceProvider.GetSourceId(arguments.Source.Path);
            
            var (start, end) = ToJintRange(arguments.Line, arguments.Column, arguments.EndLine, arguments.EndColumn);

            var info = debugger.GetScriptInfo(id);
            var positions = info.FindBreakpointPositionsInRange(start, end);

            var locations = positions.Select(p => {
                var pos = ToClientPosition(p);
                return new BreakpointLocation(pos.Line, pos.Column);
            });
            
            return new BreakpointLocationsResponse(locations);
        }

        protected override async Task CancelRequest(CancelArguments arguments)
        {
        }

        protected override async Task ConfigurationDoneRequest()
        {
            debugger.NotifyUIReady();
        }

        protected override async Task<ContinueResponse> ContinueRequest(ContinueArguments arguments)
        {
            debugger.Run();
            return new ContinueResponse();
        }

        protected override async Task DisconnectRequest(DisconnectArguments arguments)
        {
            // Terminate if:
            // - We're restarting (debug client will launch the adapter again)
            // - If the client explicitly requests termination
            // - If the client DOESN'T specify if we should terminate, but initial request (launch/attach) indicates
            //   that we should.
            if (
                arguments.Restart == true ||
                arguments.TerminateDebuggee == true ||
                (arguments.TerminateDebuggee == null && shouldTerminateOnDisconnect)
            )
            {
                restarting = false;
                debugger.Terminate();
            }
            else if (arguments.SuspendDebuggee != true)
            {
                debugger.Disconnect();
            }
        }

        protected override async Task<EvaluateResponse> EvaluateRequest(EvaluateArguments arguments)
        {
            var result = debugger.Evaluate(arguments.Expression);

            var valueInfo = variableStore.CreateValue("", result);
            return new EvaluateResponse(valueInfo.Value)
            {
                Type = valueInfo.Type,
                VariablesReference = valueInfo.VariablesReference,
                PresentationHint = valueInfo.PresentationHint,
                NamedVariables = valueInfo.NamedVariables,
                IndexedVariables = valueInfo.IndexedVariables,
                MemoryReference = valueInfo.MemoryReference
            };
        }

        protected override async Task<ExceptionInfoResponse> ExceptionInfoRequest(ExceptionInfoArguments arguments)
        {
            throw new NotImplementedException();
        }

        protected override async Task<InitializeResponse> InitializeRequest(InitializeArguments arguments)
        {
            logger.Info($"Connection established from: {arguments.ClientName} ({arguments.ClientId})");
            clientLinesStartAt1 = arguments.LinesStartAt1 ?? false;
            clientColumnsStartAt1 = arguments.ColumnsStartAt1 ?? false;

            return new InitializeResponse
            {
                SupportsConditionalBreakpoints = true,
                SupportsBreakpointLocationsRequest = true,
                SupportsConfigurationDoneRequest = true,
                SupportsTerminateRequest = true,
                SupportsDelayedStackTraceLoading = true,
                SupportsHitConditionalBreakpoints = true,
                SupportsLogPoints = true,
                SupportsSetVariable = true,
                SupportSuspendDebuggee = true,
                SupportTerminateDebuggee = true,
                SupportsRestartRequest = true
            };
        }

        protected override async Task LaunchRequest(LaunchArguments arguments)
        {
            // "If the debuggee has been started with the ‘launch’ request,
            // the ‘disconnect’ request terminates the debuggee."
            shouldTerminateOnDisconnect = true;

            await Launch(arguments);

            SendEvent(new InitializedEvent());
        }

        private async Task Launch(ConfigurationArguments arguments)
        {
            string program = arguments.AdditionalProperties["program"].GetString();
            bool pauseOnEntry = arguments.AdditionalProperties["stopOnEntry"].GetBoolean();
            // Not a fan of double negatives (i.e. testing for !noDebug), so let's convert it to debug
            bool debug = !(arguments.NoDebug ?? false);

            debugger.PauseOnEntry = pauseOnEntry;
            
            // TODO: Need to figure out threading here...
            host.Launch(program, debug, arguments.AdditionalProperties);
        }

        protected override async Task<LoadedSourcesResponse> LoadedSourcesRequest()
        {
            return new LoadedSourcesResponse(new List<Source>());
        }

        protected override async Task NextRequest(NextArguments arguments)
        {
            debugger.StepOver();
        }

        protected override async Task PauseRequest(PauseArguments arguments)
        {
            debugger.Pause();
        }

        protected override async Task RestartRequest(RestartArguments arguments)
        {
            // TODO: Restart is still totally broken - threading issues.
            restarting = true;
            debugger.Terminate();
            Launch(arguments.Arguments);
        }

        protected override async Task<ScopesResponse> ScopesRequest(ScopesArguments arguments)
        {
            var frame = currentDebugInformation.CallStack[arguments.FrameId];
            return new ScopesResponse(frame.ScopeChain.Select(s =>
                new Scope(s.ScopeType.ToString(),
                s.ScopeType == DebugScopeType.Local ?
                    variableStore.Add(s, frame) : // For local scope, we include the frame to get "this" and returnval
                    variableStore.Add(s))
            ));
        }

        protected override async Task<SetBreakpointsResponse> SetBreakpointsRequest(SetBreakpointsArguments arguments)
        {
            // TODO: What should be done if client sends breakpoints for unknown source?
            // (Happens e.g. when launching a file in VSCode while other files are open).
            string id = host.SourceProvider.GetSourceId(arguments.Source.Path);

            // SetBreakpoints expects us to clear all current breakpoints
            debugger.ClearBreakpoints();

            List<Breakpoint> results = new();
            foreach (var breakpoint in arguments.Breakpoints)
            {
                var jintPosition = ToJintPosition(breakpoint.Line, breakpoint.Column);
                var actualJintPosition = debugger.SetBreakpoint(id, jintPosition, breakpoint.Condition, breakpoint.HitCondition, breakpoint.LogMessage);
                var actualBreakpoint = new Breakpoint
                {
                    Verified = true
                };
                // If requested breakpoint position changed, send back the new position
                if (actualJintPosition != jintPosition)
                {
                    var actualLocation = ToClientPosition(actualJintPosition);
                    actualBreakpoint.Line = actualLocation.Line;
                    actualBreakpoint.Column = actualLocation.Column;
                }
                results.Add(actualBreakpoint);
            }
            return new SetBreakpointsResponse(results);
        }

        protected override async Task<SetVariableResponse> SetVariableRequest(SetVariableArguments arguments)
        {
            var value = debugger.Evaluate(arguments.Value);

            try
            {
                var newInfo = variableStore.SetValue(arguments.VariablesReference, arguments.Name, value);

                // TODO: Format
                return new SetVariableResponse(newInfo.Value)
                {
                    Type = newInfo.Type,
                    VariablesReference = newInfo.VariablesReference,
                    IndexedVariables = newInfo.IndexedVariables,
                    NamedVariables = newInfo.VariablesReference
                };
            }
            catch (VariableException ex)
            {
                throw new DebuggerException($"Cannot set variable/property {arguments.Name}: {ex.Message}");
            }
        }

        protected override async Task<StackTraceResponse> StackTraceRequest(StackTraceArguments arguments)
        {
            // TODO: StackFrameFormat handling?
            var frames = currentDebugInformation.CallStack.AsEnumerable();

            // Return subset
            if (arguments.Levels > 0)
            {
                frames = frames.Skip(arguments.StartFrame ?? 0).Take(arguments.Levels.Value);
            }

            var result = new List<StackFrame>();
            int index = 0;
            foreach (var frame in frames)
            {
                var location = ToClientSourceLocation(frame.Location);
                result.Add(new StackFrame(index, frame.FunctionName)
                {
                    Source = location.Source,
                    Line = location.Start.Line,
                    Column = location.Start.Column,
                    EndLine = location.End.Line,
                    EndColumn = location.End.Column
                });

                index++;
            }

            return new StackTraceResponse(result)
            {
                TotalFrames = currentDebugInformation.CallStack.Count
            };
        }

        protected override async Task StepInRequest(StepInArguments arguments)
        {
            // TODO: StepInTargets support
            debugger.StepInto();
        }

        protected override async Task StepOutRequest(StepOutArguments arguments)
        {
            debugger.StepOut();
        }

        protected override async Task TerminateRequest(TerminateArguments arguments)
        {
            restarting = false;
            debugger.Terminate();
        }

        protected override async Task<ThreadsResponse> ThreadsRequest()
        {
            // "Even if a debug adapter does not support multiple threads, it must implement the threads request and return a single (dummy) thread."
            return new ThreadsResponse(new List<Thread> { new Thread(1, "Main Thread") });
        }

        protected override async Task<VariablesResponse> VariablesRequest(VariablesArguments arguments)
        {
            var container = variableStore.GetContainer(arguments.VariablesReference);
            var variables = container.GetVariables(arguments.Filter, arguments.Start, arguments.Count);

            return new VariablesResponse(variables);
        }

        private SourceLocation ToClientSourceLocation(Location location)
        {
            return new SourceLocation(
                source: new Source { Path = host.SourceProvider.GetSourcePath(location.Source) },
                start: ToClientPosition(location.Start),
                end: ToClientPosition(location.End)
            );
        }

        /// <summary>
        /// Converts Jint (Esprima) position to client position.
        /// </summary>
        /// <remarks>
        /// Esprima lines start at 1 while columns start at 0. The client may be different - indeed, in VSCode, both
        /// lines and columns start at 1.
        /// </remarks>
        private Position ToClientPosition(Position position)
        {
            return new Position(
                clientLinesStartAt1 ? position.Line : position.Line - 1,
                clientColumnsStartAt1 ? position.Column + 1 : position.Column
                );
        }

        /// <summary>
        /// Converts client position to Jint position.
        /// </summary>
        /// <remarks>
        /// Esprima lines start at 1 while columns start at 0. The client may be different - indeed, in VSCode, both
        /// lines and columns start at 1.
        /// </remarks>
        private Position ToJintPosition(int line, int? column)
        {
            column ??= (clientColumnsStartAt1 ? 1 : 0);
            return new Position(
                clientLinesStartAt1 ? line : line + 1,
                clientColumnsStartAt1 ? column.Value - 1 : column.Value
                );
        }

        private (Position Start, Position End) ToJintRange(int line, int? column, int? endLine, int? endColumn)
        {
            if (column == null)
            {
                // "If no start column is given, the first column in the start line is assumed."
                column = 1;
            }
            if (endLine == null)
            {
                // "If no end line is given, then the end line is assumed to be the start line."
                endLine = line;
            }
            if (endColumn == null)
            {
                // "If no end column is given, then it is assumed to be in the last column of the end line."
                endColumn = Int32.MaxValue;
            }

            return (new Position(line, column.Value), new Position(endLine.Value, endColumn.Value));
        }
    }
}

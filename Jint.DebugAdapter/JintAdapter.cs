﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jither.DebugAdapter.Protocol.Events;
using Jither.DebugAdapter.Protocol.Requests;
using Jither.DebugAdapter.Protocol.Responses;
using Jither.DebugAdapter.Protocol.Types;
using Jint.Runtime.Debugger;
using Jither.DebugAdapter;
using Thread = Jither.DebugAdapter.Protocol.Types.Thread;
using Jither.DebugAdapter.Helpers;

namespace Jint.DebugAdapter
{
    public class JintAdapter : Adapter
    {
        private readonly Logger logger = LogManager.GetLogger();
        private readonly Debugger debugger;
        private readonly VariableStore variableStore;
        private Dictionary<string, string> sourceIdByPath = new();
        private Dictionary<string, string> pathBySourceId = new();

        private bool clientLinesStartAt1;
        private bool clientColumnsStartAt1;

        private string mainScriptId;
        private bool noDebug;
        private bool stopOnEntry;

        public JintAdapter(Debugger debugger)
        {
            this.debugger = debugger;
            debugger.Continued += Debugger_Continued;
            debugger.Stopped += Debugger_Stopped;

            variableStore = new VariableStore(debugger.Engine);
        }

        private void Debugger_Stopped(PauseReason reason, DebugInformation info)
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

        private void Debugger_Continued()
        {
            // TODO: a debug adapter is not expected to send this event in response to a request
            // that implies that execution continues, e.g. ‘launch’ or ‘continue’.
            //SendEvent(new ContinuedEvent());
        }

        protected override void AttachRequest(AttachArguments arguments)
        {
        }

        protected override BreakpointLocationsResponse BreakpointLocationsRequest(BreakpointLocationsArguments arguments)
        {
            string sourceId = GetSourceId(arguments.Source.Path);

            var info = debugger.GetScriptInfo(sourceId);
            
            var locations = info.BreakpointPositions.Select(p => {
                var pos = ToClientLocation(p.Position);
                return new BreakpointLocation(pos.Line, pos.Column);
            });
            
            return new BreakpointLocationsResponse(locations);
        }

        protected override void CancelRequest(CancelArguments arguments)
        {
        }

        protected override void ConfigurationDoneRequest()
        {
            debugger.ExecuteAsync(mainScriptId, noDebug, stopOnEntry);
        }

        protected override ContinueResponse ContinueRequest(ContinueArguments arguments)
        {
            debugger.Run();
            return new ContinueResponse();
        }

        protected override void DisconnectRequest(DisconnectArguments arguments)
        {
        }

        protected override EvaluateResponse EvaluateRequest(EvaluateArguments arguments)
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

        protected override ExceptionInfoResponse ExceptionInfoRequest(ExceptionInfoArguments arguments)
        {
            throw new NotImplementedException();
        }

        protected override InitializeResponse InitializeRequest(InitializeArguments arguments)
        {
            logger.Info($"Connection established from: {arguments.ClientName} ({arguments.ClientId})");
            clientLinesStartAt1 = arguments.LinesStartAt1 ?? false;
            clientColumnsStartAt1 = arguments.ColumnsStartAt1 ?? false;

            return new InitializeResponse
            {
                SupportsConditionalBreakpoints = true,
                SupportsBreakpointLocationsRequest = true,
                SupportsConfigurationDoneRequest = true,
                SupportsTerminateRequest = true
            };
        }

        protected override void LaunchRequest(LaunchArguments arguments)
        {
            string path = arguments.AdditionalProperties["program"].GetString();
            this.noDebug = arguments.NoDebug ?? false;
            this.stopOnEntry = arguments.AdditionalProperties["stopOnEntry"].GetBoolean();
            string sourceId = AddSource(path);
            this.mainScriptId = sourceId;
            debugger.Prepare(sourceId, path);

            SendEvent(new InitializedEvent());
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
            string id = GetSourceId(arguments.Source.Path);
            debugger.ClearBreakpoints();
            List<Breakpoint> results = new();
            foreach (var breakpoint in arguments.Breakpoints)
            {
                var jintPosition = ToJintLocation(breakpoint.Line, breakpoint.Column ?? 0);
                var actualJintPosition = debugger.SetBreakpoint(id, jintPosition, breakpoint.Condition);
                var actualBreakpoint = new Breakpoint
                {
                    Verified = true
                };
                if (actualJintPosition != jintPosition)
                {
                    var actualLocation = ToClientLocation(actualJintPosition);
                    actualBreakpoint.Line = actualLocation.Line;
                    actualBreakpoint.Column = actualLocation.Column;
                }
                results.Add(actualBreakpoint);
            }
            return new SetBreakpointsResponse(results);
        }

        protected override StackTraceResponse StackTraceRequest(StackTraceArguments arguments)
        {
            return new StackTraceResponse(debugger.CurrentDebugInformation.CallStack.Select((frame, index) => {
                var start = ToClientLocation(frame.Location.Start);
                var end = ToClientLocation(frame.Location.End);
                return new StackFrame(index, frame.FunctionName)
                {
                    Source = new Source
                    {
                        Path = GetSourcePath(frame.Location.Source)
                    },
                    Line = start.Line,
                    Column = start.Column,
                    EndLine = end.Line,
                    EndColumn = end.Column
                };
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
            debugger.Terminate();
        }

        protected override ThreadsResponse ThreadsRequest()
        {
            // "Even if a debug adapter does not support multiple threads, it must implement the threads request and return a single (dummy) thread."
            return new ThreadsResponse(new List<Thread> { new Thread(1, "Main Thread") });
        }

        protected override VariablesResponse VariablesRequest(VariablesArguments arguments)
        {
            var container = variableStore.GetContainer(arguments.VariablesReference);
            var variables = container.GetVariables();
            return new VariablesResponse(variables);
        }

        private string AddSource(string path)
        {
            string id = Guid.NewGuid().ToString();
            pathBySourceId.Add(id, path);
            path = NormalizePath(path);
            sourceIdByPath.Add(path, id);
            return id;
        }

        private string GetSourceId(string path)
        {
            path = NormalizePath(path);
            return sourceIdByPath.GetValueOrDefault(path);
        }

        private string GetSourcePath(string id)
        {
            return pathBySourceId.GetValueOrDefault(id);
        }

        private string NormalizePath(string path)
        {
            return path.ToLowerInvariant().Replace('\\', '/');
        }

        /// <summary>
        /// Converts Jint (Esprima) location to client location.
        /// </summary>
        /// <remarks>
        /// Esprima lines start at 1 while columns start at 0. The client may be different - indeed, in VSCode, both
        /// lines and columns start at 1.
        /// </remarks>
        private Esprima.Position ToClientLocation(Esprima.Position position)
        {
            return new Esprima.Position(
                clientLinesStartAt1 ? position.Line : position.Line - 1,
                clientColumnsStartAt1 ? position.Column + 1 : position.Column
                );
        }

        /// <summary>
        /// Converts client location to Jint location.
        /// </summary>
        /// <remarks>
        /// Esprima lines start at 1 while columns start at 0. The client may be different - indeed, in VSCode, both
        /// lines and columns start at 1.
        /// </remarks>
        private Esprima.Position ToJintLocation(int line, int column)
        {
            return new Esprima.Position(
                clientLinesStartAt1 ? line : line + 1,
                clientColumnsStartAt1 ? column - 1 : column
                );
        }
    }
}

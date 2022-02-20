using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Jint.DebugAdapter.Protocol;

namespace Jint.DebugAdapter
{
    public abstract class Adapter
    {
        protected ManualResetEvent waitForExit = new(false);
        private DebugProtocol protocol;
        private readonly ProtocolHandler handler;

        public CultureInfo Locale { get; private set; } = CultureInfo.GetCultureInfo("en-US");
        public bool LinesStartAt1 { get; private set; } = true;
        public bool ColumnsStartAt1 { get; private set; } = true;
        public string PathFormat { get; private set; }
        
        protected Adapter()
        {
            // TODO: Injection
            handler = new ProtocolHandler();
        }

        public void Initialize()
        {
            StartListening();
        }

        protected void InitializeStreams(Stream inputStream, Stream outputStream)
        {
            protocol = new DebugProtocol(handler, inputStream, outputStream);
        }

        public void Start()
        {
            protocol.Start();
        }

        protected abstract void StartListening();
        protected void Terminate()
        {
            Logger.Log("Terminating...");
            waitForExit.Set();
        }

        /*
        protected override InitializeResponse HandleInitializeRequest(InitializeArguments arguments)
        {
            Logger.Log($"{arguments.ClientName} connected");
            LinesStartAt1 = arguments.LinesStartAt1 ?? true;
            ColumnsStartAt1 = arguments.ColumnsStartAt1 ?? true;
            if (arguments.Locale != null)
            {
                Locale = CultureInfo.GetCultureInfo(arguments.Locale);
            }
            PathFormat = arguments.PathFormat ?? PathFormatValue.Unknown;

            return new InitializeResponse
            {
                SupportsConditionalBreakpoints = true,
                SupportsConfigurationDoneRequest = true
            };
        }

        protected override SetBreakpointsResponse HandleSetBreakpointsRequest(SetBreakpointsArguments arguments)
        {
            Logger.Log("Set breakpoints");
            Logger.Log(JsonSerializer.Serialize(arguments));

            // TODO: This is a mock
            var breakpoints = arguments.Breakpoints.Select(b => new Breakpoint(true)).ToList();

            return new SetBreakpointsResponse(breakpoints);
        }

        protected override ConfigurationDoneResponse HandleConfigurationDoneRequest(ConfigurationDoneArguments arguments)
        {
            Logger.Log("Configuration done");
            return new ConfigurationDoneResponse();
        }

        protected override LaunchResponse HandleLaunchRequest(LaunchArguments arguments)
        {
            string programPath = arguments.ConfigurationProperties.GetValueAsString("program");
            Logger.Log($"Launching {programPath}...");
            if (String.IsNullOrEmpty(programPath))
            {
                throw new ProtocolException("Property 'program' was not specified.");
            }
            if (!File.Exists(programPath))
            {
                throw new ProtocolException($"Program '{programPath}' does not exist.");
            }
            return new LaunchResponse();
        }

        protected override TerminateResponse HandleTerminateRequest(TerminateArguments arguments)
        {
            Terminate();
            return new TerminateResponse();
        }
        */
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Jither.DebugAdapter.Helpers;
using Jither.DebugAdapter.Protocol;

namespace Jither.DebugAdapter
{
    public abstract class Endpoint
    {
        protected Logger logger = LogManager.GetLogger();

        protected ManualResetEvent waitForExit = new(false);

        private DebugProtocol protocol;
        private readonly Adapter adapter;

        public CultureInfo Locale { get; private set; } = CultureInfo.GetCultureInfo("en-US");
        public bool LinesStartAt1 { get; private set; } = true;
        public bool ColumnsStartAt1 { get; private set; } = true;
        public string PathFormat { get; private set; }

        protected Endpoint(Adapter adapter)
        {
            this.adapter = adapter;
        }

        public void Initialize()
        {
            StartListening();
        }

        protected void InitializeStreams(Stream inputStream, Stream outputStream)
        {
            protocol = new DebugProtocol(adapter, inputStream, outputStream);
            adapter.Protocol = protocol;
        }

        public async Task StartAsync()
        {
            try
            {
                await protocol.StartAsync();
            }
            catch (OperationCanceledException)
            {
                logger.Info("Cancelled server task.");
            }
        }

        protected abstract void StartListening();
        protected void Terminate()
        {
            logger.Info("Terminating...");
            waitForExit.Set();
        }
    }
}

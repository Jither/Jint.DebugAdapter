using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint.Native;
using Jint.Runtime;
using Jither.DebugAdapter.Protocol.Events;
using Jither.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter
{
    // TODO: Determine script location of console calls
    internal class Console
    {
        private readonly JintAdapter adapter;

        public Console(JintAdapter adapter)
        {
            this.adapter = adapter;
        }

        public void Assert(JsValue assertion, params JsValue[] values)
        {
            if (!TypeConverter.ToBoolean(assertion))
            {
                Error(new JsValue[] { "Assertion failed:" }.Concat(values).ToArray());
            }
        }

        public void Clear()
        {
            // From vscode-js-debug source (https://github.com/microsoft/vscode-js-debug/blob/main/src/adapter/console/consoleMessage.ts)
            InternalSend(OutputCategory.Console, "\x1b[2J");
        }

        // TODO: Count(), CountReset()

        public void Debug(params JsValue[] values)
        {
            Send(OutputCategory.Stdout, values);
        }

        // TODO: Dir(), DirXml()

        public void Error(params JsValue[] values)
        {
            Send(OutputCategory.Stderr, values);
        }

        public void Group(string label)
        {
            InternalSend(OutputCategory.Stdout, label, group: OutputGroup.Start);
        }

        public void GroupCollapsed(string label)
        {
            InternalSend(OutputCategory.Stdout, label, group: OutputGroup.StartCollapsed);
        }

        public void GroupEnd()
        {
            InternalSend(OutputCategory.Stdout, String.Empty, group: OutputGroup.End);
        }

        public void Info(params JsValue[] values)
        {
            Send(OutputCategory.Stdout, values);
        }

        public void Log(params JsValue[] values)
        {
            Send(OutputCategory.Stdout, values);
        }

        // TODO: Table(), Time(), TimeEnd() TimeLog()

        public void Trace()
        {
            // TODO: Stack trace from console.trace()
        }

        public void Warn(params JsValue[] values)
        {
            Send(OutputCategory.Stderr, values);
        }

        internal void Send(OutputCategory category, JsValue[] values, SourceLocation location = null, OutputGroup group = null)
        {
            string message = String.Join(' ', values.Select(v => v?.ToString()));
            Send(category, message, location, group);
        }

        internal void Send(OutputCategory category, string message, SourceLocation location = null, OutputGroup group = null)
        {
            InternalSend(category, message + "\n", location, group);
        }

        private void InternalSend(OutputCategory category, string message, SourceLocation location = null, OutputGroup group = null)
        {
            adapter.SendEvent(new OutputEvent(message)
            {
                Category = category,
                Line = location?.Start.Line,
                Column = location?.Start.Column,
                Source = location?.Source,
                Group = group
            });
        }
    }
}

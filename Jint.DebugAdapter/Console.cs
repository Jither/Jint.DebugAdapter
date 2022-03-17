using System.Diagnostics;
using System.Globalization;
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
        private readonly Dictionary<string, long> timers = new();
        private readonly Dictionary<string, uint> counters = new();

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

        public void Count(string label = null)
        {
            label ??= "default";

            if (!counters.TryGetValue(label, out var count))
            {
                count = 0;
            }
            count++;
            counters[label] = count;
            Log($"{label}: {count}");
        }

        public void CountReset(string label = null)
        {
            label ??= "default";

            if (!counters.ContainsKey(label))
            {
                Warn($"Count for '{label}' does not exist.");
                return;
            }

            counters[label] = 0;
            Log($"{label}: 0");
        }

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

        // TODO: Table()

        public void Time(string label = null)
        {
            label ??= "default";

            timers[label] = Stopwatch.GetTimestamp();
        }

        public void TimeEnd(string label = null)
        {
            InternalTimeLog(label, end: true);
        }

        public void TimeLog(string label = null)
        {
            InternalTimeLog(label, end: false);
        }

        private void InternalTimeLog(string label, bool end)
        {
            label ??= "default";

            if (!timers.TryGetValue(label, out var started))
            {
                Warn($"Timer '{label}' does not exist.");
                return;
            }

            var elapsed = Stopwatch.GetTimestamp() - started;
            string ms = (elapsed / 10000d).ToString(CultureInfo.InvariantCulture);
            string message = $"{label}: {ms} ms";
            if (end)
            {
                message += " - timer ended.";
                timers.Remove(label);
            }
            Log(message);
        }

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

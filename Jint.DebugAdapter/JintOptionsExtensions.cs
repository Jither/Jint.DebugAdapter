using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint.Runtime.Debugger;

namespace Jint.DebugAdapter
{
    public static class JintOptionsExtensions
    {
        public static Options SetupDebugger(this Options options)
        {
            return options
                // In order to stop on entry, we need to be stepping from the start
                // The Debugger will change to StepMode.None if not stopping on entry.
                .InitialStepMode(StepMode.Into)
                .DebuggerStatementHandling(DebuggerStatementHandling.Script);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint.DebugAdapter;
using Jint.Runtime.Debugger;

namespace Jint.DebugAdapterExample
{
    public class ScriptHost
    {
        private readonly Engine engine;
        public Debugger Debugger { get; }

        public ScriptHost()
        {
            engine = new Engine(options =>
            {
                options.DebugMode(true)
                    .DebuggerStatementHandling(DebuggerStatementHandling.Script);
            });

            Debugger = new Debugger(engine);
        }

        public void Execute()
        {
            
        }
    }
}

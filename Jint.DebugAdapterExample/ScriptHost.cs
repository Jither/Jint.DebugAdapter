using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Jint.DebugAdapter;
using Jint.Runtime.Debugger;

namespace Jint.DebugAdapterExample
{
    public class ScriptHost : IScriptHost
    {
        private readonly Engine engine;
        public SourceProvider SourceProvider { get; }

        public Debugger Debugger { get; }

        public ScriptHost()
        {
            SourceProvider = new SourceProvider();

            engine = new Engine(options =>
            {
                options.DebugMode(true)
                    .DebuggerStatementHandling(DebuggerStatementHandling.Script);
            });

            Debugger = new Debugger(engine);
        }

        public void Launch(string program, bool debug, IReadOnlyDictionary<string, JsonElement> arguments)
        {
            string source = File.ReadAllText(program);
            if (debug)
            {
                Debugger.Attach();
            }
            try
            {
                string sourceId = SourceProvider.Register(program);
                engine.Execute(source, new Esprima.ParserOptions(sourceId));
            }
            finally
            {
                Debugger.Detach();
            }
        }
    }
}

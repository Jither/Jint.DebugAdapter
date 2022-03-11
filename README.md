Jint.DebugAdapter
=================
(Eventually) an implementation of the DebugAdapter protocol (used by e.g. Visual Studio Code) for Jint debugging.

Currently a hot mess. Not ready for production use (also uses functionality not yet merged into Jint).

* __Jither.DebugAdapter__ is a general purpose implementation of the DebugAdapter protocol.
* __Jint.DebugAdapter__ implements the DebugAdapter for Jint.
* __Jint.DebugAdapterExample__ is a small console app serving as an example (and testing ground).
* __VSCodeExtension__ contains a minimal extension to interface VS Code with the debug adapter.

Features
--------
Planned (❌) and currently working (✔) - but not necessarily fully completed - features:

### Execution and script access
- ✔ Launch (run script in current tab of client)
- ❌ Attach to running script
- ❌ Attach to debuggee process and launch script
- ❌ Debug scripts accessible to client and debuggee via file system path (debugger client loads source from disk)
- ❌ Debug scripts only accessible to debuggee (debuggee provides source to debugger client)
- ❌ Dynamic loading of scripts (imported modules)

### Breakpoints
- ✔ Column breakpoints
  - ✔ Statements
  - ✔ Loop expressions (tests, for loop updates, left side in `for-in`/`for-of`)
  - ✔ Function return
  - ✔ Skips block statements
- ✔ Conditional breakpoints
- ✔ Break on `debugger` statement
- ❌ Hit count breakpoints
- ❌ Logpoints (output log message rather than breaking)
- ❌ Break on exceptions (requires Jint exception overhaul)

### Actions
- ✔ Step over
- ✔ Step into
- ✔ Step out
- ✔ Step points are the same as the possible breakpoint positions listed above
- ✔ Continue/Pause
- ✔ Terminate
- ❌ Disconnect (but keep debuggee running/detach)
- ❌ Restart

### Variables
- ✔ Full scopes
- ✔ Display variables in selected call stack frame
- ✔ Lazy evaluation of property getters
- ✔ Lazy evaluation of structured types
- ✔ Display `this` (when defined) and `Return value` (at function return points)
- ❌ Summary of structured types (before expanding/lazy evaluation)
- ❌ Display type on hover
- ❌ Hit count breakpoints
- ❌ Modify/set variables

### Watch
- ✔ Evaluation

### Call stack
- ✔ Full call stack
- ✔ Current "instruction pointer" (position) in all stack frames

### Debug console
- ✔ Evaluation and "REPL"
- ❌ Console output (e.g. `console.info`, `console.error`...)

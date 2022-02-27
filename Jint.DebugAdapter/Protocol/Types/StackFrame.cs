using System.Text.Json.Serialization;
using Jint.DebugAdapter.Helpers;

namespace Jint.DebugAdapter.Protocol.Types
{
    /// <summary>
    /// A Stackframe contains the source location.
    /// </summary>
    public class StackFrame
    {
        /// <param name="id">An identifier for the stack frame. It must be unique across all threads.</param>
        /// <param name="name">The name of the stack frame, typically a method name.</param>
        [JsonConstructor]
        public StackFrame(int id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// An identifier for the stack frame. It must be unique across all threads.
        /// </summary>
        /// <remarks>
        /// This id can be used to retrieve the scopes of the frame with the 'scopesRequest' or to restart the 
        /// execution of a stackframe.
        /// </remarks>
        public int Id { get; set; }

        /// <summary>
        /// The name of the stack frame, typically a method name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The optional source of the frame.
        /// </summary>
        public Source Source { get; set; }

        /// <summary>
        /// The line within the file of the frame. If source is null or doesn't exist, line is 0 and must be ignored.
        /// </summary>
        public int Line { get; set; }

        /// <summary>
        /// The column within the line. If source is null or doesn't exist, column is 0 and must be ignored.
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// An optional end line of the range covered by the stack frame.
        /// </summary>
        public int? EndLine { get; set; }

        /// <summary>
        /// An optional end column of the range covered by the stack frame.
        /// </summary>
        public int? EndColumn { get; set; }

        /// <summary>
        /// Indicates whether this frame can be restarted with the 'restart' request.
        /// </summary>
        /// <remarks>
        /// Clients should only use this if the debug adapter supports the 'restart' request
        /// (capability 'supportsRestartRequest' is true).
        /// </remarks>
        public bool? CanRestart { get; set; }

        /// <summary>
        /// Optional memory reference for the current instruction pointer in this frame.
        /// </summary>
        public string InstructionPointerReference { get; set; }

        /// <summary>
        /// The module associated with this frame, if any.
        /// </summary>
        public string ModuleId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public StringEnum<StackFramePresentationHint>? PresentationHint { get; set; }
    }
}

using System.Text.Json.Serialization;
using Jint.DebugAdapter.Helpers;

namespace Jint.DebugAdapter.Protocol.Types
{
    /// <summary>
    /// A Scope is a named container for variables. Optionally a scope can map to a source or a range within a source.
    /// </summary>
    public class Scope
    {
        /// <param name="name">Name of the scope</param>
        /// <param name="variablesReference">The variables of this scope can be retrieved by passing the value of
        /// variablesReference to the VariablesRequest.</param>
        [JsonConstructor]
        public Scope(string name, int variablesReference)
        {
            Name = name;
            VariablesReference = variablesReference;
        }

        /// <summary>
        /// Name of the scope such as 'Arguments', 'Locals', or 'Registers'. This string is shown in the UI as is and 
        /// can be translated.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// An optional hint for how to present this scope in the UI. If this attribute is missing, the scope is
        /// shown with a generic UI.
        /// </summary>
        public ScopePresentationHint PresentationHint { get; set; }

        /// <summary>
        /// The variables of this scope can be retrieved by passing the value of variablesReference
        /// to the VariablesRequest.
        /// </summary>
        public int VariablesReference { get; set; }

        /// <summary>
        /// The number of named variables in this scope. The client can use this optional information to present the
        /// variables in a paged UI and fetch them in chunks.
        /// </summary>
        public int? NamedVariables { get; set; }

        /// <summary>
        /// The number of indexed variables in this scope. The client can use this optional information to present
        /// the variables in a paged UI and fetch them in chunks.
        /// </summary>
        public int? IndexedVariables { get; set; }

        /// <summary>
        /// If true, the number of variables in this scope is large or expensive to retrieve.
        /// </summary>
        public bool Expensive { get; set; }

        /// <summary>
        /// Optional source for this scope.
        /// </summary>
        public Source Source { get; set; }

        /// <summary>
        /// Optional start line of the range covered by this scope.
        /// </summary>
        public int? Line { get; set; }

        /// <summary>
        /// Optional start column of the range covered by this scope.
        /// </summary>
        public int? Column { get; set; }

        /// <summary>
        /// Optional end line of the range covered by this scope.
        /// </summary>
        public int? EndLine { get; set; }

        /// <summary>
        /// Optional end column of the range covered by this scope.
        /// </summary>
        public int? EndColumn { get; set; }
    }
}

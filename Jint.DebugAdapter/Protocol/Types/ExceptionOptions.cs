using Jint.DebugAdapter.Helpers;

namespace Jint.DebugAdapter.Protocol.Types
{
    /// <summary>
    /// An ExceptionOptions assigns configuration options to a set of exceptions.
    /// </summary>
    public class ExceptionOptions
    {
        /// <param name="breakMode">Condition when a thrown exception should result in a break.</param>
        public ExceptionOptions(StringEnum<ExceptionBreakMode> breakMode)
        {
            BreakMode = breakMode;
        }

        /// <summary>
        /// A path that selects a single or multiple exceptions in a tree. If 'path' is missing, the whole tree
        /// is selected. By convention the first segment of the path is a category that is used to group exceptions
        /// in the UI.
        /// </summary>
        public List<ExceptionPathSegment> Path { get; set; }

        /// <summary>
        /// Condition when a thrown exception should result in a break.
        /// </summary>
        public StringEnum<ExceptionBreakMode> BreakMode { get; set; }
    }
}

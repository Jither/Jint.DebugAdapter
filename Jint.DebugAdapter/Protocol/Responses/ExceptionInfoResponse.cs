using Jint.DebugAdapter.Helpers;
using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    /// <summary>
    /// Response to ‘exceptionInfo’ request.
    /// </summary>
    public class ExceptionInfoResponse : ProtocolResponseBody
    {
        /// <param name="exceptionId">ID of the exception that was thrown.</param>
        /// <param name="breakMode">Mode that caused the exception notification to be raised.</param>
        public ExceptionInfoResponse(string exceptionId, StringEnum<ExceptionBreakMode> breakMode)
        {
            ExceptionId = exceptionId;
            BreakMode = breakMode;
        }

        /// <summary>
        /// ID of the exception that was thrown.
        /// </summary>
        public string ExceptionId { get; set; }

        /// <summary>
        /// Descriptive text for the exception provided by the debug adapter.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Mode that caused the exception notification to be raised.
        /// </summary>
        public StringEnum<ExceptionBreakMode> BreakMode { get; set; }

        /// <summary>
        /// Detailed information about the exception.
        /// </summary>
        public ExceptionDetails ExceptionDetails { get; set; }
    }
}

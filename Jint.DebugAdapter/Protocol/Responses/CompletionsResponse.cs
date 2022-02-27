using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    /// <summary>
    /// Response to ‘completions’ request.
    /// </summary>
    public class CompletionsResponse : ProtocolResponseBody
    {
        /// <param name="targets">The possible completions</param>
        public CompletionsResponse(List<CompletionItem> targets)
        {
            Targets = targets;
        }

        /// <summary>
        /// The possible completions
        /// </summary>
        public List<CompletionItem> Targets { get; set; }
    }
}

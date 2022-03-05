using Jither.DebugAdapter.Protocol.Types;

namespace Jither.DebugAdapter.Protocol.Responses
{
    /// <summary>
    /// Response to ‘completions’ request.
    /// </summary>
    public class CompletionsResponse : ProtocolResponseBody
    {
        /// <param name="targets">The possible completions</param>
        public CompletionsResponse(IEnumerable<CompletionItem> targets)
        {
            Targets = targets;
        }

        /// <summary>
        /// The possible completions
        /// </summary>
        public IEnumerable<CompletionItem> Targets { get; set; }
    }
}

namespace Jint.DebugAdapter.Protocol.Requests
{
    /// <summary>
    /// Returns a list of possible completions for a given caret position and text.
    /// </summary>
    /// <remarks>
    /// Clients should only call this request if the capability ‘supportsCompletionsRequest’ is true.
    /// </remarks>
    public class CompletionsArguments : ProtocolArguments
    {
        /// <summary>
        /// Returns completions in the scope of this stack frame. If not specified, the completions are returned
        /// for the global scope.
        /// </summary>
        public int? FrameId { get; set; }

        /// <summary>
        /// One or more source lines. Typically this is the text a user has typed into the debug console before
        /// he asked for completion.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The character position for which to determine the completion proposals.
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// An optional line for which to determine the completion proposals. If missing,
        /// the first line of the text is assumed.
        /// </summary>
        public int? Line { get; set; }
    }
}

using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    /// <summary>
    /// The request returns the variable scopes for a given stackframe ID.
    /// </summary>
    public class ScopesResponse : ProtocolResponseBody
    {
        /// <param name="scopes">The scopes of the stackframe. If the array has length zero,
        /// there are no scopes available.</param>
        public ScopesResponse(IEnumerable<Scope> scopes)
        {
            Scopes = scopes;
        }

        /// <summary>
        /// The scopes of the stackframe. If the array has length zero, there are no scopes available.
        /// </summary>
        public IEnumerable<Scope> Scopes { get; set; }
    }
}

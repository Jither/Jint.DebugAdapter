namespace Jint.DebugAdapter.Protocol.Types
{
    public enum ScopePresentationHint
    {
        Other,

        /// <summary>
        /// Scope contains method arguments.
        /// </summary>
        Arguments,

        /// <summary>
        /// Scope contains local variables.
        /// </summary>
        Locals,

        /// <summary>
        /// Scope contains registers. Only a single 'registers' scope should be returned from a 'scopes' request.
        /// </summary>
        Registers
    }
}

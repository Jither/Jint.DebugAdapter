namespace Jint.DebugAdapter.Protocol.Types
{
    public enum VariableKind
    {
        Other,

        /// <summary>
        /// Indicates that the object is a property.
        /// </summary>
        Property,

        /// <summary>
        /// Indicates that the object is a method.
        /// </summary>
        Method,

        /// <summary>
        /// Indicates that the object is a class.
        /// </summary>
        Class,

        /// <summary>
        /// Indicates that the object is data.
        /// </summary>
        Data,

        /// <summary>
        /// Indicates that the object is an event.
        /// </summary>
        Event,

        /// <summary>
        /// Indicates that the object is a base class.
        /// </summary>
        BaseClass,

        /// <summary>
        /// Indicates that the object is an inner class.
        /// </summary>
        InnerClass,

        /// <summary>
        /// Indicates that the object is an interface.
        /// </summary>
        Interface,

        /// <summary>
        /// Indicates that the object is the most derived class.
        /// </summary>
        MostDerivedClass,

        /// <summary>
        /// Indicates that the object is virtual, that means it is a synthetic object introduced by the
        /// adapter for rendering purposes, e.g. an index range for large arrays.
        /// </summary>
        Virtual,

        /// <summary>
        /// Indicates that a data breakpoint is registered for the object. The 'hasDataBreakpoint' attribute should
        /// generally be used instead.
        /// </summary>
        [Obsolete("Deprecated. The 'hasDataBreakpoint' attribute should generally be used instead.")]
        DataBreakpoint
    }
}

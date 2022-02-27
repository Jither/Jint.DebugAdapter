using Jint.DebugAdapter.Helpers;

namespace Jint.DebugAdapter.Protocol.Types
{
    /// <completionlist cref="VariableKind"/>
    public class VariableKind : StringEnum<VariableKind>
    {
        /// <summary>
        /// Indicates that the object is a property.
        /// </summary>
        public static readonly VariableKind Property = Create("property");

        /// <summary>
        /// Indicates that the object is a method.
        /// </summary>
        public static readonly VariableKind Method = Create("method");

        /// <summary>
        /// Indicates that the object is a class.
        /// </summary>
        public static readonly VariableKind Class = Create("class");

        /// <summary>
        /// Indicates that the object is data.
        /// </summary>
        public static readonly VariableKind Data = Create("data");

        /// <summary>
        /// Indicates that the object is an event.
        /// </summary>
        public static readonly VariableKind Event = Create("event");

        /// <summary>
        /// Indicates that the object is a base class.
        /// </summary>
        public static readonly VariableKind BaseClass = Create("baseClass");

        /// <summary>
        /// Indicates that the object is an inner class.
        /// </summary>
        public static readonly VariableKind InnerClass = Create("innerClass");

        /// <summary>
        /// Indicates that the object is an interface.
        /// </summary>
        public static readonly VariableKind Interface = Create("interface");

        /// <summary>
        /// Indicates that the object is the most derived class.
        /// </summary>
        public static readonly VariableKind MostDerivedClass = Create("mostDerivedClass");

        /// <summary>
        /// Indicates that the object is virtual, that means it is a synthetic object introduced by the
        /// adapter for rendering purposes, e.g. an index range for large arrays.
        /// </summary>
        public static readonly VariableKind Virtual = Create("virtual");

        /// <summary>
        /// Indicates that a data breakpoint is registered for the object. The 'hasDataBreakpoint' attribute should
        /// generally be used instead.
        /// </summary>
        [Obsolete("Deprecated. The 'hasDataBreakpoint' attribute should generally be used instead.")]
        public static readonly VariableKind DataBreakpoint = Create("dataBreakpoint");
    }
}

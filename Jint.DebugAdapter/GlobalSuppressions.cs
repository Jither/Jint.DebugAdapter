// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "May be good for performance, not necessarily good for API")]
[assembly: SuppressMessage("Style", "IDE0063:Use simple 'using' statement", Justification = "Not a fan of simple using statements - hide scope")]

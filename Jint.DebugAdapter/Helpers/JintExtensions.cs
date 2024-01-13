using Jint.Native;
using Jint.Native.Object;

namespace Jint.DebugAdapter.Helpers;

/// <summary>
/// Extensions for Jint types.
/// </summary>
public static class JintExtensions
{
    private static readonly JsString lengthPropertyName = new("length");

    /// <summary>
    /// Returns "length" property of an object instance as an integer - 0 if object has no length property or it isn't a number.
    /// </summary>
    public static int GetLengthValue(this ObjectInstance obj)
    {
        var lengthProp = obj.Get(lengthPropertyName);
        if (!lengthProp.IsNumber())
        {
            return 0;
        }
        return (int)lengthProp.AsNumber();
    }
}

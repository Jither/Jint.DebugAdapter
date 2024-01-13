using Jint.Native.Object;
using Jint.Native;
using Jint.Runtime;
using Jint.Runtime.Descriptors;
using Jint.Native.TypedArray;
using Jint.Native.Argument;
using Jint.DebugAdapter.Helpers;

namespace Jint.DebugAdapter.Variables;

public class ArrayLikeVariableContainer : ObjectVariableContainer
{
    public ArrayLikeVariableContainer(VariableStore store, int id, ObjectInstance instance) : base(store, id, instance)
    {
    }

    public override JsValue SetVariable(string name, JsValue value)
    {
        var prop = instance.GetOwnProperty(name);
        if (prop.Writable)
        {
            prop.Value = value;
            return value;
        }

        if (prop.Set != null)
        {
            instance.Engine.Invoke(prop.Set, value);
            return instance.Engine.Invoke(prop.Get);
        }

        throw new VariableException($"Property is read only.");
    }

    protected override IEnumerable<JintVariable> GetAllVariables(int? start, int? count)
    {
        var result = GetNamedVariables(null, 0).Concat(GetIndexedVariables(null, 0));
        // Return subset
        // TODO: Does this ever happen? Unfiltered variables being paged?
        if (count > 0)
        {
            result = result.Skip(start ?? 0).Take(count.Value);
        }
        return result;
    }

    protected override IEnumerable<JintVariable> GetIndexedVariables(int? start, int? count)
    {
        var items = instance switch
        {
            JsTypedArray => GetTypedArrayIndexValues(start, count),
            JsArguments => GetArgumentsArrayIndexValues(start, count),
            _ => GetArrayIndexValues(start, count)
        };

        return items.Select(i => CreateVariable(i.Key, i.Value));
    }

    private IEnumerable<KeyValuePair<string, JsValue>> GetArrayIndexValues(int? start, int? count)
    {
        // Yes, JS supports array length up to 2^32-1, but DAP only supports up to 2^31-1
        int length = (int)instance.GetLengthValue();
        if (count > 0)
        {
            length = Math.Min(length, count.Value);
        }

        // We can assume that array indices are the first Length properties returned by GetOwnProperties
        // https://tc39.es/ecma262/#sec-ordinaryownpropertykeys
        var items = instance.GetOwnProperties();
        if (start > 0)
        {
            items = items.Skip(start.Value);
        }
        return items.Take(length).Select(kv => KeyValuePair.Create(kv.Key.ToString(), kv.Value.Value));
    }

    private IEnumerable<KeyValuePair<string, JsValue>> GetArgumentsArrayIndexValues(int? start, int? count)
    {
        // Unlike Array instances, we CAN'T assume order of properties on Arguments.
        // So, we check if each property key is an array index.
        var result = instance.GetOwnProperties().Where(p => IsArrayIndex(p.Key));
        if (count > 0)
        {
            result = result.Skip(start ?? 0).Take(count.Value);
        }
        return result.Select(kv => KeyValuePair.Create(kv.Key.ToString(), kv.Value.Value));
    }

    private IEnumerable<KeyValuePair<string, JsValue>> GetTypedArrayIndexValues(int? start, int? count)
    {
        var arr = instance as JsTypedArray;
        
        int length = (int)arr.Length;
        if (count > 0)
        {
            length = Math.Min(length, count.Value);
        }

        var list = new List<KeyValuePair<string, JsValue>>();
        for (int i = start ?? 0; i < length; i++)
        {
            list.Add(KeyValuePair.Create(i.ToString(), arr[i]));
        }
        return list;
    }

    private IEnumerable<KeyValuePair<JsValue, PropertyDescriptor>> GetArrayProperties()
    {
        // Yes, JS supports array length up to 2^32-1, but DAP only supports up to 2^31-1
        int length = (int)instance.GetLengthValue();

        // We can assume that array indices are the first Length properties returned by GetOwnProperties
        // https://tc39.es/ecma262/#sec-ordinaryownpropertykeys
        return instance.GetOwnProperties().Skip(length);
    }

    private IEnumerable<KeyValuePair<JsValue, PropertyDescriptor>> GetTypedArrayProperties()
    {
        // TypedArray does not include array indices in own properties
        return instance.GetOwnProperties();
    }

    private IEnumerable<KeyValuePair<JsValue, PropertyDescriptor>> GetArgumentsProperties()
    {
        // Unlike Array instances, we CAN'T assume the order of properties on Arguments.
        // So, we're checking if each property is a valid array index.
        return instance.GetOwnProperties().Where(p => !IsArrayIndex(p.Key));
    }

    protected override IEnumerable<JintVariable> GetNamedVariables(int? start, int? count)
    {
        var props = instance switch
        {
            JsTypedArray => GetTypedArrayProperties(),
            JsArguments => GetArgumentsProperties(),
            _ => GetArrayProperties()
        };

        props = props.Concat(GetPrototypeProperties());

        if (count > 0)
        {
            props = props.Skip(start ?? 0).Take(count.Value);
        }

        return AddPrototypeIfExists(props.Select(p => CreateVariable(p.Key.ToString(), p.Value, instance)));
    }

    private static bool IsArrayIndex(JsValue value)
    {
        if (value.IsNumber())
        {
            var numValue = value.AsNumber();
            uint intValue = (uint)numValue;
            return numValue == intValue && intValue != UInt32.MaxValue;
        }

        // TODO: Handle numeric string
        return false;
    }
}

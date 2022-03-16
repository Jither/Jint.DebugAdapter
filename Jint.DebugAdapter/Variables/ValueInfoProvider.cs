using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Jint.Native;
using Jint.Native.Argument;
using Jint.Native.Array;
using Jint.Native.Date;
using Jint.Native.Function;
using Jint.Native.Object;
using Jint.Native.RegExp;
using Jint.Native.TypedArray;
using Jint.Runtime.Descriptors;
using Jither.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Variables
{
    public class ValueInfoProvider
    {
        private static readonly JsonSerializerOptions stringToJsonOptions = new()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        private readonly VariableStore store;

        public ValueInfoProvider(VariableStore store)
        {
            this.store = store;
        }

        /// <summary>
        /// Creates ValueInfo for a given JsValue.
        /// </summary>
        public ValueInfo Create(string name, JsValue value)
        {
            var result = new ValueInfo(name)
            {
                Value = RenderValue(name, value),
                Type = RenderType(value)
            };

            ObjectInstance obj;

            switch (value)
            {
                case ArgumentsInstance:
                case ArrayInstance:
                case TypedArrayInstance:
                    obj = value as ObjectInstance;
                    // Yes, JS supports array length up to 2^32-1, but DAP only supports up to 2^31-1
                    int length = (int)obj.Length;

                    if (length > 100)
                    {
                        result.IndexedVariables = length;
                        // If we specify number of indexed variables, we also need to specify number of named variables
                        // Judging from the VSCode JS debug adapter, we can just specify 1 (to get the client to query us
                        // when needed), rather than precounting the properties
                        result.NamedVariables = 1;
                    }

                    result.VariablesReference = store.AddArrayLike(obj);
                    break;
                case ObjectInstance:
                    result.VariablesReference = store.Add(value as ObjectInstance);
                    break;
            }

            return result;
        }

        /// <summary>
        /// Creates ValueInfo for a given property.
        /// </summary>
        public ValueInfo Create(string name, PropertyDescriptor prop, ObjectInstance owner)
        {
            if (prop.Get != null)
            {
                return new ValueInfo(name)
                {
                    Value = "(...)",
                    Type = RenderType(prop.Get),
                    PresentationHint = new VariablePresentationHint { Lazy = true },
                    // Add a variable reference for lazy evaluation of the getter
                    VariablesReference = store.Add(prop, owner),
                };
            }
            else
            {
                return Create(name, prop.Value);
            }
        }

        /// <summary>
        /// Returns the string representation of a JsValue (displayed in the Variables panel in the debugger)
        /// </summary>
        public string RenderValue(string name, JsValue value)
        {
            return value switch
            {
                null => "null",

                JsNull or
                JsNumber or
                JsBigInt or
                JsBoolean or
                JsUndefined or
                JsSymbol => value.ToString(),
                // For DAP, strings need to be returned with surrounding quotes - and with control characters
                // escaped - but otherwise with minimal escaping for readability.
                JsString => JsonSerializer.Serialize(value.ToString(), stringToJsonOptions),

                // TODO: Array preview
                ArgumentsInstance arr => $"({arr.Length}) []",
                ArrayInstance arr => $"({arr.Length}) []",
                TypedArrayInstance arr => $"{GetObjectType(arr)}({arr.Length}) []",

                FunctionInstance func => $"ƒ {GetFunctionName(func) ?? name}",

                DateInstance or
                RegExpInstance => value.ToString(),
                // TODO: Object preview
                ObjectInstance => "{...}", 
                _ => value.ToString()
            };
        }

        /// <summary>
        /// Returns the string representation of JsValue's type (displayed on hover in the Variables panel in the
        /// debugger). The string representation is intended to be more specific than JsValue's Type property -
        /// e.g. returning the constructor of Object instances.
        /// </summary>
        public string RenderType(JsValue value)
        {
            return value switch
            {
                // If value is (CLR) null, it means the variable is uninitialized (let/const)
                null => "Not initialized",
                JsString or
                JsNumber or
                JsBigInt or
                JsBoolean or
                JsUndefined or
                JsSymbol or
                JsNull => value.Type.ToString(),

                FunctionInstance => "Function",
                ObjectInstance obj => GetObjectType(obj),
                _ => "Unknown type"
            };
        }

        // Intended as possible future extension point
        protected string GetObjectType(ObjectInstance obj)
        {
            var constructor = obj.Get("constructor");
            if (constructor is ObjectConstructor)
            {
                // Special case - ObjectConstructor has the name "delegate"
                return "Object";
            }
            return constructor?.Get("name")?.ToString() ?? "Object";
        }

        
        // Intended as possible future extension point
        protected string GetFunctionName(FunctionInstance func)
        {
            if (func is ObjectConstructor)
            {
                // Special case - ObjectConstructor has the name "delegate"
                return "Object";
            }
            var name = func.GetOwnProperty("name").Value;
            if (!name.IsUndefined())
            {
                return name.ToString();
            }
            return null;
        }
    }
}

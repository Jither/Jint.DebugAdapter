using System.Text.Encodings.Web;
using System.Text.Json;
using Jint.Native;
using Jint.Native.Argument;
using Jint.Native.Array;
using Jint.Native.Function;
using Jint.Native.Object;
using Jint.Native.RegExp;
using Jint.Native.TypedArray;
using Jint.Runtime.Debugger;
using Jint.Runtime.Descriptors;
using Jither.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter
{
    public abstract class ValueInfo
    {
        protected static readonly JsonSerializerOptions stringToJsonOptions = new()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public string Name { get; }
        public string Value { get; protected set; }
        public string Type { get; protected set; }
        public int VariablesReference { get; set; }
        public VariablePresentationHint PresentationHint { get; set; }
        public int? NamedVariables { get; set; }
        public int? IndexedVariables { get; set; }
        public string MemoryReference { get; set; }
        public string EvaluateName { get; set; }

        protected ValueInfo(string name)
        {
            Name = name;
        }

        protected static string GetObjectType(ObjectInstance obj)
        {
            return obj.Get("constructor")?.Get("name")?.ToString() ?? "Object";
        }
    }

    public class NullValueInfo : ValueInfo
    {
        public NullValueInfo(string name) : base(name)
        {
            Value = "null";
            Type = "null";
        }
    }

    public class PrimitiveValueInfo : ValueInfo
    {
        public PrimitiveValueInfo(string name, JsValue value) : base(name)
        {
            Value = value switch
            {
                JsString => JsonSerializer.Serialize(value.ToString(), stringToJsonOptions),
                _ => value.ToString()
            };
            Type = value.Type.ToString();
        }
    }

    public class ArrayValueInfo : ValueInfo
    {
        public ArrayValueInfo(string name, ObjectInstance value) : base(name)
        {
            // Yes, JS supports array length up to 2^32-1, but DAP only supports up to 2^31-1
            int length = (int)value.Length;
            Type = GetObjectType(value);

            Value = $"({length}) []";

            if (length > 100)
            {
                this.IndexedVariables = (int)length;
                // If we specify number of indexed variables, we also need to specify number of named variables
                // Judging from the VSCode JS debug adapter, we can just specify 1 (to get the client to query us
                // when needed), rather than precounting the properties
                this.NamedVariables = 1;
            }
        }
    }

    public class FunctionValueInfo : ValueInfo
    {
        public FunctionValueInfo(string name, FunctionInstance function) : base(name)
        {
            Value = $"ƒ {name}";
            Type = "Function";
        }
    }

    public class ObjectValueInfo : ValueInfo
    {
        public ObjectValueInfo(string name, ObjectInstance value) : base(name)
        {
            Value = "{...}";
            Type = GetObjectType(value);
        }
    }

    public class GetterValueInfo : ValueInfo
    {
        public GetterValueInfo(string name) : base(name)
        {
            Value = "(...)";
            PresentationHint = new VariablePresentationHint { Lazy = true };
        }
    }
    

    public class VariableStore
    {
        private readonly Engine engine;
        private int nextId = 1;
        private readonly Dictionary<int, VariableContainer> containers = new();

        public VariableStore(Engine engine)
        {
            this.engine = engine;
        }

        public int Add(DebugScope scope, CallFrame frame = null)
        {
            var container = new ScopeVariableContainer(this, nextId++, scope, frame);
            return Add(container);
        }

        public int Add(ObjectInstance instance)
        {
            var container = new ObjectVariableContainer(this, nextId++, instance);
            return Add(container);
        }

        public int AddArrayLike(ObjectInstance instance)
        {
            var container = new ArrayLikeVariableContainer(this, nextId++, instance);
            return Add(container);
        }

        public int Add(PropertyDescriptor prop, ObjectInstance owner)
        {
            var container = new PropertyVariableContainer(this, nextId++, prop, owner, engine);
            return Add(container);
        }

        private int Add(VariableContainer container)
        {
            containers.Add(container.Id, container);
            return container.Id;
        }

        public VariableContainer GetContainer(int id)
        {
            return containers[id];
        }

        public ValueInfo CreateValue(string name, JsValue value)
        {
            return value switch
            {
                null => new NullValueInfo(name),
                JsNull or
                JsString or
                JsNumber or
                JsBigInt or
                JsBoolean or
                JsUndefined or
                JsSymbol or
                JsNull => new PrimitiveValueInfo(name, value),
                ArgumentsInstance arr => new ArrayValueInfo(name, arr) { VariablesReference = AddArrayLike(arr) },
                ArrayInstance arr => new ArrayValueInfo(name, arr) { VariablesReference = AddArrayLike(arr) },
                TypedArrayInstance arr => new ArrayValueInfo(name, arr) { VariablesReference = AddArrayLike(arr) },
                FunctionInstance func => new FunctionValueInfo(name, func),
                ObjectInstance obj => new ObjectValueInfo(name, obj) { VariablesReference = Add(obj) },
                _ => throw new NotImplementedException($"Unimplemented JsValue type: {value.GetType()}")
            };
        }

        public ValueInfo CreateValue(string name, PropertyDescriptor prop, ObjectInstance owner)
        {
            if (prop.Get != null)
            {
                return new GetterValueInfo(name)
                {
                    // Add a variable reference for lazy evaluation of the getter
                    VariablesReference = Add(prop, owner),
                };
            }
            else
            {
                return CreateValue(name, prop.Value);
            }
        }

        public void Clear()
        {
            containers.Clear();
        }
    }
}

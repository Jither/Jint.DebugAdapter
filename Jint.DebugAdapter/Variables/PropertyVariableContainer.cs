﻿using Jint.Runtime.Descriptors;
using Jint.Native.Object;
using Jint.Native;

namespace Jint.DebugAdapter.Variables
{
    public class PropertyVariableContainer : VariableContainer
    {
        private readonly PropertyDescriptor prop;
        private readonly ObjectInstance owner;

        public PropertyVariableContainer(VariableStore store, int id, PropertyDescriptor prop, ObjectInstance owner) : base(store, id)
        {
            this.prop = prop;
            this.owner = owner;
        }

        public override JsValue SetVariable(string name, JsValue value)
        {
            // TODO: Is this right?
            throw new VariableException($"Cannot modify property value {name}");
        }

        protected override IEnumerable<JintVariable> GetAllVariables(int? start, int? count)
        {
            return new[] { CreateVariable(string.Empty, owner.Engine.Invoke(prop.Get, owner, Array.Empty<object>())) };
        }
    }
}

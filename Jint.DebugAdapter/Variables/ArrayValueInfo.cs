using Jint.Native.Object;

namespace Jint.DebugAdapter.Variables
{
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
                IndexedVariables = length;
                // If we specify number of indexed variables, we also need to specify number of named variables
                // Judging from the VSCode JS debug adapter, we can just specify 1 (to get the client to query us
                // when needed), rather than precounting the properties
                NamedVariables = 1;
            }
        }
    }
}

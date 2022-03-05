using System.Text.Json.Serialization;

namespace Jither.DebugAdapter.Protocol.Types
{
    /// <summary>
    /// A Module object represents a row in the modules view.
    /// </summary>
    /// <remarks>
    /// Two attributes are mandatory: an id identifies a module in the modules view and is used in a ModuleEvent for
    /// identifying a module for adding, updating or deleting.
    /// 
    /// The name is used to minimally render the module in the UI.
    /// 
    /// Additional attributes can be added to the module.They will show up in the module View if they have a
    /// corresponding ColumnDescriptor.
    /// 
    /// To avoid an unnecessary proliferation of additional attributes with similar semantics but different names
    /// we recommend to re-use attributes from the ‘recommended’ list below first, and only introduce new attributes 
    /// if nothing appropriate could be found.
    /// </remarks>
    public class Module
    {
        /// <param name="id">Unique identifier for the module.</param>
        /// <param name="name">A name of the module.</param>
        [JsonConstructor]
        public Module(string id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// Unique identifier for the module.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// A name of the module.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Logical full path to the module. The exact definition is implementation defined, but usually this would
        /// be a full path to the on-disk file for the module.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// True if the module is optimized.
        /// </summary>
        public bool? IsOptimized { get; set; }

        /// <summary>
        /// True if the module is considered 'user code' by a debugger that supports 'Just My Code'.
        /// </summary>
        public bool? IsUserCode { get; set; }

        /// <summary>
        /// Version of Module.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// User understandable description of if symbols were found for the module (ex: 'Symbols Loaded', 
        /// 'Symbols not found', etc.)
        /// </summary>
        public string SymbolStatus { get; set; }

        /// <summary>
        /// Logical full path to the symbol file. The exact definition is implementation defined.
        /// </summary>
        public string SymbolFilePath { get; set; }

        /// <summary>
        /// Module created or modified.
        /// </summary>
        public string DateTimeStamp { get; set; }

        /// <summary>
        /// Address range covered by this module.
        /// </summary>
        public string AddressRange { get; set; }
    }
}

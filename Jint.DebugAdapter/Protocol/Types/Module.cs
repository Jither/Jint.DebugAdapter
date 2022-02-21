namespace Jint.DebugAdapter.Protocol.Types
{
    public class Module
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public bool? IsOptimized { get; set; }
        public bool? IsUserCode { get; set; }
        public string Version { get; set; }
        public string SymbolStatus { get; set; }
        public string SymbolFilePath { get; set; }
        public string DateTimeStamp { get; set; }
        public string AddressRange { get; set; }
    }
}

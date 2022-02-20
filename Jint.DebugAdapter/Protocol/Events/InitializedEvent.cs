namespace Jint.DebugAdapter.Protocol.Events
{
    internal class InitializedEvent : ProtocolEventBody
    {
        protected override string EventNameInternal => "initialized";
    }
}

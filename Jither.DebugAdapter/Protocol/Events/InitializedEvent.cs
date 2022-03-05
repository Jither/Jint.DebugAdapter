namespace Jither.DebugAdapter.Protocol.Events
{
    public class InitializedEvent : ProtocolEventBody
    {
        protected override string EventNameInternal => "initialized";
    }
}

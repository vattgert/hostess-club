namespace Session
{
    public interface ISessionModifier
    {
        void OnSessionStart(HostAndCustomerSession session);
        void OnSessionUpdate(HostAndCustomerSession session, float timeElapsed);
        void OnSessionEnd(HostAndCustomerSession session);
    }
}


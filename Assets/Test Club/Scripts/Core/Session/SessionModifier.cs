using UnityEngine;

namespace Session
{
    public abstract class SessionModifier : ScriptableObject, ISessionModifier
    {
        public string modifierName;
        public abstract void OnSessionStart(HostAndCustomerSession session);
        public abstract void OnSessionUpdate(HostAndCustomerSession session, float timeElapsed);
        public abstract void OnSessionEnd(HostAndCustomerSession session);
    }
}
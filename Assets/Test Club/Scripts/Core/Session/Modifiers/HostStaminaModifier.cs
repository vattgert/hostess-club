using UnityEngine;
using Characters;

namespace Session
{
    public class HostStaminaModifier : ISessionModifier
    {
        private readonly int BASE_STAMINA_LOSS = 15;
        private readonly float ABSENT_COMPATIBILITY_LOSS_MULTIPLIER = 1.5f;
        private readonly float FULL_COMPATIBILITY_LOSS_MULTIPLIER = 0.75f;

        public void OnSessionStart(HostAndCustomerSession session)
        { }

        public void OnSessionUpdate(HostAndCustomerSession session, float timeElapsed)
        { }

        public void OnSessionEnd(HostAndCustomerSession session)
        {
            float staminaLoss = BASE_STAMINA_LOSS;
            switch (session.SessionSettings.Compatibility)
            {
                case Compatibility.Absent:
                    staminaLoss *= ABSENT_COMPATIBILITY_LOSS_MULTIPLIER; // e.g. 22.5
                    break;
                case Compatibility.Full:
                    staminaLoss *= FULL_COMPATIBILITY_LOSS_MULTIPLIER; // e.g. 11.25
                    break;
            }
            Host host = session.Host().GetComponent<HostBehavior>().Host;
            host.DecreaseStamina(staminaLoss);
            Debug.Log("Host on session end after stamina decrease: \n" + host);
        }
    }
}


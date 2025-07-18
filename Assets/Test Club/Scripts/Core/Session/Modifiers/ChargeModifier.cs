using UnityEngine;
using Characters;

namespace Session
{
    public class ChargeModifier : ISessionModifier
    {
        private float chargeTimer = 0f;

        public void OnSessionStart(HostAndCustomerSession session)
        {}

        public void OnSessionUpdate(HostAndCustomerSession session, float timeElapsed)
        {
            chargeTimer += timeElapsed;
            if (chargeTimer >= session.SessionSettings.ChargeInterval)
            {
                chargeTimer = 0;
                //// Charge
                CustomerBehavior cb = session.Customer().GetComponent<CustomerBehavior>();
                Host host = session.Host().GetComponent<HostBehavior>().Host;

                if (session.SessionBilling.NextChargeOverflow())
                {
                    Debug.Log($"Session ended due to customer balance overflow. \n{host.Name} finished {timeElapsed} seconds with the client. \nCustomer balance: ${cb.Customer.Budget}");
                    session.FinishSession();
                }
                else
                {
                    session.SessionBilling.Bill();
                }
            }
        }

        public void OnSessionEnd(HostAndCustomerSession session)
        {
        }
    }
}


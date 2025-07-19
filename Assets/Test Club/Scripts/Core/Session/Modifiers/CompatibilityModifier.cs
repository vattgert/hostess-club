using UnityEngine;
using Characters;

namespace Session
{
    class CompatibilityChargeModifier : IChargeModifier
    {
        private int compatibilityModifier;

        public CompatibilityChargeModifier(int modifier)
        {
            compatibilityModifier = modifier;
        }

        public (int Charged, int Earned) Modify((int Charged, int Earned) billing)
        {
            // If its a compatibility fine - host charges customer for less money
            if (compatibilityModifier < 0)
            {
                int hostBillingFine = billing.Charged + Mathf.FloorToInt(billing.Charged * compatibilityModifier / 100);
                return (hostBillingFine, hostBillingFine);
            }
            // If its a compatibility bonus - host charges customer usual amount but earns virtual bonus above
            else if (compatibilityModifier > 0)
            {
                int virtualHostBonus = billing.Earned + Mathf.FloorToInt(billing.Earned * compatibilityModifier / 100);
                return (billing.Charged, virtualHostBonus);
            }
            // If modifier is 0 - then just do nothing
            return billing;
        }
    }

    public class CompatibilityModifier : ISessionModifier
    {
        private readonly int FULL_COMPATIBILITY_BONUS = 20;
        private readonly int NON_COMPATIBILITY_FINE = -10;
        public float ModifierDuration { get; private set; } = 10f;

        private TemporalSessionModifier temporalSessionModifier;
        private CompatibilityChargeModifier chargeModifier;

        private CompatibilityChargeModifier ChargeModifier(Host host, Customer customer)
        {
            Debug.Log(host);
            Debug.Log(customer);
            bool compatible = host.Personality == customer.PersonalityPreference && host.Appearance == customer.AppearancePreference;
            bool partiallyCompatible = host.Personality == customer.PersonalityPreference || host.Appearance == customer.AppearancePreference;
            if (compatible)
            {
                Debug.Log($"Host {host.Name} has full compatibility with the customer");
                return new CompatibilityChargeModifier(FULL_COMPATIBILITY_BONUS);
            } else if(partiallyCompatible)
            {
                Debug.Log($"Host {host.Name} has partial compatibility with the customer");
                return default;
            }
            Debug.Log($"Host {host.Name} has no compatibility with the customer");
            return new CompatibilityChargeModifier(NON_COMPATIBILITY_FINE);
        }

        public void OnSessionStart(HostAndCustomerSession session)
        {
            temporalSessionModifier = new TemporalSessionModifier(ModifierDuration);
            Host host = session.Host().GetComponent<HostBehavior>().Host;
            Customer customer = session.Customer().GetComponent<CustomerBehavior>().Customer;
            CompatibilityChargeModifier modifier = ChargeModifier(host, customer);
            if (modifier != null)
            {
                chargeModifier = modifier;
                session.SessionBilling.AddModifier(modifier);
                temporalSessionModifier.Start();
            }
        }

        public void OnSessionUpdate(HostAndCustomerSession session, float timeElapsed)
        {
            temporalSessionModifier.Update(timeElapsed);
            if (!temporalSessionModifier.Active)
            {
                session.SessionBilling.RemoveModifier(chargeModifier);
                chargeModifier = null;
                temporalSessionModifier.End();
            }
        }

        public void OnSessionEnd(HostAndCustomerSession session)
        {
            session.SessionBilling.RemoveModifier(chargeModifier);
            temporalSessionModifier.End();
        }

    }
}


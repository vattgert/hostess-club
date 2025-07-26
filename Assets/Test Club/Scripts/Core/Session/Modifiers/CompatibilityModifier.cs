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

        public BillingResult Modify(BillingResult billing)
        {
            // If its a compatibility fine - host charges customer for less money
            if (compatibilityModifier < 0)
            {
                int hostBillingFine = billing.Charged + Mathf.FloorToInt(billing.Charged * compatibilityModifier / 100);
                return new BillingResult(hostBillingFine, hostBillingFine);
            }
            // If its a compatibility bonus - host charges customer usual amount but earns virtual bonus above
            else if (compatibilityModifier > 0)
            {
                int virtualHostBonus = billing.Earned + Mathf.FloorToInt(billing.Earned * compatibilityModifier / 100);
                return new BillingResult(billing.Charged, virtualHostBonus);
            }
            // If modifier is 0 - then just do nothing
            return billing;
        }
    }

    public class CompatibilityModifier : ISessionModifier
    {
        private readonly int FULL_COMPATIBILITY_BONUS = 20;
        private readonly int ABSENT_COMPATIBILITY_FINE = -10;
        public float ModifierDuration { get; private set; } = 10f;

        private TemporalSessionModifier temporalSessionModifier;
        private CompatibilityChargeModifier chargeModifier;

        private CompatibilityChargeModifier CalculateChargeModification(Compatibility compatibility)
        {
            if (compatibility == Compatibility.Full)
            {
                return new CompatibilityChargeModifier(FULL_COMPATIBILITY_BONUS);
            } else if(compatibility == Compatibility.Partial)
            {
                return default;
            }
            return new CompatibilityChargeModifier(ABSENT_COMPATIBILITY_FINE);
        }

        public void OnSessionStart(HostAndCustomerSession session)
        {
            temporalSessionModifier = new TemporalSessionModifier(ModifierDuration);
            Host host = session.Host().GetComponent<HostBehavior>().Host;
            Customer customer = session.Customer().GetComponent<CustomerBehavior>().Customer;
            Compatibility compatibility = CompatibilityCalcualtor.Calculate(host, customer);
            Debug.Log($"Host has {compatibility} compatibility with the customer");
            session.SessionSettings.SetCompatibility(compatibility);
            CompatibilityChargeModifier modifier = CalculateChargeModification(compatibility);
            if (modifier != null)
            {
                chargeModifier = modifier;
                session.SessionEvents.TriggerReaction(session.Customer(), compatibility);
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


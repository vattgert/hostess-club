using System.Collections.Generic;
using UnityEngine;
using Characters;

namespace Session
{
    public enum BillingModifierType {
        Bonus,
        Fine
    }

    public struct BillingModifier
    {
        public BillingModifierType Type { get; private set; }
        public int Amount { get; private set; }
    }

    public interface IChargeModifier
    {
        (int Charged, int Earned) Modify((int Charged, int Earned) billing);
    }

    public class SessionBilling
    {
        private Host host;
        private Customer customer;
        private ShiftData shiftData;
        private List<IChargeModifier> modifiers = new();

        public SessionBilling(Host host, Customer customer, ShiftData shiftData)
        {
            this.host = host;
            this.customer = customer;
            this.shiftData = shiftData;
        }

        //TODO: I do not like the idea that CalculateChargeAndEarning is virtually called in 2 functions which are called in a while loop
        private (int Charged, int Earned) CalculateChargeAndEarning()
        {
            int charged = host.ChargePerHour;
            int earned = charged;
            foreach (var modifier in modifiers)
            {
                Debug.Log("Modifier " + modifier.GetType().ToString() + " is active.");
                (int Charged, int Earned) modified = modifier.Modify((charged, earned));
                charged = modified.Charged;
                earned = modified.Earned;
                Debug.Log(modifier.GetType().ToString() + " modifier application: \n" + "Customer charged: " + charged + "\n" + "Host earned: " + earned);
            }

            //int charged = Mathf.Max(1, Mathf.FloorToInt(charged));
            return (Charged: charged, Earned: earned);
        }

        public bool NextChargeOverflow()
        {
            (int Charged, int Earned) preview = CalculateChargeAndEarning();
            return (customer.Budget - preview.Charged) <= 0;
        }

        public (int Charged, int Earned) Bill()
        {
            (int Charged, int Earned) preview = CalculateChargeAndEarning();
            customer.Budget = customer.Budget - preview.Charged;
            shiftData.AddEarning(host.Name, preview.Earned);
            Debug.Log($"Customer was charged ${preview.Charged}. \n{host.Name} earned {preview.Earned}. \nCustomer balance: ${customer.Budget}");
            return preview;
        }

        public void AddModifier(IChargeModifier modifier) => modifiers.Add(modifier);

        public void RemoveModifier(IChargeModifier modifier) => modifiers.Remove(modifier);
    }
}
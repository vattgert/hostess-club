using System.Collections.Generic;
using UnityEngine;
using Characters;

namespace Session
{
    public struct BillingResult
    {
        public BillingResult(int charged, int earned)
        {
            Charged = charged;
            Earned = earned;
        }

        public int Charged { get; }
        public int Earned { get; }
    }

    public interface IChargeModifier
    {
        BillingResult Modify(BillingResult billing);
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
        private BillingResult CalculateBilling()
        {
            BillingResult billingResult = new BillingResult(host.ChargePerHour, host.ChargePerHour);
            foreach (var modifier in modifiers)
            {
                Debug.Log("Modifier " + modifier.GetType().ToString() + " is active.");
                BillingResult modified = modifier.Modify(billingResult);
                billingResult = modified;
                Debug.Log(modifier.GetType().ToString() + " modifier application: \n" + "Customer charged: " + billingResult.Charged + "\n" + "Host earned: " + billingResult.Earned);
            }

            //int charged = Mathf.Max(1, Mathf.FloorToInt(charged));
            return billingResult;
        }

        public bool NextChargeOverflow()
        {
            BillingResult preview = CalculateBilling();
            return (customer.Budget - preview.Charged) <= 0;
        }

        public BillingResult Bill()
        {
            BillingResult result = CalculateBilling();
            customer.Budget = customer.Budget - result.Charged;
            shiftData.AddEarning(host.Name, result.Earned);
            Debug.Log($"Customer was charged ${result.Charged}. \n{host.Name} earned {result.Earned}. \nCustomer balance: ${customer.Budget}");
            return result;
        }

        public void AddModifier(IChargeModifier modifier) => modifiers.Add(modifier);

        public void RemoveModifier(IChargeModifier modifier) => modifiers.Remove(modifier);
    }
}
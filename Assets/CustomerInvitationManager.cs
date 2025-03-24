using System;
using UnityEngine;

public class CustomerInvitationManager : MonoBehaviour
{
    private int inviteInterval = 4;
    private float nextInviteTime = 0;
    private bool tablesWereFull = false;

    private ShiftTimer shiftTimer;
    private TablesManager tablesManager;
    private CustomerManager customerManager;

    public event Action<GameObject> OnCustomerInvited;

    private void Awake()
    {
        shiftTimer = gameObject.GetComponent<ShiftTimer>();
        tablesManager = gameObject.GetComponent<TablesManager>();
        customerManager = gameObject.GetComponent<CustomerManager>();
        shiftTimer.OnShiftTimerUpdate += ManageCustomerInvitation;
        float firstCustomerInviteTime = shiftTimer.ShitDuration() - inviteInterval;
        nextInviteTime = firstCustomerInviteTime;
    }

    private void InviteCustomer()
    {
        GameObject customer = customerManager.GetCustomers().Pop();
        CustomerBehavior customerBehaviour = customer.GetComponent<CustomerBehavior>();
        customerBehaviour.CreateCustomerSprite();
        OnCustomerInvited?.Invoke(customer);
    }

    private void ResetInviteTimer(float timeLeft)
    {
        nextInviteTime = timeLeft - inviteInterval;
        tablesWereFull = false;
    }

    private void ManageCustomerInvitation(float timeLeft)
    {
        if (tablesManager.HasFreeTables())
        {
            // If tables are full - update next invite time, since when one of tables will be free
            // the next customer must not be invited immediately, but only after 'inviteInterval' seconds
            if (tablesWereFull)
            {
                ResetInviteTimer(timeLeft);
            }
            bool hasCustomers = customerManager.GetCustomers().Count > 0;
            bool timeToInvite = timeLeft <= nextInviteTime;
            if (timeToInvite && hasCustomers)
            {
                InviteCustomer();
                nextInviteTime -= inviteInterval;
            }
        } else
        {
            tablesWereFull = true;
        }
    }
}

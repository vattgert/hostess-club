using System;
using UnityEngine;

public class CustomerInvitationManager : MonoBehaviour
{
    private int inviteInterval = 6;
    private float nextInviteTime = 0;
    bool invitingPaused = false;

    private ShiftTimer shiftTimer;
    private TablesManager tablesManager;
    private CustomerManager customerManager;
    private HostManager hostManager;

    public event Action<GameObject> OnCustomerInvited;

    private void Awake()
    {
        shiftTimer = gameObject.GetComponent<ShiftTimer>();
        tablesManager = gameObject.GetComponent<TablesManager>();
        customerManager = gameObject.GetComponent<CustomerManager>();
        hostManager = gameObject.GetComponent<HostManager>();
        shiftTimer.OnShiftTimerUpdate += ManageCustomerInvitation;
        float firstCustomerInviteTime = shiftTimer.ShitDuration() - inviteInterval;
        nextInviteTime = firstCustomerInviteTime;
    }

    private void InviteCustomer()
    {
        GameObject customer = customerManager.GetCustomers().Pop();
        CustomerBehavior customerBehaviour = customer.GetComponent<CustomerBehavior>();
        customerBehaviour.CreateCustomer();
        OnCustomerInvited?.Invoke(customer);
    }

    private void ResetInviteTimer(float timeLeft)
    {
        nextInviteTime = timeLeft - inviteInterval;
        invitingPaused = false;
    }

    private void ManageCustomerInvitation(float timeLeft)
    {
        if (tablesManager.HasFreeTables() && hostManager.HasAvailableHost())
        {
            // If tables are full - update next invite time, since when one of tables will be free
            // the next customer must not be invited immediately, but only after 'inviteInterval' seconds
            if (invitingPaused)
            {
                ResetInviteTimer(timeLeft);
            }
            bool hasCustomers = customerManager.GetCustomers().Count > 0;
            bool hasAvailableHost = hostManager.GetShiftHosts().Count > 0;
            bool timeToInvite = timeLeft <= nextInviteTime;
            if (timeToInvite && hasCustomers && hasAvailableHost)
            {
                InviteCustomer();
                nextInviteTime -= inviteInterval;
            }
        } else
        {
            invitingPaused = true;
        }
    }
}

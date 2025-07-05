using System;
using System.Linq;
using UnityEngine;

class InvitationCooldown
{
    private float lastUnbusyTime = -1f;
    private readonly float cooldownDuration;

    public InvitationCooldown(float cooldownDuration)
    {
        this.cooldownDuration = cooldownDuration;
    }

    public void Reset()
    {
        lastUnbusyTime = -1f;
    }

    public void NotifyBusy()
    {
        lastUnbusyTime = -1f;
    }

    public void NotifyUnbusy(float currentTimeLeft)
    {
        if (lastUnbusyTime < 0f)
        {
            lastUnbusyTime = currentTimeLeft;
        }
    }

    public bool IsCooldownOver(float currentTimeLeft)
    {
        if (lastUnbusyTime < 0f)
        {
            return false;
        }

        return (lastUnbusyTime - currentTimeLeft) >= cooldownDuration;
    }
}

public class CustomerInvitationManager : MonoBehaviour
{
    [SerializeField]
    private ShiftManager shiftManager;
    [SerializeField]
    private ShiftTimer shiftTimer;
    [SerializeField]
    private TablesManager tablesManager;
    [SerializeField]
    private CustomersSpawner customersSpawner;
    [SerializeField]
    private HostManager hostManager;
    [SerializeField]
    private SpriteRenderer entrance;


    [SerializeField] 
    private float invitationCooldown = 4f;
    private InvitationCooldown inviteCooldown;

    public event Action<CustomerBehavior> OnCustomerInvited;

    private void Awake()
    {
        inviteCooldown = new InvitationCooldown(invitationCooldown);
        shiftTimer.OnShiftTimerUpdate += ManageCustomerInvitation;
    }

    private void SpawnCustomerNearEntrance(GameObject customer)
    {
        Bounds doorBounds = entrance.bounds;
        float xCenter = doorBounds.center.x;
        float yBelow = doorBounds.min.y - 0.7f; // 2 units *below* the door

        Vector3 spawnPosition = new Vector3(xCenter, yBelow, 0f);
        customer.transform.position = spawnPosition;
    }

    private void InviteCustomer()
    {
        GameObject customer = customersSpawner.GetCustomers().Pop();
        customer.SetActive(true);
        SpawnCustomerNearEntrance(customer);
        CustomerBehavior cb = customer.GetComponent<CustomerBehavior>();
        cb.SetState(CustomerState.Entering);
        OnCustomerInvited?.Invoke(cb);
    }

    private bool BusyToInvite()
    {
        return shiftManager.ActiveCustomers().Any(c =>
            c.CurrentState == CustomerState.Entering ||
            c.CurrentState == CustomerState.AssignedMovingToTable
        );
    }

    private void ManageCustomerInvitation(float timeLeft)
    {
        bool customersToSpawn = customersSpawner.GetCustomers().Count > 0;
        bool availableSeatAndHost = tablesManager.HasFreeTables() && hostManager.HasAvailableHost();
        bool busyToInvite = BusyToInvite();

        if (busyToInvite)
        {
            inviteCooldown.NotifyBusy();
            return;
        }

        inviteCooldown.NotifyUnbusy(timeLeft);

        if (customersToSpawn && availableSeatAndHost && inviteCooldown.IsCooldownOver(timeLeft))
        {
            InviteCustomer();
            inviteCooldown.Reset();
        }
    }
}

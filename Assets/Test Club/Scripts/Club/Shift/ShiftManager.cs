using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ShiftManager : MonoBehaviour
{
    [SerializeField]
    private bool shiftActive;
    private List<CustomerBehavior> customers = new List<CustomerBehavior>();

    private ShiftData shiftData;
    [SerializeField]
    private ShiftTimer shiftTimer;
    [SerializeField]
    private CustomersSpawner customersSpawner;
    [SerializeField]
    private HostManager hostManager;
    [SerializeField]
    private TablesManager tablesManager;
    [SerializeField]
    private CustomerInvitationManager customerInvitationManager;

    public event Action<List<GameObject>> OnShiftStarted;
    public event Action<ShiftData> OnShiftFinished;

    private void Awake()
    {
        shiftActive = false;
        shiftData = new ShiftData();
        shiftTimer.OnShiftTimerComplete += EndShift;
        customerInvitationManager.OnCustomerInvited += AddCustomerToList;
    }

    private void AddCustomerToList(CustomerBehavior customer)
    {
        customers.Add(customer);
    }

    private void StartShiftForHosts()
    {
        foreach (HostAndCustomerSession host in tablesManager.GetSessions())
        {
            host.SetShiftActive(true);
        }
    }

    public List<CustomerBehavior> ActiveCustomers()
    {
        return customers;
    }

    public ShiftData GetShiftData()
    {
        return shiftData;
    }

    public void ChangeShiftState()
    {
        if(!shiftActive)
        {
            StartShift();
        } else
        {
            EndShift();
        }
    }

    public bool ShiftActive()
    {
        return shiftActive;
    }

    void StartShift()
    {
        Debug.Log("Shift started");
        shiftActive = true;
        shiftData.Clear();
        customersSpawner.GenerateCustomersPoolForShift();
        hostManager.GenerateShiftHosts();
        shiftTimer.StartTimer();
        StartShiftForHosts();
        OnShiftStarted?.Invoke(hostManager.GetShiftHosts());
    }

    void EndShift()
    {
        shiftActive = false;
        shiftTimer.StopTimer();
        tablesManager.ClearSessions();
        customersSpawner.ClearCustomers();
        hostManager.ClearHosts();
        customers.Clear();
        OnShiftFinished?.Invoke(shiftData);
        Debug.Log("Shift ended");
    }
}

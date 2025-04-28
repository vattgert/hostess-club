using System;
using System.Collections.Generic;
using UnityEngine;

public class ShiftManager : MonoBehaviour
{
    [SerializeField]
    private bool shiftActive;
    private ShiftData shiftData;

    private ShiftTimer shiftTimer;
    private CustomerManager customerManager;
    private HostManager hostManager;
    private TablesManager tablesManager;

    public event Action<List<GameObject>> OnShiftStarted;
    public event Action<ShiftData> OnShiftFinished;

    private void Awake()
    {
        shiftActive = false;
        shiftData = new ShiftData();
        shiftTimer = gameObject.GetComponent<ShiftTimer>();
        customerManager = gameObject.GetComponent<CustomerManager>();
        hostManager = gameObject.GetComponent<HostManager>();
        tablesManager = gameObject.GetComponent<TablesManager>();
        shiftTimer.OnShiftTimerComplete += EndShift;
    }

    private void StartShiftForHosts()
    {
        foreach (HostAndCustomerSession host in tablesManager.GetSessions())
        {
            host.SetShiftActive(true);
        }
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
        customerManager.GenerateCustomersPoolForShift();
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
        customerManager.ClearCustomers();
        hostManager.ClearHosts();
        OnShiftFinished?.Invoke(shiftData);
        Debug.Log("Shift ended");
    }
}

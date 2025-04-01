using GamesCore.ObservableSubjects;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ShiftManager : MonoBehaviour
{
    [SerializeField]
    private RootObservableContext rootContext;
    private ObservableSubject<bool> shiftActive = new ObservableSubject<bool>();
    private ShiftData shiftData;

    private ShiftTimer shiftTimer;
    private CustomerManager customerManager;
    private HostManager hostManager;
    private HostAndCustomerSession[] hostsSessionsOnTables;

    public event Action<List<GameObject>> OnShiftStarted;
    public event Action<ShiftData> OnShiftFinished;

    private void Awake()
    {
        shiftData = new ShiftData();
        shiftTimer = gameObject.GetComponent<ShiftTimer>();
        customerManager = gameObject.GetComponent<CustomerManager>();
        hostManager = gameObject.GetComponent<HostManager>();
        hostsSessionsOnTables = FindObjectsByType<HostAndCustomerSession>(FindObjectsSortMode.None);
        shiftActive.SetValue(false);
        shiftActive.Subscribe(rootContext, ChangeShiftState);
        shiftTimer.OnShiftTimerComplete += EndShift;
    }

    private void StartShiftForHosts()
    {
        foreach (HostAndCustomerSession host in hostsSessionsOnTables)
        {
            host.SetShiftActive(true);
        }
    }

    private void EndShiftForHosts()
    {
        foreach (HostAndCustomerSession host in hostsSessionsOnTables)
        {
            host.SetShiftActive(false);
        }
    }

    public ShiftData GetShiftData()
    {
        return shiftData;
    }

    public void ChangeShiftState(bool shiftActive)
    {
        if(shiftActive)
        {
            StartShift();
        } else
        {
            EndShift();
        }
    }

    void StartShift()
    {
        Debug.Log("Shift started");
        shiftData.Clear();
        customerManager.GenerateCustomersPoolForShift();
        hostManager.GenerateShiftHosts();
        shiftTimer.StartTimer();
        StartShiftForHosts();
        OnShiftStarted?.Invoke(hostManager.GetShiftHosts());
    }

    void EndShift()
    {
        shiftTimer.StopTimer();
        customerManager.ClearShiftCustomers();
        EndShiftForHosts();
        OnShiftFinished?.Invoke(shiftData);
        Debug.Log("Shift ended");
    }
}

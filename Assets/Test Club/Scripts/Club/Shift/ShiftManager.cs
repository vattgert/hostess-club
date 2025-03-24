using GamesCore.ObservableSubjects;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftManager : MonoBehaviour
{
    [SerializeField]
    private RootObservableContext rootContext;
    private ObservableSubject<bool> shiftActive = new ObservableSubject<bool>();
    private ShiftTimer shiftTimer;
    private CustomerManager customerManager;
    private HostManager hostManager;
    private ShiftHostsUI shiftHostsUi;
    private HostAndCustomerSession[] hostsSessionsOnTables;

    private Stack<GameObject> customers;

    void Start()
    {
        shiftTimer = gameObject.GetComponent<ShiftTimer>();
        customerManager = gameObject.GetComponent<CustomerManager>();
        hostManager = gameObject.GetComponent<HostManager>();
        shiftHostsUi = FindFirstObjectByType<ShiftHostsUI>();
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
        customerManager.GenerateCustomersPoolForShift();
        shiftTimer.StartTimer();
        hostManager.GenerateShiftHosts();
        StartShiftForHosts();
        shiftHostsUi.SetHostsForShiftList(hostManager.GetShiftHosts());
    }

    void EndShift()
    {
        shiftTimer.StopTimer();
        customerManager.ClearShiftCustomers();
        EndShiftForHosts();
        Debug.Log("Shift ended");
    }
}

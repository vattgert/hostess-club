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
        this.shiftTimer = gameObject.GetComponent<ShiftTimer>();
        this.customerManager = gameObject.GetComponent<CustomerManager>();
        this.hostManager = gameObject.GetComponent<HostManager>();
        this.shiftHostsUi = FindFirstObjectByType<ShiftHostsUI>();
        this.hostsSessionsOnTables = FindObjectsByType<HostAndCustomerSession>(FindObjectsSortMode.None);
        this.shiftActive.SetValue(false);
        this.shiftActive.Subscribe(this.rootContext, this.ChangeShiftState);
        shiftTimer.OnShiftTimerComplete += EndShift;
    }

    private void StartShiftForHosts()
    {
        foreach (HostAndCustomerSession host in this.hostsSessionsOnTables)
        {
            host.SetShiftActive(true);
        }
    }

    private void EndShiftForHosts()
    {
        foreach (HostAndCustomerSession host in this.hostsSessionsOnTables)
        {
            host.SetShiftActive(false);
        }
    }

    public void ChangeShiftState(bool shiftActive)
    {
        if(shiftActive)
        {
            this.StartShift();
        } else
        {
            this.EndShift();
        }
    }

    void StartShift()
    {
        Debug.Log("Shift started");
        this.shiftTimer.StartTimer();
        this.StartShiftForHosts();
        this.customerManager.GenerateCustomersPoolForShift();
        this.hostManager.GenerateHostsForShift();
        this.shiftHostsUi.SetHostsForShiftList(this.hostManager.GetHosts());
    }

    void EndShift()
    {
        this.shiftTimer.StopTimer();
        this.customerManager.ClearShiftCustomers();
        this.EndShiftForHosts();
        Debug.Log("Shift ended");
    }
}

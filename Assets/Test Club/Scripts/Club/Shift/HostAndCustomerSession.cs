using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class HostAndCustomerSession: MonoBehaviour
{
    private bool shiftActive;
    private bool assignedToCustomer;

    private ClubManager clubManager;

    private GameObject table;

    private Coroutine serviceCoroutine;

    private GameObject assignedCustomer;
    private Transform customerPlace;
    private CustomerBehavior customerBehaviour;
    private Customer customer;

    private GameObject assignedHost;
    private HostBehavior hostBehaviour;
    private Transform hostPlace;
    private Host host;

    private void Awake()
    {
        this.table = gameObject;
        this.clubManager = ClubManager.GetInstance();
        this.customerPlace = gameObject.transform.Find("Customer Place");
        this.hostPlace = gameObject.transform.Find("Host Place");
    }

    public bool TableEmpty()
    {
        return this.assignedCustomer == null && this.assignedHost == null;
    }


    public bool TableWaitsForHost()
    {
        return this.assignedCustomer != null && this.assignedHost == null;
    }
    /// <summary>
    /// Start charging money if not already charging.
    /// </summary>
    /// 
    /// <summary>
    /// Called by the ShiftManager when the shift starts or ends globally.
    /// </summary>
    public void SetShiftActive(bool active)
    {
        this.shiftActive = active;

        // If the shift just ended while we're servicing, stop
        if (!this.shiftActive && this.serviceCoroutine != null)
        {
            StopServiceRoutine();
        }
        // If the shift became active and we're assigned, we can start serving if needed
        else if (this.shiftActive && this.assignedCustomer && this.assignedHost && this.serviceCoroutine == null)
        {
            StartServiceRoutine();
        }
    }

    /// <summary>
    /// Called by some logic that assigns a client to this hostess.
    /// </summary>
    public void AssignCustomer(GameObject customer)
    {
        if (customer == null) return;

        this.assignedCustomer = customer;
        this.customerBehaviour = this.assignedCustomer.GetComponent<CustomerBehavior>();
        this.customer = customerBehaviour.GetCustomer();
        this.assignedCustomer.transform.SetParent(this.table.transform);
        this.PositionCustomer(this.assignedCustomer);
    }

    private void PositionCustomer(GameObject customer)
    {
        customer.transform.position = this.customerPlace.position;
    }

    /// <summary>
    /// Called by some logic that unassigns a client from this hostess.
    /// </summary>
    public void UnassignCustomer()
    {
        assignedCustomer = null;
        this.assignedToCustomer = false;

        // Stop charging if the hostess was in the middle of servicing
        if (serviceCoroutine != null)
        {
            StopServiceRoutine();
        }
    }

    public void AssignHost(GameObject host)
    {
        if (host == null) return;
        if (this.assignedCustomer == null)
        {
            Debug.LogError("Host cannot be assigned: there is no customer behind this table");
        }

        this.assignedHost = host;
        this.hostBehaviour = assignedHost.GetComponent<HostBehavior>();
        this.host = hostBehaviour.GetHost();
        this.assignedToCustomer = true;
        this.assignedHost.transform.SetParent(this.table.transform);
        this.PositionHost(this.assignedHost);

        // If the shift is active, begin servicing immediately
        if (this.shiftActive && this.serviceCoroutine == null)
        {
            StartServiceRoutine();
        }
    }

    private void PositionHost(GameObject host)
    {
        host.transform.localPosition = this.hostPlace.position;
    }

    /// <summary>
    /// Unassignes hostess from the table
    /// </summary>
    public void UnassignHost()
    {
        this.assignedHost = null;

        // Here I must run waiting timer for customer
    }

    private void StartServiceRoutine()
    {
        serviceCoroutine = StartCoroutine(ServeCustomerRoutine());
    }

    private void StopServiceRoutine()
    {
        StopCoroutine(serviceCoroutine);
        serviceCoroutine = null;
    }

    /// <summary>
    /// This coroutine does two things:
    /// 1) Waits for M seconds at a time, charging money each interval.
    /// 2) Continues up to a total of N seconds, or until the hostess is unassigned or the shift ends.
    /// </summary>
    private IEnumerator ServeCustomerRoutine()
    {
        float timeElapsed = 0f;

        while (this.shiftActive && this.assignedToCustomer && this.assignedHost && timeElapsed < this.host.TimeWithCustomer)
        {
            // Wait M seconds
            yield return new WaitForSeconds(this.host.ChargeInterval);
            timeElapsed += this.host.ChargeInterval;

            // Check if we became unassigned or the shift ended during the wait
            if (!this.shiftActive || !this.assignedToCustomer)
            {
                // End early if conditions are no longer met
                yield break;
            }

            // Charge
            if (clubManager != null)
            {
                clubManager.AddIncome(this.host.ChargePerHour);
                Debug.Log($"{name} charged {this.host.ChargePerHour}. Current balance: {clubManager.GetCurrentBalance()}");
            }
        }

        // If we exit because we've hit totalServiceTime, we can automatically unassign the hostess if desired.
        if (timeElapsed >= this.host.TimeWithCustomer)
        {
            Debug.Log($"{name} finished {this.host.TimeWithCustomer} seconds with the client.");
            UnassignCustomer();
        }

        this.serviceCoroutine = null;
    }
}

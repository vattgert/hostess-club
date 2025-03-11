using System;
using System.Collections;
using UnityEngine;

public class HostAndCustomerSession: MonoBehaviour
{
    private bool shiftActive;

    private ClubManager clubManager;

    private Coroutine serviceCoroutine;

    private GameObject assignedCustomer;
    private Transform customerPlace;

    private GameObject assignedHost;
    private Transform hostPlace;

    public event Action<GameObject> OnSessionFinished;

    private void Awake()
    {
        clubManager = ClubManager.GetInstance();
        customerPlace = gameObject.transform.Find(ComponentsNames.CustomerPlaceOnTable);
        hostPlace = gameObject.transform.Find(ComponentsNames.HostPlaceOnTable);
    }

    public bool TableEmpty()
    {
        return assignedCustomer == null && assignedHost == null;
    }


    public bool TableWaitsForHost()
    {
        return assignedCustomer != null && assignedHost == null;
    }

    private bool HostAssigned()
    {
        return assignedHost != null;
    }

    private bool CustomerAssigned()
    {
        return assignedHost != null;
    }

    private bool SessionContinues()
    {
        return shiftActive && HostAssigned() && CustomerAssigned();
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
        shiftActive = active;
    }

    /// <summary>
    /// Called by some logic that assigns a client to this hostess.
    /// </summary>
    public void AssignCustomer(GameObject customer)
    {
        if (customer == null) return;

        assignedCustomer = customer;
        PositionCustomer(assignedCustomer);
    }

    private void PositionCustomer(GameObject customer)
    {
        customer.transform.SetParent(customerPlace.transform);
        customer.transform.position = customerPlace.position;
    }

    /// <summary>
    /// Called by some logic that unassigns a client from this hostess.
    /// </summary>
    public void UnassignCustomer()
    {
        customerPlace.DetachChildren();
        assignedCustomer.SetActive(false);
        assignedCustomer = null;
        // Stop charging if the hostess was in the middle of servicing
        if (serviceCoroutine != null)
        {
            StopServiceRoutine();
        }
    }

    public void AssignHost(GameObject hostGo)
    {
        Host host = hostGo.GetComponent<HostBehavior>().GetHost();
        if (host == null) return;
        if (assignedCustomer == null)
        {
            Debug.LogError("Host cannot be assigned: there is no customer behind this table");
        }

        assignedHost = hostGo;
        host = assignedHost.GetComponent<HostBehavior>().GetHost();
        PositionHost(assignedHost);
        if (shiftActive && serviceCoroutine == null)
        {
            StartServiceRoutine();
        }
    }

    private void PositionHost(GameObject host)
    {
        host.transform.SetParent(hostPlace.transform);
        host.transform.position = hostPlace.position;
    }

    /// <summary>
    /// Unassignes hostess from the table
    /// </summary>
    public void UnassignHost()
    {
        hostPlace.DetachChildren();
        assignedHost.GetComponent<HostBehavior>().Deactivate();
        assignedHost = null;
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

    private void FinishSession()
    {
        OnSessionFinished.Invoke(assignedHost);
        UnassignCustomer();
        UnassignHost();
    }

    /// <summary>
    /// This coroutine does two things:
    /// 1) Waits for M seconds at a time, charging money each interval.
    /// 2) Continues up to a total of N seconds, or until the hostess is unassigned or the shift ends.
    /// </summary>
    private IEnumerator ServeCustomerRoutine()
    {
        float timeElapsed = 0f;
        Host host = assignedHost.GetComponent<HostBehavior>().GetHost();
        while (SessionContinues() && timeElapsed < host.TimeWithCustomer)
        {
            // Wait M seconds
            yield return new WaitForSeconds(host.ChargeInterval);
            timeElapsed += host.ChargeInterval;

            // Check if we became unassigned or the shift ended during the wait
            if (!shiftActive || !HostAssigned())
            {
                // End early if conditions are no longer met
                yield break;
            }

            // Charge
            if (clubManager != null)
            {
                clubManager.AddIncome(host.ChargePerHour);
                Debug.Log($"{name} charged {host.ChargePerHour}. Current balance: {clubManager.GetCurrentBalance()}");
            }
        }

        // If we exit because we've hit totalServiceTime, we can automatically unassign the hostess if desired.
        if (timeElapsed >= host.TimeWithCustomer)
        {
            Debug.Log($"{name} finished {host.TimeWithCustomer} seconds with the client.");
            FinishSession();
        }

        serviceCoroutine = null;
    }
}

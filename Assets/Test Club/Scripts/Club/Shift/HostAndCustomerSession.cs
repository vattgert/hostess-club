using System;
using System.Collections;
using UnityEngine;

public class HostAndCustomerSession: MonoBehaviour
{
    private bool shiftActive;
    private readonly float waitingForHostTime = 10f;
    //private readonly float maxSessionTimeWithoutExtension = 90f; 
    private readonly float defaultSessionTime = 40f;

    private ShiftData shiftData;

    private ClubManager clubManager;

    private Coroutine serviceCoroutine;
    private Coroutine waitingHostCoroutine;

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

    void Start()
    {
        shiftData = FindFirstObjectByType<ShiftManager>().GetShiftData();
    }



    /// <summary>
    /// Check if a table is empty (no customer, no host).
    /// </summary>
    public bool TableEmpty()
    {
        return assignedCustomer == null && assignedHost == null;
    }

    /// <summary>
    /// Check if a customer is waiting for host
    /// </summary>
    public bool TableWaitsForHost()
    {
        return assignedCustomer != null && assignedHost == null;
    }

    /// <summary>
    /// Check if a host is assigned to the table
    /// </summary>
    private bool HostAssigned()
    {
        return assignedHost != null;
    }

    /// <summary>
    /// Check if a customer is assigned to the table
    /// </summary>
    private bool CustomerAssigned()
    {
        return assignedHost != null;
    }

    /// <summary>
    /// Check if the session is still active
    /// </summary>
    private bool SessionContinues()
    {
        return shiftActive && HostAssigned() && CustomerAssigned();
    }
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
        if(waitingHostCoroutine == null)
        {
            customer.GetComponent<CustomerBehavior>().StartWaiting();
            waitingHostCoroutine = StartCoroutine(WaitForHostToBeAssignedRoutine());
        }
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
        assignedCustomer.GetComponent<CustomerBehavior>().StopWaiting();
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
        assignedCustomer.GetComponent<CustomerBehavior>().StopWaiting();
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
        shiftData.AddServedCustomer();
        UnassignCustomer();
        UnassignHost();
    }

    private IEnumerator WaitForHostToBeAssignedRoutine()
    {
        Debug.Log("Start waiting for a host");
        float startTime = Time.time;

        // Loop until the host is assigned or the waiting time is exceeded.
        while (TableWaitsForHost() && (Time.time - startTime) < waitingForHostTime)
        {
            if (assignedHost != null)
            {
                Debug.Log("Customer has waited for a host");
                // Host assigned; exit the coroutine.
                yield break;
            }
            float timeRemaining = Mathf.Max(0, waitingForHostTime - (Time.time - startTime));
            int displayTime = Mathf.CeilToInt(timeRemaining);
            assignedCustomer.GetComponent<CustomerBehavior>().UpdateWaitingCountdown(displayTime);
            yield return null;
        }

        // If we've exceeded the waiting time, unassign the customer.
        if ((Time.time - startTime) >= waitingForHostTime)
        {
            Debug.Log("Customer leaves since host was not assigned");
            UnassignCustomer();
        }

        waitingHostCoroutine = null;
    }

    /// <summary>
    /// Coroutine to track time during the session and charges money from customer once per host.ChargeInterval
    /// </summary>
    private IEnumerator ServeCustomerRoutine()
    {
        float timeElapsed = 0f;
        Host host = assignedHost.GetComponent<HostBehavior>().GetHost();
        CustomerBehavior customer = assignedCustomer.GetComponent<CustomerBehavior>();
        while (SessionContinues() && timeElapsed < defaultSessionTime)
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
            if (customer.NextChargeOverflow())
            {
                Debug.Log($"Session ended due to customer balance overflow. \n{name} finished {timeElapsed} seconds with the client. \nCustomer balance: ${customer.GetCustomer().Budget}");
                FinishSession();
            } else
            {
                Debug.Log("Charging customer");
                int charged = customer.Charge();
                shiftData.AddEarning(host.Name, charged);
                Debug.Log($"{name} charged {charged}. \nCustomer balance: ${customer.GetCustomer().Budget}");
            }
        }

        // If we exit because we've hit totalServiceTime, we can automatically unassign the hostess if desired.
        if (timeElapsed >= defaultSessionTime)
        {
            Debug.Log($"Session ended due to time. \n{name} finished {timeElapsed} seconds with the client. \nClub balance: {clubManager.GetCurrentBalance()}. \nCustomer balance: ${customer.GetCustomer().Budget}");
            FinishSession();
        }

        serviceCoroutine = null;
    }
}

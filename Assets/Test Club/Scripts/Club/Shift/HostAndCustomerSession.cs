using System;
using System.Collections;
using UnityEngine;

public class HostAndCustomerSession: MonoBehaviour
{
    private bool shiftActive;
    private readonly float waitingForHostTime = 10f;
    private readonly float defaultSessionTime = 40f;

    private ShiftData shiftData;

    private ClubManager clubManager;

    private Coroutine serviceCoroutine;
    private Coroutine waitingHostCoroutine;

    private GameObject assignedCustomer;

    private GameObject assignedHost;

    private TableManager tableManager;
    private TablePanelUI tablePanelUI;

    public event Action<HostAndCustomerSession> OnSessionFinished;
    public event Action<GameObject> OnHostAssigned;

    private void Awake()
    {
        clubManager = ClubManager.GetInstance();
        tableManager = gameObject.GetComponentInChildren<TableManager>();
        tablePanelUI = gameObject.GetComponentInChildren<TablePanelUI>();
    }

    void Start()
    {
        shiftData = FindFirstObjectByType<ShiftManager>().GetShiftData();
    }

    /// <summary>
    /// Check if a table is empty (no customer, no host).
    /// </summary>
    public bool TableFree()
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
    public bool SessionActive()
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
    /// Assigns a customer to the session
    /// </summary>
    public void AssignCustomer(GameObject customer)
    {
        if (customer == null) return;

        assignedCustomer = customer;
        if(waitingHostCoroutine == null)
        {
            Debug.Log("Customer waiting must start here but for now I skip it");
            //customer.GetComponent<CustomerBehavior>().ActivateWaitingUI();
            //waitingHostCoroutine = StartCoroutine(WaitForHostToBeAssignedRoutine());
        }
    }

    /// <summary>
    /// Unassigns a customer from the session.
    /// </summary>
    public GameObject UnassignCustomer()
    {
        if(assignedCustomer != null)
        {
            GameObject customer = assignedCustomer;
            assignedCustomer.GetComponent<CustomerBehavior>().StopWaiting();
            assignedCustomer.GetComponent<CustomerBehavior>().FinishedSession = true;
            assignedCustomer = null;
            // Stop charging if the hostess was in the middle of servicing
            if (serviceCoroutine != null)
            {
                StopServiceRoutine();
            }
            tablePanelUI.ClearUI();
            return customer;
        }
        return null;
    }

    /// <summary>
    /// Returns the current session host
    /// </summary>
    public GameObject GetHost()
    {
        return assignedHost;
    }

    /// <summary>
    /// Assigns a host to a waiting customer and starts host service session
    /// </summary>
    public void AssignHost(GameObject hostGo)
    {
        if (hostGo == null)
        {
            Debug.LogError("You are trying to assign host which value is 'null'");
            return;
        }

        if (assignedCustomer == null)
        {
            Debug.LogError("Host cannot be assigned: there is no customer behind this table");
            return;
        }

        assignedHost = hostGo;
        assignedCustomer.GetComponent<CustomerBehavior>().StopWaiting();
        Host host = assignedHost.GetComponent<HostBehavior>().GetHost();
        tablePanelUI.ShowPanel(host);
        if (shiftActive && serviceCoroutine == null)
        {
            StartServiceRoutine();
        }
        OnHostAssigned?.Invoke(assignedHost);
    }

    /// <summary>
    /// Unassignes hostess from the table
    /// </summary>
    public GameObject UnassignHost()
    {
        if (assignedHost != null)
        {
            GameObject host = assignedHost;
            //tableManager.RemoveHostFromPlace();
            tablePanelUI.HidePanel();
            assignedHost = null;
            return host;
        }
        return null;
        // Here I must run waiting timer for customer
    }

    /// <summary>
    /// Starts a coroutine of serving the customer by the hostess
    /// </summary>
    private void StartServiceRoutine()
    {
        serviceCoroutine = StartCoroutine(ServeCustomerRoutine());
    }

    /// <summary>
    /// Stops the serving coroutine
    /// </summary>
    private void StopServiceRoutine()
    {
        StopCoroutine(serviceCoroutine);
        serviceCoroutine = null;
    }

    /// <summary>
    /// Finishes the session
    /// </summary>
    private void FinishSession()
    {
        OnSessionFinished.Invoke(this);
        shiftData.AddServedCustomer();
    }

    /// <summary>
    /// Clears the session
    /// </summary>
    public void ClearSession()
    {
        SetShiftActive(false);
        UnassignHost();
        UnassignCustomer();
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
        while (SessionActive() && timeElapsed < defaultSessionTime)
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

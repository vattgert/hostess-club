using Session;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters;

public struct SessionSettings {
    public float Duration { get; private set; }
    public float ChargeInterval { get; private set; }

    public SessionSettings(float duration, float chargeInterval)
    {
        Duration = duration;
        ChargeInterval = chargeInterval;
    }

    public override string ToString()
    {
        return "Duration: " + Duration + "\n"  + "Charge interval: " + ChargeInterval;
    }
}

public class HostAndCustomerSession: MonoBehaviour
{
    private bool shiftActive;
    private readonly float waitingForHostTime = 10f;
    private readonly float DEFAULT_SESSION_TIME = 40f;
    private readonly float SESSION_COROUTINE_TICK_INTERVAL = 1f;

    public SessionSettings SessionSettings { get; private set; }
    public SessionBilling SessionBilling { get; private set; }
    public ShiftData ShiftData { get; private set; }

    private ClubManager clubManager;

    private Coroutine serviceCoroutine;
    private Coroutine waitingHostCoroutine;

    private GameObject assignedCustomer;

    private GameObject assignedHost;

    private TablePanelUI tablePanelUI;

    private List<ISessionModifier> modifiers = new();

    public SessionEvents SessionEvents { get; private set; }
    public event Action<HostAndCustomerSession> OnSessionFinished;
    public event Action<GameObject> OnHostAssigned;

    private void Awake()
    {
        clubManager = ClubManager.GetInstance();
        SessionEvents = new SessionEvents();
        tablePanelUI = gameObject.GetComponentInChildren<TablePanelUI>();
    }

    void Start()
    {
        ShiftData = FindFirstObjectByType<ShiftManager>().GetShiftData();
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
    /// Returns the current session customer
    /// </summary>
    public GameObject Customer()
    {
        return assignedCustomer;
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
            customer.GetComponent<CustomerBehavior>().SetState(CustomerState.Seated);
            customer.GetComponent<CustomerReactions>().Setup(SessionEvents);
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
    public GameObject Host()
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
        HostBehavior hb = assignedHost.GetComponent<HostBehavior>();
        hb.SetState(HostState.Seated);
        //assignedCustomer.GetComponent<CustomerBehavior>().StopWaiting();
        Host host = hb.Host;
        tablePanelUI.ShowPanel(host);
        if (shiftActive && serviceCoroutine == null)
        {
            StartSession();
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
            tablePanelUI.HidePanel();
            GameObject host = assignedHost;
            assignedHost = null;
            serviceCoroutine = null; // Should I nullify it or put it "on pause" or something to that extent?
            return host;
        }
        return null;
        // Here I must run waiting timer for customer
    }

    private void SetSessionSettings()
    {
        Host host = assignedHost.GetComponent<HostBehavior>().Host;
        SessionSettings = new SessionSettings(DEFAULT_SESSION_TIME, host.ChargeInterval);
    }

    private void SetSessionBilling()
    {
        Host host = assignedHost.GetComponent<HostBehavior>().Host;
        Customer customer = assignedCustomer.GetComponent<CustomerBehavior>().Customer;
        SessionBilling = new SessionBilling(host, customer, ShiftData);
    }

    private void StartSession()
    {
        Debug.Log("Starting the session");
        SetSessionSettings();
        SetSessionBilling();
        StartModifiers();
        StartServiceRoutine();
    }

    /// <summary>
    /// Starts a coroutine of serving the customer by the hostess
    /// </summary>
    private void StartServiceRoutine()
    {
        assignedCustomer.GetComponent<CustomerBehavior>().SetState(CustomerState.InSession);
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
    public void FinishSession()
    {
        OnSessionFinished.Invoke(this);
        ClearModifiers();
        ShiftData.AddServedCustomer();
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
        Host host = assignedHost.GetComponent<HostBehavior>().Host;
        CustomerBehavior customer = assignedCustomer.GetComponent<CustomerBehavior>();
        while (SessionActive() && timeElapsed < SessionSettings.Duration)
        {
            // Wait M seconds
            yield return new WaitForSeconds(SESSION_COROUTINE_TICK_INTERVAL);
            timeElapsed += SESSION_COROUTINE_TICK_INTERVAL;
            // Check if we became unassigned or the shift ended during the wait
            if (!shiftActive || !HostAssigned())
            {
                // End early if conditions are no longer met
                yield break;
            }

            UpdateModifiers(SESSION_COROUTINE_TICK_INTERVAL);
        }

        // Session time expired
        if (timeElapsed >= SessionSettings.Duration)
        {
            Debug.Log($"Session ended due to time. \n{name} finished {timeElapsed} seconds with the client. \nClub balance: {clubManager.GetCurrentBalance()}. \nCustomer balance: ${customer.Customer.Budget}");
            FinishSession();
        }

        serviceCoroutine = null;
    }

    private void StartModifiers()
    {
        modifiers.Add(new ChargeModifier());
        modifiers.Add(new CompatibilityModifier());
        foreach (var modifier in modifiers)
        {
            Debug.Log(modifier.GetType());
            modifier.OnSessionStart(this);
        }
    }

    private void UpdateModifiers(float timeLeft)
    {
        foreach (var modifier in modifiers)
        {
            modifier.OnSessionUpdate(this, timeLeft);
        }
    }

    private void ClearModifiers()
    {
        foreach (var modifier in modifiers)
        {
            modifier.OnSessionEnd(this);
        }
    }
}

using System;
using UnityEngine;

public class TablesManager : MonoBehaviour
{
    private HostAndCustomerSession[] tables;
    private HostManager hostManager;
    private CustomerInvitationManager customerInvitationManager;
    private GameObject selectedHost;

    public event Action<GameObject> OnHostAssigned;
    public event Action<GameObject> OnSessionFinished;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tables = FindObjectsByType<HostAndCustomerSession>(FindObjectsSortMode.None);
        foreach(HostAndCustomerSession table in tables)
        {
            table.OnSessionFinished += HandleOnSessionFinished;
        }
        hostManager = gameObject.GetComponent<HostManager>();
        customerInvitationManager = gameObject.GetComponent<CustomerInvitationManager>();
        customerInvitationManager.OnCustomerInvited += AssignCustomerToFreeTable;
    }

    private void HandleOnSessionFinished(GameObject host)
    {
        Debug.Log("Hopefully, event triggered only once");
        OnSessionFinished.Invoke(host);
    }

    public void AssignCustomerToFreeTable(GameObject customer)
    {
        foreach(HostAndCustomerSession session in tables)
        {
            if (session.TableEmpty())
            {
                session.AssignCustomer(customer);
                break;
            }
        }
    }

    public bool HasFreeTables()
    {
        foreach (HostAndCustomerSession session in tables)
        {
            if (session.TableEmpty())
            {
                return true;
            }
        }
        return false;
    }

    public void HighlightTablesWaitingForHost(GameObject host)
    {
        selectedHost = host;
        foreach (var table in tables)
        {
            if (table.TableWaitsForHost())
            {
                TableInteraction tableUI = table.GetComponent<TableInteraction>();
                tableUI.SetHighlight(true);
            }
        }
    }

    public void HighlightedTableClicked(TableInteraction table)
    {
        Debug.Log("Table is highlight: " + table.isHighlighted);
        Debug.Log("Selected is null: " + selectedHost == null);
        if (table.isHighlighted && selectedHost != null)
        {
            HostAndCustomerSession session = table.gameObject.GetComponent<HostAndCustomerSession>();
            session.AssignHost(selectedHost);
            OnHostAssigned.Invoke(selectedHost);
            Debug.Log("Host list count: " + hostManager.GetShiftHosts().Count);
        }
        ClearHighlights();
    }

    public void ClearHighlights()
    {
        foreach (var table in tables)
        {
            TableInteraction tableUI = table.GetComponent<TableInteraction>();
            tableUI.SetHighlight(false);
        }
        selectedHost = null;
    }
}

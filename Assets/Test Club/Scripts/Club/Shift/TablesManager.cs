using System;
using UnityEngine;

public class TablesManager : MonoBehaviour
{
    private HostAndCustomerSession[] tables;
    private CustomerManager customerManager;
    private GameObject selectedHost;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.tables = FindObjectsByType<HostAndCustomerSession>(FindObjectsSortMode.None);
        this.customerManager = gameObject.GetComponent<CustomerManager>();
        this.customerManager.OnCustomerEntered += this.AssignCustomerToFreeTable;
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
        this.selectedHost = host;
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
        Debug.Log("Selected is null: " + this.selectedHost == null);
        if (table.isHighlighted && selectedHost != null)
        {
            HostAndCustomerSession session = table.gameObject.GetComponent<HostAndCustomerSession>();
            session.AssignHost(selectedHost);
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
        this.selectedHost = null;
    }
}

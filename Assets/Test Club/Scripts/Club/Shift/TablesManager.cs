using UnityEngine;

public class TablesManager : MonoBehaviour
{
    private HostAndCustomerSession[] tables;
    private HostBehavior selectedHost;
    private CustomerManager customerManager;

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

    public void HighlightAvailableTables(/*HostBehavior host*/)
    {
        //this.selectedHost = host;
        foreach (var table in tables)
        {
            if (table.TableWaitsForHost())
            {
                TableInteraction tableUI = table.GetComponent<TableInteraction>();
                tableUI.SetHighlight(true);
            }
        }
    }
}

using UnityEngine;

public class TablesManager : MonoBehaviour
{
    [SerializeField]
    private ShiftHostsUI shiftHostsUI;

    [SerializeField]
    private ReceptionZone receptionZone;

    private HostAndCustomerSession[] tables;
    private CustomerInvitationManager customerInvitationManager;
    private GameObject selectedTable;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tables = FindObjectsByType<HostAndCustomerSession>(FindObjectsSortMode.None);
        foreach(HostAndCustomerSession table in tables)
        {
            table.OnSessionFinished += shiftHostsUI.AddHostToList;
            table.OnHostAssigned += shiftHostsUI.RemoveHostFromList;
        }
        customerInvitationManager = gameObject.GetComponent<CustomerInvitationManager>();
        //customerInvitationManager.OnCustomerInvited += AssignCustomerToFreeTable;
    }

    public HostAndCustomerSession[] GetSessions()
    {
        return tables;
    }

    public GameObject SelectedTable()
    {
        return selectedTable;
    }

    public void SelectTable(GameObject table)
    {
        selectedTable = table;
    }

    private void ClearSelected()
    {
        selectedTable = null;
    }

    public void AssignCustomerToFreeTable(GameObject customer)
    {
        foreach(HostAndCustomerSession session in tables)
        {
            if (session.TableFree())
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
            if (session.TableFree())
            {
                return true;
            }
        }
        return false;
    }

    public void HighlightFreeTables()
    {
        foreach (var table in tables)
        {
            if (table.TableFree())
            {
                TablePanelUI tableUI = table.GetComponentInChildren<TablePanelUI>();
                tableUI.Highlight(true);
            }
        }
    }

    public void HighlightTablesWaitingForHost()
    {
        foreach (var table in tables)
        {
            if (table.TableWaitsForHost())
            {
                TablePanelUI tableUI = table.GetComponentInChildren<TablePanelUI>();
                tableUI.Highlight(true);
            }
        }
    }

    public void HighlightWaitingAndActiveTables()
    {
        foreach (var table in tables)
        {
            if (table.TableWaitsForHost() || table.SessionActive())
            {
                TablePanelUI tableUI = table.GetComponentInChildren<TablePanelUI>();
                tableUI.Highlight(true);
            }
        }
    }

    public void HightlightActiveTables(GameObject host)
    {
        foreach (var table in tables)
        {
            if (table.TableWaitsForHost() || table.SessionActive())
            {
                TablePanelUI tableUI = table.GetComponentInChildren<TablePanelUI>();
                tableUI.Highlight(true);
            }
        }
    }

    public void TableClicked(HostAndCustomerSession session)
    {
        if (session.SessionActive())
        {
            SelectTable(session.gameObject);
            HightlightActiveTables(session.GetHost());
        }
    }

    public void HighlightedTableClicked(HostAndCustomerSession session)
    {
        GameObject selectedHost = shiftHostsUI.SelectedHost();
        bool hostFromListAssignedToTable = selectedHost != null && selectedTable == null;
        bool hostFromTableAssignedToTable = selectedHost == null && selectedTable != null;

        if (session.TableFree())
        {
            receptionZone.WalkCustomerToSelectedTable(session);
        }
        else if (hostFromListAssignedToTable)
        {
            if (session.SessionActive())
            {
                SwapHostsBetweenSessionAndList(selectedHost, session);
            } else
            {
                AssignHostToSession(session);
            }
        } 
        else if (hostFromTableAssignedToTable)
        {   
            SwapHostsBetweenSessions(session);
        }
        ClearHighlights();
        ClearSelections();
    }

    private void ClearHighlights()
    {
        foreach (var table in tables)
        {
            TablePanelUI tableUI = table.GetComponentInChildren<TablePanelUI>();
            tableUI.Highlight(false);
        }
    }

    private void ClearSelections()
    {
        ClearSelected();
        shiftHostsUI.ClearSelection();
    }

    private void AssignHostToSession(HostAndCustomerSession session)
    {
        session.AssignHost(shiftHostsUI.SelectedHost());
    }

    private void SwapHostsBetweenSessions(HostAndCustomerSession target)
    {
        Debug.Log("Swapping hosts between tables");
        HostAndCustomerSession source = selectedTable.GetComponent<HostAndCustomerSession>();
        GameObject sourceHost = source.UnassignHost();
        GameObject targetHost = target.UnassignHost();
        target.AssignHost(sourceHost);
        source.AssignHost(targetHost);
    }

    private void SwapHostsBetweenSessionAndList(GameObject newHost, HostAndCustomerSession session)
    {
        Debug.Log("Swapping hosts between table and host list");
        GameObject hostFromSession = session.UnassignHost();
        session.AssignHost(newHost);
        shiftHostsUI.AddHostToList(hostFromSession);
    }

    public void SwapHostsBetweenSessionAndListAndClear(GameObject newHost, HostAndCustomerSession session)
    {
        SwapHostsBetweenSessionAndList(newHost, session);
        ClearHighlights();
        ClearSelections();
    }

    public void ClearSessions()
    {
        foreach (HostAndCustomerSession table in tables)
        {
            table.ClearSession();
        }
    }
}

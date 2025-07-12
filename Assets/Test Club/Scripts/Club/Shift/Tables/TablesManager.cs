using UnityEngine;

public class TablesManager : MonoBehaviour
{
    [SerializeField]
    private ShiftHostsUI shiftHostsUI;
    private HostAndCustomerSession[] tables;
    [SerializeField]
    private ReceptionZone receptionZone;
    [SerializeField]
    private StuffOnlyZone stuffOnlyZone;
    [SerializeField]
    private HostManager hostManager;
    private GameObject selectedTable;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tables = FindObjectsByType<HostAndCustomerSession>(FindObjectsSortMode.None);
        foreach(HostAndCustomerSession table in tables)
        {
            table.OnSessionFinished += UnassignHostAndCustomerFromSession;
            table.OnHostAssigned += shiftHostsUI.RemoveHostFromList;
        }
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
            receptionZone.MoveCustomerToSelectedTable(session);
        }
        else if (hostFromListAssignedToTable)
        {
            if (session.SessionActive())
            {
                SwapHostsBetweenSessionAndList(selectedHost, session);
            } 
            else
            {
                hostManager.CallHost(selectedHost, session);
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

    private void UnassignHostAndCustomerFromSession(HostAndCustomerSession session)
    {
        GameObject table = session.gameObject;
        GameObject customer = session.UnassignCustomer();
        GameObject host = session.UnassignHost();
        customer.GetComponent<CustomerBehavior>().SetState(CustomerState.Leaving);
        host.GetComponent<HostBehavior>().SetState(HostState.Leaving);
        CharacterMovementController.Instance.AStarMoveTo(customer, receptionZone.GetCustomerExit(), true);
        CharacterMovementController.Instance.AStarMoveTo(host, stuffOnlyZone.HostSpawnPoint(), true);
    }

    private void SwapHostsBetweenSessions(HostAndCustomerSession targetSession)
    {
        Debug.Log("Swapping hosts between tables");
        HostAndCustomerSession sourceSession = selectedTable.GetComponent<HostAndCustomerSession>();
        GameObject sourceHost = sourceSession.UnassignHost();
        GameObject targetHost = targetSession.UnassignHost();
        TableManager sourceTable = sourceSession.GetComponent<TableManager>();
        TableManager targetTable = targetSession.GetComponent<TableManager>();
        CharacterMovementController.Instance.AStarMoveTo(sourceHost, targetTable.HostSeat(), true);
        CharacterMovementController.Instance.AStarMoveTo(targetHost, sourceTable.HostSeat(), true);
    }

    private void SwapHostsBetweenSessionAndList(GameObject newHost, HostAndCustomerSession session)
    {
        Debug.Log("Swapping hosts between table and host list");
        GameObject hostFromSession = session.UnassignHost();
        hostManager.CallHost(newHost, session);
        TableManager table = session.GetComponent<TableManager>();
        CharacterMovementController.Instance.AStarMoveTo(hostFromSession, stuffOnlyZone.HostSpawnPoint(), true);
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

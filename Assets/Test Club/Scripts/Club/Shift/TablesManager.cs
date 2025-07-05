using UnityEngine;

public class TablesManager : MonoBehaviour
{
    [SerializeField]
    private ShiftHostsUI shiftHostsUI;

    [SerializeField]
    private ReceptionZone receptionZone;
    private HostManager hostManager;
    private HostAndCustomerSession[] tables;
    private GameObject selectedTable;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hostManager = gameObject.GetComponent<HostManager>();
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
            receptionZone.WalkCustomerToSelectedTable(session);
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
        UnsubscribeOnCharacterArrival(customer);
        UnsubscribeOnCharacterArrival(host);
        customer.GetComponent<CustomerMovement>().WalkFromTable(table);
        customer.GetComponent<CustomerBehavior>().SetState(CustomerState.Leaving);
        host.GetComponent<HostMovement>().WalkFromTable(table);
        host.GetComponent<HostBehavior>().SetState(HostState.Leaving);
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

    private void AssignCharacterToSession(GameObject character, Transform arrival)
    {
        HostAndCustomerSession session = arrival.GetComponentInParent<HostAndCustomerSession>();
        if(session != null)
        {
            if (character.CompareTag("Customer"))
            {
                session.AssignCustomer(character);
            }
            else if (character.CompareTag("Host"))
            {
                session.AssignHost(character);
            }
        }
    }

    public void SubscribeOnCharacterArrival(GameObject character)
    {
        character.GetComponent<WaypointsMovement>().OnArrivedAtDestination += AssignCharacterToSession;
    }

    private void UnsubscribeOnCharacterArrival(GameObject character)
    {
        character.GetComponent<WaypointsMovement>().OnArrivedAtDestination -= AssignCharacterToSession;
    }
}

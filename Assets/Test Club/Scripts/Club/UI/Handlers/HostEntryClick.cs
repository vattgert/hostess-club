using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HostEntryClick : MonoBehaviour, IPointerClickHandler
{
    TablesManager tablesManager;
    ShiftHostsUI shiftHostsUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tablesManager = FindFirstObjectByType<TablesManager>();
        shiftHostsUI = GetComponentInParent<ShiftHostsUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        HostEntryUI hostUI = gameObject.GetComponent<HostEntryUI>();
        if(hostUI != null)
        {
            GameObject host = hostUI.GetHost();
            GameObject selectedTable = tablesManager.SelectedTable();
            if (selectedTable != null)
            {
                HostAndCustomerSession tableSession = selectedTable.GetComponent<HostAndCustomerSession>();
                tablesManager.SwapHostsBetweenSessionAndListAndClear(host, tableSession);
            }
            else
            {
                shiftHostsUI.SelectHost(host);
                tablesManager.HighlightWaitingAndActiveTables();
            } 
        }
    }
}

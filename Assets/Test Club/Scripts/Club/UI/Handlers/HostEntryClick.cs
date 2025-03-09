using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HostEntryClick : MonoBehaviour, IPointerClickHandler
{
    TablesManager tablesManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.tablesManager = FindFirstObjectByType<TablesManager>();
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
            // When clicked, tell the TableManager to highlight available tables using this host.
            this.tablesManager.HighlightTablesWaitingForHost(host);
        }
        
    }
}

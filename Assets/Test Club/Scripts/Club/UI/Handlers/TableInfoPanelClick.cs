using UnityEngine;

public class TableInfoPanelClick : MonoBehaviour
{
    [SerializeField]
    private HostAndCustomerSession session;
    [SerializeField]
    private TablePanelUI tablePanelUI;
    private TablesManager tablesManager;

    private void Start()
    {
        tablesManager = FindFirstObjectByType<TablesManager>();
    }

    public void OnTableInfoClick()
    {
        Debug.Log("Click on table management canvas");
        if (tablePanelUI.Highlighted())
        {
            tablesManager.HighlightedTableClicked(session);
        }
        else
        {
            tablesManager.TableClicked(session);
        }
    }
}

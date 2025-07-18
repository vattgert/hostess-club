using TMPro;
using UnityEngine;
using Characters;

public class TablePanelUI : MonoBehaviour
{
    private Canvas tableManagmentCanvas;
    private TextMeshProUGUI hostNameText;
    private TablePanelOverlayPulseTrigger pulseTrigger;
    private bool isHighlighted = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tableManagmentCanvas = gameObject.GetComponentInChildren<Canvas>();
        if (tableManagmentCanvas != null)
        {
            pulseTrigger = tableManagmentCanvas.GetComponentInChildren<TablePanelOverlayPulseTrigger>();
            hostNameText = tableManagmentCanvas.GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    public void ShowPanel(Host host)
    {
        gameObject.SetActive(true);
        if(hostNameText != null)
        {
            hostNameText.text = host.Name;
        }
    }

    public void HidePanel()
    {
        hostNameText.text = "";
    }

    public void Highlight(bool highlight)
    {
        isHighlighted = highlight;
        if (isHighlighted)
        {
            pulseTrigger.PlayPulse();
        } else
        {
            pulseTrigger.StopPulse();
        }
    }

    public bool Highlighted()
    {
        return isHighlighted;
    }

    public void ClearUI()
    {
        Highlight(false);
    }
}

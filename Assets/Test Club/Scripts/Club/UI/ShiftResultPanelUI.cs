using TMPro;
using UnityEngine;

public class ShiftResultPanelUI : MonoBehaviour
{
    private ShiftManager shiftManager;
    private RectTransform shiftResultPanel;
    private ShiftUI shiftUI;

    private void Awake()
    {
        Debug.Log("Shift result panel script active");
        shiftUI = FindAnyObjectByType<ShiftUI>();
        shiftResultPanel = gameObject.GetComponentInChildren<RectTransform>();
        shiftResultPanel.gameObject.SetActive(false);
        shiftManager = FindAnyObjectByType<ShiftManager>();
        Debug.Log(shiftManager);
        if (shiftManager != null)
        {
            shiftManager.OnShiftFinished += OnShiftFinishedEventHandler;
        }
    }

    private void PopulateShiftResultData(RectTransform shiftResultPanel, ShiftData shiftData)
    {
        // Get all TextMeshProUGUI components in children (including inactive ones)
        TextMeshProUGUI[] allTexts = shiftResultPanel.gameObject.GetComponentsInChildren<TextMeshProUGUI>(true);
        Debug.Log("Texts");
        Debug.Log(allTexts);
        Debug.Log(shiftData);
        foreach (var text in allTexts)
        {
            if (text.name.Equals("EarnedForShiftValue"))
            {
                text.text = shiftData.CumulativeEarnedPerShift.ToString();
            } else if (text.name.Equals("CustomersServedValue"))
            {
                text.text = shiftData.ServedCustomers.ToString();
            }
        }
    }

    private void OnShiftFinishedEventHandler(ShiftData shiftData)
    {
        Debug.Log("Pass shift result data here I believe and fill in ");
        Time.timeScale = 0;
        shiftUI.ShowDimBackground();
        shiftResultPanel.gameObject.SetActive(true);
        PopulateShiftResultData(shiftResultPanel, shiftData);
    }

    private void OnDestroy()
    {
        Debug.Log("Does ShiftResultUI destroy itself here?");
        if (shiftManager != null)
        {
            shiftManager.OnShiftFinished -= OnShiftFinishedEventHandler;
        }
    }
}

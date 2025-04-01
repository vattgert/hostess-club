using UnityEngine;
using UnityEngine.EventSystems;

public class ShiftResultPanelCloseClickHandler : MonoBehaviour
{
    private ShiftUI shiftUI;

    private void Awake()
    {
        shiftUI = FindFirstObjectByType<ShiftUI>();
    }

    public void CloseShiftResultPanel()
    {
        Debug.Log("Invoke a transition to post-shift schene");
        GameObject shiftResultPanel = transform.parent.gameObject;
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        shiftResultPanel.SetActive(false);
        if(shiftUI != null)
        {
            shiftUI.HideShiftUI();
        }
    }
}

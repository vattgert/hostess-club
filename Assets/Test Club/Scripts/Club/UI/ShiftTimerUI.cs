using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShiftTimerUI : MonoBehaviour
{
    [SerializeField] private ShiftTimer shiftTimer;
    private TextMeshProUGUI textComponent;

    private void Awake()
    {
        // Get the TMP component from the *same* GameObject
        textComponent = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        // Subscribe to timer events
        if (shiftTimer != null)
        {
            shiftTimer.OnShiftTimerUpdate += UpdateTimeLeft;
            shiftTimer.OnShiftTimerComplete += HandleShiftComplete;
        }
    }

    private void OnDisable()
    {
        // Unsubscribe to avoid potential memory leaks
        if (shiftTimer != null)
        {
            shiftTimer.OnShiftTimerUpdate -= UpdateTimeLeft;
            shiftTimer.OnShiftTimerComplete -= HandleShiftComplete;
        }
    }

    /// <summary>
    /// Called every time the ShiftTimer fires OnShiftTimerUpdated.
    /// </summary>
    private void UpdateTimeLeft(float timeLeft)
    {
        if (textComponent != null)
        {
            // Round up or down as suits your design:
            // - CeilToInt rounds UP, so "0.1" becomes 1 second
            // - FloorToInt rounds down, so "4.9" becomes 4
            // For a countdown, CeilToInt is often intuitive, but pick the approach you need.
            int totalSeconds = Mathf.CeilToInt(timeLeft);

            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;

            // Format like "01:05" for 1 minute 5 seconds
            textComponent.text = $"{minutes:00}:{seconds:00}";
        }
    }

    /// <summary>
    /// Called once when the timer hits 0.
    /// </summary>
    private void HandleShiftComplete()
    {
        if(textComponent != null)
        {
            textComponent.text = "Shift complete!";
        }
    }
}

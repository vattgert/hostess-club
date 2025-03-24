using System;
using System.Collections;
using UnityEngine;

public class ShiftTimer : MonoBehaviour
{
    /// <summary>
    /// Shift length in seconds.
    /// </summary>
    private readonly float shiftDuration = 300f;
    /// <summary>
    /// Time left until the end of the shift.
    /// </summary>
    public float TimeLeft { get; private set; }
    /// <summary>
    /// The coroutine, which manages the time for a shift.
    /// </summary>
    private Coroutine shiftTimerCoroutine;
    /// <summary>
    /// This event fires when the timer completes (the shift should end).
    /// </summary>
    public event Action OnShiftTimerComplete;
    /// <summary>
    /// This event fires each time when the timer updates (every second presumably).
    /// </summary>
    public event Action<float> OnShiftTimerUpdate;

    public Coroutine GetTimer()
    {
        return shiftTimerCoroutine;
    }

    public float ShitDuration()
    {
        return shiftDuration;
    }

    private IEnumerator TimerRoutineWithUpdate()
    {
        TimeLeft = shiftDuration;

        while (TimeLeft > 0f)
        {
            TimeLeft -= Time.deltaTime;

            OnShiftTimerUpdate.Invoke(TimeLeft);
            yield return null;
        }

        TimeLeft = 0f;
        OnShiftTimerUpdate.Invoke(TimeLeft);
        StopTimer();
        OnShiftTimerComplete.Invoke();
    }

    /// <summary>
    /// Starts the timer if it isn't already running.
    /// </summary>
    public void StartTimer()
    {
        if (shiftTimerCoroutine == null)
        {
            Debug.Log("Shift timer starting");
            shiftTimerCoroutine = StartCoroutine(TimerRoutineWithUpdate());
        }
    }

    /// <summary>
    /// Stops the timer if it's currently running.
    /// </summary>
    public void StopTimer()
    {
        if (shiftTimerCoroutine != null)
        {
            StopCoroutine(shiftTimerCoroutine);
            shiftTimerCoroutine = null;
            Debug.Log("Shift timer ended");
        }
    }
}

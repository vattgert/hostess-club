using UnityEngine;

public class HoldInput
{
    private KeyCode key;
    private float holdDuration = 1f;
    private float holdTimer = 0f;
    private bool holding = false;
    private bool noRepeatedExecution = true;
    private int timesExecuted = 0;

    public HoldInput(KeyCode key, bool noRepeatedExecution = true)
    {
        this.key = key;
        this.noRepeatedExecution = noRepeatedExecution;
    }

    private bool ExecutedMoreThanOnce()
    {
        return noRepeatedExecution == true && timesExecuted >= 1;
    }

    private void ResetExecution()
    {
        timesExecuted = 0;
    }

    private void StartHolding()
    {
        if (!holding)
        {
            holding = true;
            holdTimer = 0f;
        }
    }

    private void ResetHolding()
    {
        holding = false;
        holdTimer = 0f;
        if (noRepeatedExecution)
        {
            ResetExecution();
        }
    }

    private void UpdateHolding(System.Action callback)
    {
        holdTimer += Time.deltaTime;

        if (holdTimer >= holdDuration)
        {
            // Trigger your interaction
            Debug.Log(key.ToString() + " held long enough!");
            callback?.Invoke();
            timesExecuted += 1;
            holding = false; // Prevent retriggering unless released and held again
        }
    }

    public void Hold(System.Action callback)
    {
        if (Input.GetKey(key) && !ExecutedMoreThanOnce())
        {
            StartHolding();
            UpdateHolding(callback);
        }

        if (Input.GetKeyUp(key))
        {
            ResetHolding();
        }
    }
}

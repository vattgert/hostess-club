using Characters;
using System;
using UnityEngine;

public class SessionEvents
{
    public event Action<GameObject, Compatibility> OnCompatibilityReaction;

    public void TriggerReaction(GameObject customer, Compatibility compatibility)
    {
        OnCompatibilityReaction?.Invoke(customer, compatibility);
    }
}
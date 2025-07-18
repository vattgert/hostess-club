using UnityEngine;
using Characters;

public enum HostState
{
    Entering,
    AssignedMovingToTable,
    Seated,
    InSession,
    Leaving
}

public class HostBehavior : MonoBehaviour
{
    public Host Host { get; private set; }
    public HostState CurrentState { get; private set; }

    public void Initialize(Host host)
    {
        this.Host = host;
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void SetState(HostState state)
    {
        if (CurrentState != state)
        {
            CurrentState = state;
            Debug.Log($"Host {this.name} changed state to {CurrentState}");
        }
    }
}

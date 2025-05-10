using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class WaypointsMovement : MonoBehaviour
{
    protected LinkedList<Transform> path;
    protected LinkedListNode<Transform> currentWaypoint;
    private LinkedListNode<Transform> previousWaypoint;
    protected bool isWalking = false;
    [SerializeField]
    protected float moveSpeed = 2f;
    public event Action<GameObject, Transform> OnArrivedAtDestination;

    protected virtual void OnArrivedAtFinalWaypoint(Transform arrivedAt)
    {
        Debug.Log("On final waypoint arrived");
    }

    protected void StartWalking(LinkedList<Transform> pathToWalk)
    {
        path = pathToWalk;
        currentWaypoint = path.First;
        previousWaypoint = currentWaypoint;
        isWalking = true;
    }

    protected void RaiseArrivalEvent(GameObject go, Transform arrival)
    {
        OnArrivedAtDestination?.Invoke(go, arrival);
    }

    protected void Walk()
    {
        if (!isWalking || currentWaypoint == null)
            return;

        Vector3 direction = currentWaypoint.Value.position - transform.position;

        if (direction.magnitude < 0.05f) // Distance threshold, tweakable
        {
            // Arrived at this waypoint
            previousWaypoint = currentWaypoint;
            currentWaypoint = currentWaypoint.Next;

            if (currentWaypoint == null)
            {
                // No more waypoints
                isWalking = false;
                OnArrivedAtFinalWaypoint(previousWaypoint.Value);
                return;
            }
        }
        else
        {
            // Move toward current waypoint
            transform.position += direction.normalized * moveSpeed * Time.deltaTime;
        }
    }
}

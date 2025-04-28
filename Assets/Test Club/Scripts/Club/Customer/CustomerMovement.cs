using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerMovement : MonoBehaviour
{
    private LinkedList<Transform> path;
    private LinkedListNode<Transform> currentWaypoint;
    private bool isWalking = false;
    [SerializeField]
    private float moveSpeed = 2f;

    private void StartWalking(LinkedList<Transform> pathToWalk)
    {
        path = pathToWalk;
        currentWaypoint = path.First;
        isWalking = true;
    }

    public void TakePlaceBehindTheTable(GameObject table)
    {
        TableManager tm = table.GetComponent<TableManager>();
        LinkedList<Transform> way = table.GetComponent<TablePath>().PathOfPoints();
        way.AddLast(tm.CustomerPlace());
        StartWalking(way);
    }

    private void Walk()
    {
        if (!isWalking || currentWaypoint == null)
            return;

        Vector3 direction = currentWaypoint.Value.position - transform.position;

        if (direction.magnitude < 0.05f) // Distance threshold, tweakable
        {
            // Arrived at this waypoint
            currentWaypoint = currentWaypoint.Next;

            if (currentWaypoint == null)
            {
                // No more waypoints
                isWalking = false;
                //OnArrivedAtDestination();
                return;
            }
        }
        else
        {
            // Move toward current waypoint
            transform.position += direction.normalized * moveSpeed * Time.deltaTime;
        }
    }

    private void Update()
    {
        Walk();
    }
}

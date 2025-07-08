using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStarMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 2f;
    private bool moving = false;
    private List<Vector3> path;
    private int currentTargetIndex = 0;
    public event Action<GameObject, Transform> OnArrivedAtDestination;

    private void RaiseArrivalEvent(GameObject go, Transform arrival)
    {
        OnArrivedAtDestination?.Invoke(go, arrival);
        OnArrivedAtDestination = null;
    }

    private void Arrived(Transform arrival)
    {
        Debug.Log("On final position arrived");
        StopMoving();
        RaiseArrivalEvent(this.gameObject, arrival);
    }

    private void Move()
    {
        if (moving)
        {
            if (path == null || path.Count == 0)
            {
                Debug.Log("Object cannot move since there is something with the path");
                return;
            }
            if (currentTargetIndex >= path.Count)
            {
                Arrived(transform);
                return;
            }

            Vector3 target = path[currentTargetIndex];
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target) < 0.01f)
            {
                transform.position = target;
                currentTargetIndex++;
            }
        }
    }

    public void StartMoving(List<Vector3> movingPath)
    {
        path = movingPath;
        currentTargetIndex = 0;
        moving = true;
    }

    private void StopMoving()
    {
        moving = false;
        path.Clear();
        currentTargetIndex = 0;
    }

    private void Update()
    {
        Move();
    }
}

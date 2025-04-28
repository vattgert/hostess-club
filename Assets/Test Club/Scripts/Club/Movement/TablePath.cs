using System.Collections.Generic;
using UnityEngine;

public class TablePath : MonoBehaviour
{
    [SerializeField]
    private List<Transform> waypoints;
    private LinkedList<Transform> linkedWaypoints;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        linkedWaypoints = new LinkedList<Transform>(waypoints);
    }

    public LinkedList<Transform> PathOfPoints()
    {
        return linkedWaypoints;
    }
}

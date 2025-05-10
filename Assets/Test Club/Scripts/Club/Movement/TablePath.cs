using System.Collections.Generic;
using UnityEngine;

public class TablePath : MonoBehaviour
{
    [SerializeField]
    private List<Transform> customerWaypoints;
    private LinkedList<Transform> linkedCustomerWaypoints;
    [SerializeField]
    private List<Transform> hostWaypoints;
    private LinkedList<Transform> linkedHostWaypoints;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        linkedCustomerWaypoints = new LinkedList<Transform>(customerWaypoints);
        linkedHostWaypoints = new LinkedList<Transform>(hostWaypoints);
    }

    public LinkedList<Transform> CustomerPath()
    {
        return linkedCustomerWaypoints;
    }

    public LinkedList<Transform> HostPath()
    {
        return linkedHostWaypoints;
    }
}

using System.Collections.Generic;
using UnityEngine;

public class HostMovement : WaypointsMovement
{
    public void WalkToTable(GameObject table)
    {
        TableManager tm = table.GetComponent<TableManager>();
        LinkedList<Transform> way = table.GetComponent<TablePath>().HostPath();
        way.AddLast(tm.HostPlace());
        StartWalking(way);
    }

    protected override void OnArrivedAtFinalWaypoint(Transform arrival)
    {
        if (arrival.name == ComponentsNames.HostPlaceOnTable)
        {
            Debug.Log("Customer arrived");
            RaiseArrivalEvent(gameObject, arrival);
        }
    }

    private void Update()
    {
        Walk();
    }
}

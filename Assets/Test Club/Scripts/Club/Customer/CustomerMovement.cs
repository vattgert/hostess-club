using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerMovement : WaypointsMovement
{
    public void WalkToTable(GameObject table)
    {
        TableManager tm = table.GetComponent<TableManager>();
        LinkedList<Transform> way = table.GetComponent<TablePath>().CustomerPath();
        way.AddLast(tm.CustomerPlace());
        StartWalking(way);
    }

    protected override void OnArrivedAtFinalWaypoint(Transform arrival)
    {
        if (arrival.name == ComponentsNames.CustomerPlaceOnTable)
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

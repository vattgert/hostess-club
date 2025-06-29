using System.Collections.Generic;
using System.Linq;
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

    public void WalkFromTable(GameObject table)
    {
        TableManager tm = table.GetComponent<TableManager>();
        LinkedList<Transform> way = table.GetComponent<TablePath>().HostPath();
        way.AddLast(tm.HostPlace());
        LinkedList<Transform> reversedWay = new LinkedList<Transform>(way.Reverse());
        StartWalking(reversedWay);
    }

    private void Update()
    {
        Walk();
    }
}

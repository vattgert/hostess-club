using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomerMovement : WaypointsMovement
{
    public void WalkToTable(GameObject table)
    {
        TableManager tm = table.GetComponent<TableManager>();
        LinkedList<Transform> way = table.GetComponent<TablePath>().CustomerPath();
        way.AddLast(tm.CustomerPlace());
        StartWalking(way);
    }

    public void WalkFromTable(GameObject table)
    {
        TableManager tm = table.GetComponent<TableManager>();
        LinkedList<Transform> way = table.GetComponent<TablePath>().CustomerPath();
        way.AddLast(tm.CustomerPlace());
        LinkedList<Transform> reversedWay = new LinkedList<Transform>(way.Reverse()); 
        StartWalking(reversedWay);
    }

    private void Update()
    {
        Walk();
    }
}

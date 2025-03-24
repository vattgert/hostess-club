using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CustomerManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap club;
    private Stack<GameObject> customers;

    void Start()
    {
        customers = new Stack<GameObject>();
    }

    public Stack<GameObject> GetCustomers()
    {
        return customers;
    }

    public void GenerateCustomersPoolForShift() { 
        for(int i = 0; i < 10; i++)
        {
            Customer customer = new Customer(CustomerType.Poor);
            GameObject customerGO = new GameObject();
            CustomerBehavior cb = customerGO.AddComponent<CustomerBehavior>();
            cb.Initialize(customer);
            customers.Push(customerGO);
        }
        Debug.Log("Customer generated: " + customers.Count);
    }

    public void ClearShiftCustomers() {
        customers = null;
    }
}

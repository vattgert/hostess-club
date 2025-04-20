using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    private GameObject customersContainer;
    private Stack<GameObject> customers;

    void Awake()
    {
        customersContainer = new GameObject(ComponentsNames.CustomersContainer);
    }

    void Start()
    {
        customers = new Stack<GameObject>();
    }

    public Stack<GameObject> GetCustomers()
    {
        return customers;
    }

    private void SetCustomerInContainer(GameObject customer)
    {
        customer.transform.SetParent(customersContainer.transform, false);
    }

    private GameObject CreateCustomer()
    {
        Customer customer = new Customer(CustomerType.Poor);
        GameObject customerGO = new GameObject();
        CustomerBehavior cb = customerGO.AddComponent<CustomerBehavior>();
        cb.Initialize(customer);
        SetCustomerInContainer(customerGO);
        return customerGO;
    }

    public void GenerateCustomersPoolForShift() { 
        for(int i = 0; i < 10; i++)
        {
            customers.Push(CreateCustomer());
        }
        Debug.Log("Customer generated: " + customers.Count);
    }

    public void ClearCustomers() {
        customers.Clear();
        foreach (Transform child in customersContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}

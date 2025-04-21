using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    private GameObject customersContainer;
    [SerializeField]
    private GameObject customerPrefab;
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
        GameObject customerGo = new CustomerBuilder(customerPrefab)
            .SetCustomerData(customer)
            .SetActive(false)
            .Build();
        SetCustomerInContainer(customerGo);
        return customerGo;
    }

    public void GenerateCustomersPoolForShift() { 
        for(int i = 0; i < 1; i++)
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

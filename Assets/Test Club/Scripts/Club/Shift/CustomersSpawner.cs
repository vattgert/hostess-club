using System.Collections.Generic;
using UnityEngine;

public class CustomersSpawner : MonoBehaviour
{
    private GameObject customersContainer;
    [SerializeField]
    private GameObject customerPrefab;
    private TablesManager tablesManager;
    private Stack<GameObject> customers;

    void Awake()
    {
        tablesManager = gameObject.GetComponent<TablesManager>();
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
        tablesManager.SubscribeOnCharacterArrival(customerGo);
        return customerGo;
    }

    public void GenerateCustomersPoolForShift() { 
        for(int i = 0; i < 2; i++)
        {
            customers.Push(CreateCustomer());
        }
    }

    public void ClearCustomers() {
        customers.Clear();
        foreach (Transform child in customersContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}

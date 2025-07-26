using System.Collections.Generic;
using UnityEngine;
using Characters;

public class CustomersSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject customerPrefab;
    [SerializeField]
    private TraitsLibrary traitsLibrary;
    [SerializeField]
    private int spawnPerShift;
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
        customer.SetAppearancePreference(traitsLibrary.GetRandomTrait(TraitCategory.Appearance));
        customer.SetPersonalityPreference(traitsLibrary.GetRandomTrait(TraitCategory.Personality));
        GameObject customerGo = new CustomerBuilder(customerPrefab)
            .SetCustomerData(customer)
            .SetActive(false)
            .Build();
        SetCustomerInContainer(customerGo);
        return customerGo;
    }

    public void GenerateCustomersPoolForShift() { 
        for(int i = 0; i < spawnPerShift; i++)
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

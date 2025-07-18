using Characters;
using UnityEngine;


public interface ICustomerBuilder
{
    ICustomerBuilder SetCustomerData(Customer customer);
    ICustomerBuilder SetActive(bool active);
    GameObject Build();
}

public class CustomerBuilder : ICustomerBuilder
{
    private GameObject _customer;

    public CustomerBuilder(GameObject prefab)
    {
        _customer = GameObject.Instantiate(prefab);
        var behavior = _customer.GetComponent<CustomerBehavior>();
    }

    public ICustomerBuilder SetCustomerData(Customer customer)
    {
        _customer.GetComponent<CustomerBehavior>().Initialize(customer);
        return this;
    }

    public ICustomerBuilder SetActive(bool active)
    {
        _customer.SetActive(false);
        return this;
    }

    public GameObject Build()
    {
        GameObject result = this._customer;
        return result;
    }
}

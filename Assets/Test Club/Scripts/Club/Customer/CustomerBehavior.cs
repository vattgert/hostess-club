using UnityEngine;

public class CustomerBehavior : MonoBehaviour
{
    private Customer customer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Initialize(Customer customer)
    {
        this.customer = customer;
        // Now you can use _customer data to set up visuals, UI, etc.
        // For example:
        // nameText.text = _customer.Name;
        // ageText.text = _customer.Age.ToString();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Customer GetCustomer()
    {
        return this.customer;
    }
}

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

    public void CreateCustomerSprite()
    {
        GameObject customer = gameObject;
        Sprite triangleSprite = Resources.Load<Sprite>("Triangle");
        if (triangleSprite == null)
        {
            Debug.LogError("Failed to load Triangle sprite!");
            return;
        }
        SpriteRenderer sr = customer.AddComponent<SpriteRenderer>();
        sr.sprite = triangleSprite;
        sr.sortingOrder = 1;
        sr.color = new Color32(1, 125, 243, 255);
        sr.transform.rotation = Quaternion.Euler(0, 0, 180);
    }

    public bool NextChargeOverflow()
    {
        return (customer.Budget - customer.ChargeAmount) <= 0;
    }

    public int Charge()
    {
        customer.Budget = customer.Budget - customer.ChargeAmount;
        return customer.ChargeAmount;
    }

    public Customer GetCustomer()
    {
        return this.customer;
    }
}

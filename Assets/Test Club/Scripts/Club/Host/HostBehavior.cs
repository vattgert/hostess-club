using UnityEngine;

public class HostBehavior : MonoBehaviour
{
    Host host;
    public void Initialize(Host host)
    {
        this.host = host;
        // Now you can use _customer data to set up visuals, UI, etc.
        // For example:
        // nameText.text = _customer.Name;
        // ageText.text = _customer.Age.ToString();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Host GetHost()
    {
        return this.host;
    }
}

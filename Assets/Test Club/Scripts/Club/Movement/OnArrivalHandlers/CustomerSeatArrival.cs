using UnityEngine;

public class CustomerSeatArrival : MonoBehaviour, IOnArrivalHandler
{
    [SerializeField]
    private HostAndCustomerSession session;

    public void Arrived(GameObject customer, Transform arrival)
    {
        if (customer.CompareTag(Tags.Customer))
        {
            Debug.Log("Customer arrived to the table seat");
            session.AssignCustomer(customer);
        }
    }
}

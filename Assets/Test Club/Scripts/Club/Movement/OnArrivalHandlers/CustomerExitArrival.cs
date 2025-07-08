using UnityEngine;

public class CustomerExitArrival : MonoBehaviour, IOnArrivalHandler
{

    public void Arrived(GameObject customer, Transform arrival)
    {
        if (customer.CompareTag(Tags.Customer))
        {
            Debug.Log("Customer arrived to customer exit");
            customer.SetActive(false);
        }
    }
}

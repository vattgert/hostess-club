using UnityEngine;

public class TableManager : MonoBehaviour
{
    [SerializeField]
    private Transform customerPlace;
    [SerializeField]
    private Transform hostPlace;

    public Transform CustomerPlace()
    {
        return customerPlace;
    }

    public Transform HostPlace()
    {
        return hostPlace;
    }

    public void SitHostOnTable(GameObject host)
    {
        host.transform.SetParent(hostPlace.transform);
        host.transform.position = hostPlace.position;
    }

    public void SitCustomerOnTable(GameObject customer)
    {
        customer.transform.SetParent(customerPlace.transform);
        customer.transform.position = customerPlace.position;
    }

    public void RemoveHostFromPlace()
    {
        foreach (Transform child in hostPlace)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void RemoveCustomerFromPlace()
    {
        foreach (Transform child in customerPlace)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}

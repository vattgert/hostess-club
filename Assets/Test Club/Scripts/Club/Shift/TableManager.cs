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

    public void RemoveHostFromPlace()
    {
        foreach (Transform child in hostPlace)
        {
            Destroy(child.gameObject);
        }
    }

    public void RemoveCustomerFromPlace()
    {
        foreach (Transform child in customerPlace)
        {
            Destroy(child.gameObject);
        }
    }
}

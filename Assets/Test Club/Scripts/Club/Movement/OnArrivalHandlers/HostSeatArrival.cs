using UnityEngine;

public class HostSeatArrival : MonoBehaviour, IOnArrivalHandler
{
    [SerializeField]
    private HostAndCustomerSession session;

    public void Arrived(GameObject host, Transform arrival)
    {
        if (host.CompareTag(Tags.Host))
        {
            Debug.Log("Host arrived to the table seat");
            session.AssignHost(host);
        }
    }
}
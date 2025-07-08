using UnityEngine;

public class HostSpawnPointArrival : MonoBehaviour, IOnArrivalHandler
{
    public void Arrived(GameObject host, Transform arrival)
    {
        if (host.CompareTag(Tags.Host))
        {
            Debug.Log("Host arrived to host spawn point");
            host.SetActive(false);
        }
    }
}
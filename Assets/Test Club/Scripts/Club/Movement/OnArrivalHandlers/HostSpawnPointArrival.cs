using UnityEngine;

public class HostSpawnPointArrival : MonoBehaviour, IOnArrivalHandler
{
    [SerializeField]
    ShiftHostsUI hostsUI;

    public void Arrived(GameObject host, Transform arrival)
    {
        if (host.CompareTag(Tags.Host))
        {
            Debug.Log("Host arrived to host spawn point");
            host.SetActive(false);
            hostsUI.AddHostToList(host);
        }
    }
}
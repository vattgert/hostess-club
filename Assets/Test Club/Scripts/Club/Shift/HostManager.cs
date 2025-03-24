using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HostManager : MonoBehaviour
{
    private List<GameObject> hosts;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hosts = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateShiftHosts()
    {
        for (int i = 0; i < 2; i++)
        {
            Host host = new Host();
            GameObject hostGameObject = new GameObject();
            hostGameObject.name = host.Name;
            HostBehavior hostBehavior = hostGameObject.AddComponent<HostBehavior>();
            hostBehavior.Initialize(host);
            hosts.Add(hostGameObject);
        }
    }

    public List<GameObject> GetShiftHosts()
    {
        return hosts;
    }

    public bool HasAvailableHost()
    {
        return hosts.Count > 0;
    }

    public void RemoveHost(GameObject host)
    {
        hosts.Remove(host);
    }
}

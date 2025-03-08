using System.Collections.Generic;
using UnityEngine;

public class HostManager : MonoBehaviour
{
    private List<GameObject> hosts;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.hosts = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateHostsForShift()
    {
        for (int i = 0; i < 2; i++)
        {
            Host host = new Host();
            GameObject hostGameObject = new GameObject();
            HostBehavior hostBehavior = hostGameObject.AddComponent<HostBehavior>();
            hostBehavior.Initialize(host);
            this.hosts.Add(hostGameObject);
        }
    }

    public List<GameObject> GetHosts()
    {
        return this.hosts;
    }

    public void ClearShiftHosts()
    {
        this.hosts = null;
    }
}

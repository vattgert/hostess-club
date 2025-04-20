using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HostManager : MonoBehaviour
{
    private GameObject hostsContainer;
    private List<GameObject> hosts;

    private void Awake()
    {
        hostsContainer  = new GameObject(ComponentsNames.HostsContainer);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hosts = new List<GameObject>();
    }

    private void SetHostInContainer(GameObject host)
    {
        host.transform.SetParent(hostsContainer.transform, false);
    }

    private GameObject CreateHost()
    {
        Host host = new Host();
        GameObject hostGameObject = new GameObject();
        hostGameObject.name = host.Name;
        HostBehavior hostBehavior = hostGameObject.AddComponent<HostBehavior>();
        hostBehavior.Initialize(host);
        SetHostInContainer(hostGameObject);
        return hostGameObject;
    }

    public void GenerateShiftHosts()
    {
        for (int i = 0; i < 2; i++)
        {
            
            hosts.Add(CreateHost());
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

    public void ClearHosts()
    {
        hosts.Clear();
        foreach (Transform child in hostsContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HostManager : MonoBehaviour
{
    private GameObject hostsContainer;
    [SerializeField]
    private GameObject hostPrefab;
    private List<GameObject> hosts;
    private TablesManager tablesManager;
    [SerializeField]
    private Transform hostSpawnPoint;
    [SerializeField]
    private TilemapGridBuilder gridBuilder;

    private void Awake()
    {
        tablesManager = gameObject.GetComponent<TablesManager>();
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
        GameObject hostGo = new HostBuilder(hostPrefab)
            .SetHostData(host)
            .SetActive(false)
            .Build();
        SetHostInContainer(hostGo);
        return hostGo;
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

    private void SpawnHostNearHostSpot(GameObject host)
    {
        host.transform.position = hostSpawnPoint.position;
    }

    private void WalkHostToAssignedTable(GameObject host, HostAndCustomerSession session)
    {
        GameObject table = session.gameObject;
        TableManager tm = table.GetComponent<TableManager>();
        Transform hostSeat = tm.HostSeat();
        CharacterMovementController.Instance.AStarMoveTo(host, hostSeat, true);
    }

    public void CallHost(GameObject host, HostAndCustomerSession session)
    {
        SpawnHostNearHostSpot(host);
        host.SetActive(true);
        host.GetComponent<HostBehavior>().SetState(HostState.Entering);
        WalkHostToAssignedTable(host, session);
    }
}

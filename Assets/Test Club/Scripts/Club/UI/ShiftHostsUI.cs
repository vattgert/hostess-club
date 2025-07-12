using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShiftHostsUI : MonoBehaviour
{
    public GameObject hostListItemPrefab;
    private Transform hostUIContainer;
    private List<GameObject> hosts;
    private GameObject selectedHost;

    private void Awake()
    {
        hostUIContainer = gameObject.transform;
    }

    public void SetHostsForShiftList(List<GameObject> hosts)
    {
        this.hosts = hosts;
        PopulateHostUIList();
    }

    private void CreateHostListItem(GameObject hostGo)
    {
        HostBehavior hostBehavior = hostGo.GetComponent<HostBehavior>();
        if (hostBehavior != null)
        {
            Host host = hostBehavior.Host;
            GameObject uiEntry = Instantiate(hostListItemPrefab, hostUIContainer);
            HostEntryUI hostEntry = uiEntry.GetComponent<HostEntryUI>();
            // Assuming the prefab has a Text component to display the hostess's name.
            if (hostEntry != null)
            {
                hostEntry.SetHost(hostGo);
            }
            TextMeshProUGUI hostNameText = uiEntry.GetComponentInChildren<TextMeshProUGUI>();
            if (hostNameText != null)
            {
                hostNameText.text = host.Name;
            }
        }
    }

    private void PopulateHostUIList()
    {
        // Clear any existing UI entries (optional)
        foreach (Transform child in hostUIContainer)
        {
            Destroy(child.gameObject);
        }

        // Instantiate a UI entry for each hostess in the list.
        foreach (GameObject hostGo in hosts)
        {
            CreateHostListItem(hostGo);
        }
    }

    public void AddHostToList(GameObject hostGo)
    {
        if (!hosts.Contains(hostGo))
        {
            hosts.Add(hostGo);
            Debug.Log("Add  host list item to UI");
            CreateHostListItem(hostGo);
        }
    }

    public void RemoveHostFromList(GameObject host)
    {
        if (hosts.Contains(host))
        {
            hosts.Remove(host);

            // Find and destroy only the corresponding UI entry
            foreach (Transform child in hostUIContainer)
            {
                HostEntryUI hostEntry = child.GetComponent<HostEntryUI>();
                if (hostEntry != null && hostEntry.GetHost() == host)
                {
                    Debug.Log("Destroy host list item from UI");
                    Destroy(child.gameObject);
                    break; // Exit loop after removing the first matching entry
                }
            }
        }
    }

    public void SelectHost(GameObject host)
    {
        selectedHost = host;
    }

    public GameObject SelectedHost()
    {
        return selectedHost;
    }

    public void ClearSelection()
    {
        selectedHost = null;
    }
}
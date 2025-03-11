using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShiftHostsUI : MonoBehaviour
{
    private TablesManager tablesManager;
    // Prefab for a single hostess entry, assign in the Inspector.
    public GameObject hostListItemPrefab;

    // The container (a UI panel) that has the Vertical Layout Group component.
    private Transform hostUIContainer;

    // The list of hostesses to display.
    private List<GameObject> hosts;

    private void Awake()
    {
        this.tablesManager = FindAnyObjectByType<TablesManager>();
        this.hostUIContainer = gameObject.transform;
        this.tablesManager.OnHostAssigned += this.HandleOnHostAssigned;
        this.tablesManager.OnSessionFinished += this.HandleOnSessionFinished;
    }

    public void SetHostsForShiftList(List<GameObject> hosts)
    {
        this.hosts = hosts;
        this.PopulateHostUIList();
    }

    private void CreateHostListItem(GameObject hostGo)
    {
        HostBehavior hostBehavior = hostGo.GetComponent<HostBehavior>();
        if (hostBehavior != null)
        {
            Host host = hostBehavior.GetHost();
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

    private void HandleOnHostAssigned(GameObject host)
    {
        host.GetComponent<HostBehavior>().Activate();
        this.RemoveHostFromUIList(host);
    }

    private void HandleOnSessionFinished(GameObject host)
    {
        this.AddHostToUIList(host);
    }

    private void PopulateHostUIList()
    {
        // Clear any existing UI entries (optional)
        foreach (Transform child in hostUIContainer)
        {
            Destroy(child.gameObject);
        }

        // Instantiate a UI entry for each hostess in the list.
        foreach (GameObject hostGo in this.hosts)
        {
            this.CreateHostListItem(hostGo);
        }
    }

    private void AddHostToUIList(GameObject hostGo)
    {
        if (!hosts.Contains(hostGo))
        {
            hosts.Add(hostGo);
            Debug.Log("Add  host list item to UI");
            this.CreateHostListItem(hostGo);
        }
    }

    private void RemoveHostFromUIList(GameObject host)
    {
        if (this.hosts.Contains(host))
        {
            this.hosts.Remove(host);

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
}
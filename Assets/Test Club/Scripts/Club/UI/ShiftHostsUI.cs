using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class ShiftHostsUI : MonoBehaviour
{
    // Prefab for a single hostess entry, assign in the Inspector.
    public GameObject hostListItemPrefab;

    // The container (a UI panel) that has the Vertical Layout Group component.
    private Transform hostUIContainer;

    // The list of hostesses to display.
    private List<GameObject> hosts;

    private void Awake()
    {
        this.hostUIContainer = gameObject.transform;
    }

    public void SetHostsList(List<GameObject> hosts)
    {
        this.hosts = hosts;
    }

    public void PopulateHostessUIList()
    {
        // Clear any existing UI entries (optional)
        foreach (Transform child in hostUIContainer)
        {
            Destroy(child.gameObject);
        }

        // Instantiate a UI entry for each hostess in the list.
        foreach (GameObject hostGo in hosts)
        {
            HostBehavior hostBehavior = hostGo.GetComponent<HostBehavior>();
            if(hostBehavior != null)
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
                    Debug.Log("Host name " + host.Name);
                    hostNameText.text = host.Name;
                }
            }
        }
    }
}
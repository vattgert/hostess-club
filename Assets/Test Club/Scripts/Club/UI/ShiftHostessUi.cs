using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShiftHostsUI : MonoBehaviour
{
    // Prefab for a single hostess entry, assign in the Inspector.
    public GameObject hostsUIPrefab;

    // The container (a UI panel) that has the Vertical Layout Group component.
    public Transform hostessUIContainer;

    // The list of hostesses to display.
    private List<GameObject> hosts;

    public void SetHostsList(List<GameObject> hosts)
    {
        this.hosts = hosts;
    }

    public void PopulateHostessUIList()
    {
        // Clear any existing UI entries (optional)
        foreach (Transform child in hostessUIContainer)
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
                GameObject uiEntry = Instantiate(hostsUIPrefab, hostessUIContainer);
                // Assuming the prefab has a Text component to display the hostess's name.
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
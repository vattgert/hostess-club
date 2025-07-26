using UnityEngine;

public class HostEntryUI : MonoBehaviour
{
    private GameObject host;
    [SerializeField]
    private GameObject strikethrough;
 
    public bool Available()
    {
        return host != null && !host.GetComponent<HostBehavior>().Host.Exhausted();
    }

    public void SetHost(GameObject host)
    {
        this.host = host;
        strikethrough.SetActive(!Available());
    }

    public GameObject GetHost()
    {
        return this.host;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

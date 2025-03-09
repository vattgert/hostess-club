using UnityEngine;

public class HostEntryUI : MonoBehaviour
{
    private GameObject host;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetHost(GameObject host)
    {
        this.host = host;
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

using UnityEngine;

public class StuffOnlyZone : MonoBehaviour
{
    [SerializeField]
    ShiftHostsUI shiftHostsUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Host"))
        {
            Debug.Log("Host entered stuff only zone");
            SubscribeOnStartPointArrival(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Host"))
        {
            Debug.Log("Host exited stuff only zone");
            UnsubscribeOnStartPointArrival(other.gameObject);
        }
    }

    private void SetHostOnWaiting(GameObject host, Transform arrival)
    {
        bool hostStartPoint = arrival.name == ComponentsNames.HostStartWaypoint;
        HostBehavior hb = host.GetComponent<HostBehavior>();
        if (hostStartPoint && hb != null && hb.CurrentState == HostState.Leaving)
        {
            host.SetActive(false);
            shiftHostsUI.AddHostToList(host);
        }
    }

    private void SubscribeOnStartPointArrival(GameObject character)
    {
        character.GetComponent<WaypointsMovement>().OnArrivedAtDestination += SetHostOnWaiting;
    }

    private void UnsubscribeOnStartPointArrival(GameObject character)
    {
        character.GetComponent<WaypointsMovement>().OnArrivedAtDestination -= SetHostOnWaiting;
    }
}

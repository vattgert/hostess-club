using UnityEngine;

public class TableManager : MonoBehaviour
{
    [SerializeField]
    private Transform customerSeat;
    [SerializeField]
    private Transform hostSeat;
    [SerializeField]
    private HostAndCustomerSession session;

    public Transform CustomerSeat()
    {
        return customerSeat;
    }

    public Transform HostSeat()
    {
        return hostSeat;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Customer"))
        {
            Debug.Log("Customer entered tables's customer seat");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Customer"))
        {
            Debug.Log("Customer left tables's customer seat");
        }
    }
}

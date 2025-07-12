using UnityEngine;

public class StuffOnlyZone : MonoBehaviour
{
    [SerializeField]
    ShiftHostsUI shiftHostsUI;
    [SerializeField]
    Transform hostSpawnPoint;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.Host))
        {
            Debug.Log("Host entered stuff only zone");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(Tags.Host))
        {
            Debug.Log("Host exited stuff only zone");
        }
    }

    public Transform HostSpawnPoint()
    {
        return hostSpawnPoint;
    }
}

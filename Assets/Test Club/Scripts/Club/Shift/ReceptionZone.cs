using System.Collections;
using UnityEngine;

public class ReceptionZone : MonoBehaviour
{
    [SerializeField]
    private CustomerInvitationManager customerInvitationManager;
    [SerializeField]
    private ShiftManager shiftManager;

    private bool playerInZone = false;
    private bool customerInZone = false;

    void Start()
    {
        customerInvitationManager.OnCustomerInvited += ProcessCustomerOnReception;
    }

    private void OnDestroy()
    {
        customerInvitationManager.OnCustomerInvited -= ProcessCustomerOnReception;
    }

    private void ProcessCustomerOnReception(GameObject customer)
    {
        Debug.Log("Process customer when appeared in zone");
        Collider2D inviteTrigger = gameObject.GetComponent<Collider2D>();
        Collider2D customerCollider = customer.GetComponent<Collider2D>();
        bool collidersIntesect = inviteTrigger.bounds.Intersects(customerCollider.bounds);
        bool receptionColliderContains = inviteTrigger.bounds.Contains(customerCollider.bounds.center);
        if (shiftManager.ShiftActive() && (collidersIntesect || receptionColliderContains))
        {
            customerInZone = true;
            Debug.Log("Customer entered reception zone");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (shiftManager.ShiftActive())
        {
            if (other.CompareTag("Player"))
            {
                playerInZone = true;
                Debug.Log("Player entered reception zone");
            }

            if (other.CompareTag("Customer"))
            {
                customerInZone = true;
                Debug.Log("Customer entered reception zone");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            Debug.Log("Player exited reception zone");
        }

        if (other.CompareTag("Customer"))
        {
            customerInZone = false;
            Debug.Log("Customer exited reception zone");
        }
    }

    private void Update()
    {
        if (shiftManager.ShiftActive() && playerInZone && customerInZone)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("F pressed — initiate table selection or next step");
                // TODO: Call whatever comes next
            }
        }
    }
}

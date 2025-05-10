using System.Collections;
using UnityEngine;

public class ReceptionZone : MonoBehaviour
{
    [SerializeField]
    private CustomerInvitationManager customerInvitationManager;

    [SerializeField]
    private ShiftManager shiftManager;

    [SerializeField]
    private TablesManager tablesManager;


    private bool playerInZone = false;
    private bool customerInZone = false;
    private GameObject customer;

    private HoldInput FInputHold;

    private void Awake()
    {
        FInputHold = new HoldInput(KeyCode.F);
    }

    void Start()
    {
     
    }

    private void SetCustomer(GameObject c)
    {
        customer = c;
        customerInZone = true;
    }

    private void ResetCustomer()
    {
        customerInZone = false;
        customer = null;
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
                SetCustomer(other.gameObject);
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
            ResetCustomer();
            Debug.Log("Customer exited reception zone");
        }
    }

    private void OnReceptionInputHold()
    {
        if (customerInZone)
        {
            tablesManager.HighlightFreeTables();
        }
    }

    private void Update()
    {
        if (shiftManager.ShiftActive() && playerInZone && customerInZone)
        {
            FInputHold.Hold(OnReceptionInputHold);
        }
    }

    public void WalkCustomerToSelectedTable(HostAndCustomerSession session)
    {
        GameObject table = session.gameObject;
        customer.GetComponent<CustomerMovement>().WalkToTable(table);
    }
}

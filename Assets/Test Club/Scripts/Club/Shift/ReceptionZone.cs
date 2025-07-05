using UnityEngine;
using UnityEngine.TextCore.Text;

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

    private void SetCustomer(GameObject c)
    {
        customer = c;
        customerInZone = true;
        SubscribeOnEntranceArrival(customer);
    }

    private void ResetCustomer()
    {
        UnsubscribeOnEntranceArrival(customer);
        customerInZone = false;
        customer = null;
    }

    private void WalkCustomerAway(GameObject customer, Transform arrival)
    {
        bool customerOnEntrancePoint = arrival.name == ComponentsNames.CustomerStartWaypoint;
        CustomerBehavior cb = customer.GetComponent<CustomerBehavior>();
        if (customerOnEntrancePoint && cb != null && cb.FinishedSession)
        {
            customer.SetActive(false);
        }
    }

    private void SubscribeOnEntranceArrival(GameObject character)
    {
        character.GetComponent<WaypointsMovement>().OnArrivedAtDestination += WalkCustomerAway;
    }

    private void UnsubscribeOnEntranceArrival(GameObject character)
    {
        character.GetComponent<WaypointsMovement>().OnArrivedAtDestination -= WalkCustomerAway;
    }

    private void OnTriggerEnter2D(Collider2D other)
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
        tablesManager.HighlightFreeTables();
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
        customer.GetComponent<CustomerBehavior>().SetState(CustomerState.AssignedMovingToTable);
        customer.GetComponent<CustomerMovement>().WalkToTable(table);
    }
}

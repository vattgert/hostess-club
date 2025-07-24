using Characters;
using System.Collections.Generic;
using UnityEngine;

public class ReceptionZone : MonoBehaviour
{
    [SerializeField]
    private CustomerInvitationManager customerInvitationManager;
    [SerializeField]
    private ShiftManager shiftManager;
    [SerializeField]
    private TablesManager tablesManager;
    [SerializeField]
    private Transform customerExit;

    private bool playerInZone = false;
    private Queue<GameObject> customersInZone = new Queue<GameObject>();
    private GameObject customer;

    private HoldInput FInputHold;

    private void Awake()
    {
        FInputHold = new HoldInput(KeyCode.F);
    }

    private void SetCustomer(GameObject c)
    {
        CustomerBehavior cb = c.GetComponent<CustomerBehavior>();
        if(cb != null && cb.CurrentState == CustomerState.Entering)
        {
            customersInZone.Enqueue(c);
        } else
        {
            Debug.Log($"Customer either does not exist or entrance or has current status {cb.CurrentState}");
        }
    }

    private void ResetCustomer(GameObject c)
    {
        if(c == customersInZone.Peek())
        {
            customersInZone.Dequeue();
            customer = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.Player))
        {
            playerInZone = true;
            Debug.Log("Player entered reception zone");
        }

        if (other.CompareTag(Tags.Customer))
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
            ResetCustomer(other.gameObject);
            Debug.Log("Customer exited reception zone");
        }
    }

    private void OnReceptionInputHold()
    {
        Debug.Log("Shift active to invite: " + shiftManager.ShiftActive());
        if (shiftManager.ShiftActive() && playerInZone)
        {
            tablesManager.HighlightFreeTables();
            customer = customersInZone.Peek();
        }
    }

    private void Update()
    {
        FInputHold.Hold(OnReceptionInputHold);
    }

    private void OnEnable()
    {
        if (FInputHold == null)
        {
            FInputHold = new HoldInput(KeyCode.F);
        }
    }

    public void MoveCustomerToSelectedTable(HostAndCustomerSession session)
    {
        if(customer == null)
        {
            Debug.Log("Cannot move customer to the selected table. Customer is null");
            return;
        }
        GameObject table = session.gameObject;
        TableManager tm = table.GetComponent<TableManager>();
        Transform customerSeat = tm.CustomerSeat();
        customer.GetComponent<CustomerBehavior>().SetState(CustomerState.AssignedMovingToTable);
        CharacterMovementController.Instance.AStarMoveTo(customer, customerSeat, true);
    }

    public Transform GetCustomerExit()
    {
        return customerExit;
    }
}

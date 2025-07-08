using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.Tilemaps;

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
    }

    private void ResetCustomer()
    {
        customerInZone = false;
        customer = null;
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

    public void MoveCustomerToSelectedTable(HostAndCustomerSession session)
    {
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

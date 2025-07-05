using TMPro;
using UnityEngine;

public enum CustomerState
{
    Entering,
    AssignedMovingToTable,
    Seated,
    InSession,
    Leaving
}

public class CustomerBehavior : MonoBehaviour
{
    public CustomerState CurrentState { get; private set; }
    public Customer Customer { get; private set; }

    [SerializeField]
    public Canvas countdownCanvas;
    public bool FinishedSession = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        SetWaitingCountdownCanvas();
    }

    public void Initialize(Customer customer)
    {
        this.Customer = customer;
    }

    public void SetState(CustomerState newState)
    {
        if (CurrentState != newState)
        {
            CurrentState = newState;
            Debug.Log($"Customer {this.name} changed state to {CurrentState}");
        }
    }

    private void SetWaitingCountdownCanvas()
    {
        countdownCanvas.transform.localPosition = new Vector3(0, -0.5f, 0);
        countdownCanvas.transform.localScale = Vector3.one * 0.01f; // scale down if necessary
    }

    public void ActivateWaitingUI()
    {
        if(countdownCanvas != null)
        {
            countdownCanvas.gameObject.SetActive(true);
        }
    }

    public void UpdateWaitingCountdown(float time)
    {
        if (countdownCanvas != null && countdownCanvas.gameObject.activeSelf == true)
        {
            // (Optional) store reference to countdown text for future updates
            TextMeshProUGUI text = countdownCanvas.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = time.ToString(); // or whatever your logic needs
            }
        }
    }

    public void StopWaiting()
    {
        if (countdownCanvas != null)
        {
            countdownCanvas.gameObject.SetActive(false);
        }
    }

    public bool NextChargeOverflow()
    {
        return (Customer.Budget - Customer.ChargeAmount) <= 0;
    }

    public int Charge()
    {
        Customer.Budget = Customer.Budget - Customer.ChargeAmount;
        return Customer.ChargeAmount;
    }
}

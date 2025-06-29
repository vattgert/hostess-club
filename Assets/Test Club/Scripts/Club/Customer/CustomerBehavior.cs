using TMPro;
using UnityEngine;

public class CustomerBehavior : MonoBehaviour
{
    private Customer customer;
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
        this.customer = customer;
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
        return (customer.Budget - customer.ChargeAmount) <= 0;
    }

    public int Charge()
    {
        customer.Budget = customer.Budget - customer.ChargeAmount;
        return customer.ChargeAmount;
    }

    public Customer GetCustomer()
    {
        return this.customer;
    }
}

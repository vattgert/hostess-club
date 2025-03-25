using TMPro;
using UnityEngine;

public class CustomerBehavior : MonoBehaviour
{
    private Customer customer;
    private GameObject countdownCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Initialize(Customer customer)
    {
        this.customer = customer;
        // Now you can use _customer data to set up visuals, UI, etc.
        // For example:
        // nameText.text = _customer.Name;
        // ageText.text = _customer.Age.ToString();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetWaitingCountdownCanvas(Transform parent)
    {
        // Load countdown prefab
        GameObject canvasInstance = Instantiate(Resources.Load<GameObject>(ComponentsNames.CustomerWaitingCountdown));
        canvasInstance.SetActive(false);
        canvasInstance.transform.SetParent(parent);

        // Position it above the triangle (adjust height as needed)
        canvasInstance.transform.localPosition = new Vector3(0, -0.5f, 0);
        canvasInstance.transform.localScale = Vector3.one * 0.01f; // scale down if necessary

        countdownCanvas = canvasInstance;
    }

    private void CreateCustomerSprite()
    {
        GameObject customer = gameObject;
        Sprite triangleSprite = Resources.Load<Sprite>("Triangle");
        if (triangleSprite == null)
        {
            Debug.LogError("Failed to load Triangle sprite!");
            return;
        }
        SpriteRenderer sr = customer.AddComponent<SpriteRenderer>();
        sr.sprite = triangleSprite;
        sr.sortingOrder = 1;
        sr.color = new Color32(1, 125, 243, 255);
        sr.transform.rotation = Quaternion.Euler(0, 0, 180);
    }

    public void CreateCustomer()
    {
        CreateCustomerSprite();
        SetWaitingCountdownCanvas(gameObject.transform);
    }

    public void StartWaiting()
    {
        if(countdownCanvas != null)
        {
            countdownCanvas.SetActive(true);
        }
    }

    public void UpdateWaitingCountdown(float time)
    {
        if (countdownCanvas != null && countdownCanvas.activeSelf == true)
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
            countdownCanvas.SetActive(false);
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

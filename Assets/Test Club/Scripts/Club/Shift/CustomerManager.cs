using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CustomerManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap club;
    private int timeBetweenCustomers = 20;
    private float lastTriggeredTime;
    private Stack<GameObject> customers;
    private ShiftTimer shiftTimer;
    private TablesManager tablesManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public event Action<GameObject> OnCustomerEntered;
    void Start()
    {
        this.shiftTimer = gameObject.GetComponent<ShiftTimer>();
        this.tablesManager = gameObject.GetComponent<TablesManager>();
        lastTriggeredTime = this.shiftTimer.ShitDuration();
        customers = new Stack<GameObject>();
        this.shiftTimer.OnShiftTimerUpdate += this.ServeCustomer;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Stack<GameObject> GetCustomers()
    {
        return this.customers;
    }

    private void FormCustomerSprite(GameObject customer)
    {
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

    public void GenerateCustomersPoolForShift() { 
        for(int i = 0; i < 10; i++)
        {
            Customer customer = new Customer(CustomerType.Poor);
            GameObject customerGO = new GameObject();
            CustomerBehavior cb = customerGO.AddComponent<CustomerBehavior>();
            cb.Initialize(customer);
            customers.Push(customerGO);
        }
    }

    private void InviteCustomer(GameObject customer)
    {
        this.FormCustomerSprite(customer);  
        this.OnCustomerEntered.Invoke(customer);
    }

    private void ServeCustomer(float timeLeft)
    {
        int currentTime = Mathf.FloorToInt(timeLeft);

        // Check if timeLeft is an exact multiple of 20 and ensure we haven't already triggered for this value.
        if (currentTime % timeBetweenCustomers == 0 && currentTime != this.lastTriggeredTime)
        {
            this.lastTriggeredTime = currentTime;
            Debug.Log("20-second mark reached. Time left: " + currentTime);
            // Insert the code you want to execute every 20 seconds here.
            Debug.Log("Time comparison worked");
            if (this.customers.Count > 0 && this.tablesManager.HasFreeTables())
            {
                Debug.Log("We have more than 0 customers in queue and there are free tables!");
                GameObject customer = this.customers.Pop();
                if (customer != null)
                {
                    Debug.Log("Invite customer");
                    this.InviteCustomer(customer);

                } 
            }
        }
    }

    public void ClearShiftCustomers() {
        this.customers = null;
    }
}

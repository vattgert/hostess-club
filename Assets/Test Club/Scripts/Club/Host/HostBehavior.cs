using UnityEngine;

public class HostBehavior : MonoBehaviour
{
    Host host;
    public void Initialize(Host host)
    {
        this.host = host;
        // Now you can use _customer data to set up visuals, UI, etc.
        // For example:
        // nameText.text = _customer.Name;
        // ageText.text = _customer.Age.ToString();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       this.FormHostSprite(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Host GetHost()
    {
        return this.host;
    }

    private void FormHostSprite(GameObject host)
    {
        Sprite triangleSprite = Resources.Load<Sprite>("Triangle");
        if (triangleSprite == null)
        {
            Debug.LogError("Failed to load Triangle sprite!");
            return;
        }
        SpriteRenderer sr = host.AddComponent<SpriteRenderer>();
        sr.sprite = triangleSprite;
        sr.sortingOrder = 1;
        sr.color = new Color32(255, 192, 203, 255);
    }
}

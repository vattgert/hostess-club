using UnityEngine;

public class HostBehavior : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Host host;
    public void Initialize(Host host)
    {
        this.host = host;
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public Host GetHost()
    {
        return this.host;
    }
}

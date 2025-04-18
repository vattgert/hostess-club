using UnityEngine;
using UnityEngine.UI;

public class TablePanelOverlayPulseTrigger : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private Image pulseOverlay;
    private readonly Color Transparent = new Color(0f, 0f, 0f, 0f); // same purple, 0 alpha

    void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator)
        {
            animator.enabled = false;
        }
    }

    public void PlayPulse()
    {
        animator.enabled = true;
        animator.Play("TablePanelPulseOverlay", -1, 0f); // Restart from beginning
    }

    public void StopPulse()
    {
        pulseOverlay.color = Transparent;
        animator.enabled = false;
    }
}

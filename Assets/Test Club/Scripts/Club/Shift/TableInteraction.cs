using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class TableInteraction : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color normalColor;
    private Color highlightColor = Color.yellow;
    public bool isBusy = false;
    public bool isHighlighted = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        this.normalColor = spriteRenderer.color;
    }

    public void SetHighlight(bool highlight)
    {
        isHighlighted = highlight;
        if (spriteRenderer != null)
        {
            this.spriteRenderer.color = highlight ? highlightColor : normalColor;
        }
    }
}

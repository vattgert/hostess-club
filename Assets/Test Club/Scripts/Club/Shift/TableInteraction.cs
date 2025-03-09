using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class TableInteraction : MonoBehaviour, IPointerClickHandler
{
    private TablesManager tablesManager;
    private SpriteRenderer spriteRenderer;
    private Color normalColor;
    private Color highlightColor = Color.yellow;
    public bool isHighlighted = false;

    private void Awake()
    {
        this.tablesManager = FindFirstObjectByType<TablesManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        this.normalColor = spriteRenderer.color;
    }

    public void SetHighlight(bool highlight)
    {
        this.isHighlighted = highlight;
        if (spriteRenderer != null)
        {
            this.spriteRenderer.color = highlight ? highlightColor : normalColor;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked on table");
        this.tablesManager.HighlightedTableClicked(this);
    }
}

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HostEntryHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Assign a "select" cursor texture in the Inspector.
    public Texture2D selectCursor;
    // Assign the default cursor texture (or leave null to use OS default)
    public Texture2D defaultCursor;

    // Optional: reference to an Image component used for the border.
    // This could be a child object that visually represents a border.
    [SerializeField]
    private Image isHoveredImage;

    private void Awake()
    {
        isHoveredImage.gameObject.SetActive(false);
    }

    // When the pointer enters the HostEntry area.
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Change the cursor.
        //Cursor.SetCursor(selectCursor, Vector2.zero, CursorMode.Auto);

        // Change the border color to highlight it.
        if (isHoveredImage != null)
        {
            isHoveredImage.gameObject.SetActive(true);
        }
    }

    // When the pointer exits the HostEntry area.
    public void OnPointerExit(PointerEventData eventData)
    {
        // Reset the cursor.
        //Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);

        // Reset the border color.
        isHoveredImage.gameObject.SetActive(false);
    }
}
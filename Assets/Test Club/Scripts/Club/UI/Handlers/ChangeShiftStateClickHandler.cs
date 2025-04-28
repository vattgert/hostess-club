using Unity.VisualScripting;
using UnityEngine;

public class ChangeShiftStateClickHandler : MonoBehaviour
{
    private ShiftManager shiftManager;

    private void Start()
    {
        shiftManager = gameObject.GetComponent<ShiftManager>();
    }
    public void OnButtonClicked()
    {
        shiftManager.ChangeShiftState();
    }
}

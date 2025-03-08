using Unity.VisualScripting;
using UnityEngine;

public class ChangeShiftStateClickHandler : MonoBehaviour
{
    private ShiftManager shiftManager;
    private bool shiftState = false;

    private void Start()
    {
        shiftManager = gameObject.GetComponent<ShiftManager>();
    }
    public void OnButtonClicked()
    {
        this.shiftState = !this.shiftState;
        Debug.Log("Shift was changed to " + this.shiftState);
        shiftManager.ChangeShiftState(this.shiftState);
    }
}

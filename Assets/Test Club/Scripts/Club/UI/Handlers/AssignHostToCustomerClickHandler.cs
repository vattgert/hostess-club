using UnityEngine;

public class AssignHostToCustomerClickHandler : MonoBehaviour
{
    private ShiftManager shiftManager;

    private void Start()
    {
        this.shiftManager = gameObject.GetComponent<ShiftManager>();
    }

    public void AssignByIndex(int index)
    {
        if (index < 0 || index > 1)
        {
            Debug.LogError("Currently, host index must be 0 or 1");
            return;
        }
        this.shiftManager.AssignHostToCustomerByIndex(index);
    }
}

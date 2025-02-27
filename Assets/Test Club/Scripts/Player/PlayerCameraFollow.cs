using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offset = new Vector3(0, 0, -1);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if (player != null)
        {
            // Update the camera's position to follow the player's position with the offset
            transform.position = player.position + offset;
        }
    }
}

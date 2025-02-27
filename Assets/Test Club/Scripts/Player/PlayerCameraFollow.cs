using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private float borderMarginX;
    [SerializeField]
    private float borderMarginY;
    public Transform player;
    public Vector3 offset;
    public Rigidbody2D map;
    private CompositeCollider2D mapCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offset = new Vector3(0, 0, -1);   
        mapCollider = map.GetComponent<CompositeCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Vector3 calculateCameraPosition(CompositeCollider2D mapCollider)
    {
        Vector3 cameraPosition = player.position + offset;
        if (mapCollider != null)
        {
            Bounds mapBounds = mapCollider.bounds;

            // Calculate clamped x position:
            // When the desired camera x is greater than the right limit, we clamp it.
            // When it's less than the left limit, we clamp it too.
            float minX = mapBounds.min.x + borderMarginX;
            float maxX = mapBounds.max.x - borderMarginX;

            float minY = mapBounds.min.y - borderMarginY;
            float maxY = mapBounds.max.y + borderMarginY;

            cameraPosition.x = Mathf.Clamp(cameraPosition.x, minX, maxX);
            cameraPosition.y = Mathf.Clamp(cameraPosition.y, minY, maxY);
        }
        return cameraPosition;

    }

    private void LateUpdate()
    {
        if (player != null)
        {
            transform.position = calculateCameraPosition(this.mapCollider);
        }
    }
}

using System;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5.0f;
    private Rigidbody2D rgbd;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rgbd = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get raw input values (-1, 0, or 1) for horizontal and vertical directions
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(moveX, moveY, 0).normalized;
        // Create a movement vector. Using normalized ensures consistent speed in all directions.
        rgbd.linearVelocity = movement * speed;
    }
}

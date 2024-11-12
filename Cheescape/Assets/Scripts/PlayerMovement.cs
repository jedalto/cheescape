using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float moveSpeed = 5f;        // Speed at which the player moves
    public float jumpForce = 10f;       // Force applied for jumping
    public bool isGrounded = false;     // To check if the player is on the ground

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // WASD
        if (Input.GetKeyDown(KeyCode.W)) 
        {

        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            rb.velocity = new Vector3(-moveSpeed, rb.velocity.y, 0);  // Move left
        }
        if (Input.GetKeyDown(KeyCode.S))
        {

        }
        if (Input.GetKeyDown(KeyCode.D))
        {

        }
    }
}

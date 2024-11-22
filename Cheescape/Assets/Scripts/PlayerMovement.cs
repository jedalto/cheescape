using UnityEngine;
using Cinemachine; // Include this if using Cinemachine

public class PlayerMovement : MonoBehaviour
{
    // Movement variables
    public float moveSpeed = 5f;
    public float rotateSpeed = 180f;
    public float jumpForce = 5f;

    // Camera variables
    public Camera playerCamera; // Your custom camera
    public CinemachineFreeLook cinemachineCamera; // Cinemachine camera
    private bool usingCinemachineCamera = false; // Tracks active camera

    public float cameraHeight = 2f;
    public float cameraDistance = 5f;
    public float cameraSmoothSpeed = 5f;
    public float cameraXAngle = 15f; // New variable for fixed X rotation
    public Transform lookTarget;

    // Ground checking
    public float groundCheckDistance = 0.3f;
    public LayerMask groundLayer;
    private Vector3[] cornerPositions; // Array to store corner positions of collider
    public BoxCollider boxCollider;

    private Rigidbody rb;
    private Transform cameraTransform;
    private Vector3 movement;
    public bool isGrounded;

    // Animator components
    public Animator animator;
    public bool speed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.freezeRotation = true;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        if (playerCamera != null)
        {
            cameraTransform = playerCamera.transform;
            UpdateCameraPosition();
        }

        // Ensure the custom camera starts active
        playerCamera.gameObject.SetActive(true);
        if (cinemachineCamera != null)
        {
            cinemachineCamera.gameObject.SetActive(false);
        }

        boxCollider = GetComponent<BoxCollider>();
        
    }

    private void Update()
    {
        CheckGrounded();

        // Get input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        movement = new Vector3(verticalInput, 0f, 0f).normalized;

        // Transform the movement direction based on current rotation
        movement = transform.TransformDirection(movement);

        // Handle rotation
        if (horizontalInput != 0)
        {
            Quaternion deltaRotation = Quaternion.Euler(0f, horizontalInput * rotateSpeed * Time.deltaTime, 0f);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }

        // Handle jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // Camera update for custom camera
        if (!usingCinemachineCamera)
        {
            UpdateCameraPosition();
        }

        // Handle camera swapping
        if (Input.GetMouseButtonDown(1)) // Right mouse button
        {
            SwapCameras();
        }

        // Calculate magnitude of movement
        float currentSpeed = movement.magnitude;
        speed = isGrounded && currentSpeed != 0;

        // Update Animator's Speed parameter
        animator.SetBool("Speed", speed);
    }

    //private void CheckGrounded()
    //{
    //    isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, groundCheckDistance + 0.1f, groundLayer);
    //}

    private void CheckGrounded()
    {
        isGrounded = false; // Default to not grounded

        // Get the bounds of the collider
        Bounds bounds = boxCollider.bounds;

        // Calculate the corner positions
        Vector3 bottomLeft = new Vector3(bounds.min.x, bounds.min.y, bounds.center.z);
        Vector3 bottomRight = new Vector3(bounds.max.x, bounds.min.y, bounds.center.z);
        Vector3 topLeft = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
        Vector3 topRight = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);

        // List of corners to check
        Vector3[] corners = { bottomLeft, bottomRight, topLeft, topRight };

        foreach (Vector3 corner in corners)
        {
            // Perform raycast from each corner
            bool hit = Physics.Raycast(corner + Vector3.up * 0.1f, Vector3.down, groundCheckDistance + 0.1f, groundLayer);
            Debug.DrawRay(corner + Vector3.up * 0.1f, Vector3.down * (groundCheckDistance + 0.1f), hit ? Color.green : Color.red);

            if (hit)
            {
                isGrounded = true; // Set grounded to true if any ray hits
                Debug.Log("Grounded at: " + corner); // Log successful grounding
                break;
            }
        }
    }

    private void FixedUpdate()
    {
        if (movement != Vector3.zero)
        {
            Vector3 targetVelocity = movement * moveSpeed;
            targetVelocity.y = rb.velocity.y;
            rb.velocity = targetVelocity;
        }
        else
        {
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        }
    }

    private void UpdateCamera()
    {
        if (playerCamera != null)
        {
            // Calculate the desired position behind the player
            Vector3 forwardDirection = Vector3.right;
            forwardDirection = transform.TransformDirection(forwardDirection);
            Vector3 targetPosition = transform.position - forwardDirection * cameraDistance + Vector3.up * cameraHeight;

            // Smoothly move the camera to the target position
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPosition, Time.deltaTime * cameraSmoothSpeed);

            // Calculate rotation with fixed X angle
            Vector3 directionToPlayer = transform.position - cameraTransform.position;
            float desiredYRotation = Mathf.Atan2(directionToPlayer.x, directionToPlayer.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(cameraXAngle, desiredYRotation, 0);

            // Smoothly rotate the camera
            cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, targetRotation, Time.deltaTime * cameraSmoothSpeed);
        }
    }

    private void UpdateCameraPosition()
    {
        Vector3 forwardDirection = Vector3.right;
        forwardDirection = transform.TransformDirection(forwardDirection);
        Vector3 targetPosition = transform.position - forwardDirection * cameraDistance + Vector3.up * cameraHeight;
        cameraTransform.position = targetPosition;

        // Set initial rotation with fixed X angle
        Vector3 directionToPlayer = transform.position - cameraTransform.position;
        float desiredYRotation = Mathf.Atan2(directionToPlayer.x, directionToPlayer.z) * Mathf.Rad2Deg;
        cameraTransform.rotation = Quaternion.Euler(cameraXAngle, desiredYRotation, 0);
    }

    private void SwapCameras()
    {
        usingCinemachineCamera = !usingCinemachineCamera;

        if (!usingCinemachineCamera)
        {
            // Enable the custom camera and disable the Cinemachine virtual camera
            playerCamera.gameObject.SetActive(true);
            if (cinemachineCamera != null)
            {
                cinemachineCamera.gameObject.SetActive(false);
            }
        }
        else
        {
            // Enable the Cinemachine virtual camera and disable the custom camera
            playerCamera.gameObject.SetActive(false);
            if (cinemachineCamera != null)
            {
                cinemachineCamera.gameObject.SetActive(true);
            }
        }
    }

}








/*
    public CharacterController controller;
    //public Transform cam;

    public float speed = 6f;  // character movement speed

    //public float turnSmoothTime = 0.1f;
    //float turnSmoothVelocity;

    public float mouseSensitivity = 100f; // camera rotation sensitivity
    public Transform cameraTransform; // assign the camera in the Inspector

    private float verticalRotation = 0f; // to limit vertical camera rotation
    public float rotationSpeed;

    //public bool isJumping, isJumpingAlt, isGrounded = false;

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    void Update()
    {
        // --- Character Movement ---
        float moveHorizontal = Input.GetAxis("Horizontal"); // A/D keys
        float moveVertical = Input.GetAxis("Vertical");     // W/S keys

        // Calculate movement direction relative to the player's forward direction
        Vector3 movement = new Vector3(moveHorizontal, 0, verticalInput);
        movement.Normalize();

        transform.Translate(movement * speed *Time.deltaTime, Space.World);

        // Apply movement
        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        // --- Camera Rotation ---
        /*if (Input.GetMouseButton(1)) // Right mouse button
        {
            // Get mouse input
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            // Rotate the camera horizontally
            cameraTransform.Rotate(Vector3.up * mouseX, Space.World);

            // Rotate the camera vertically (local rotation)
            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f); // Limit vertical rotation to avoid flipping
            cameraTransform.localRotation = Quaternion.Euler(verticalRotation, cameraTransform.localRotation.eulerAngles.y, 0f);
        }

        //float horizontal = Input.GetAxisRaw("Horizontal");
        //float vertical = Input.GetAxisRaw("Vertical");
        //Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        //if (direction.magnitude >= 0.1f)
        //{
        //    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        //    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        //    transform.rotation = Quaternion.Euler(0f, angle, 0f);

        //    Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        //    controller.Move(moveDir.normalized * speed * Time.deltaTime);
    }


    //void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("Entered");
    //    if (collision.gameObject.CompareTag("Ground"))
    //    {
    //        isGrounded = true;
    //    }
    //}

    //void OnCollisionExit(Collision collision)
    //{
    //    Debug.Log("Exited");
    //    if (collision.gameObject.CompareTag("Ground"))
    //    {
    //        isGrounded = false;
    //    }
    //}
    */


using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // movement vars
    public float moveSpeed = 5f;
    public float rotateSpeed = 180f;
    public float jumpForce = 5f;

    // camera vars
    public Camera playerCamera;
    public float cameraHeight = 2f;
    public float cameraDistance = 5f;
    public float cameraSmoothSpeed = 5f;

    // ground checking
    public float groundCheckDistance = 0.3f;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private Transform cameraTransform;
    private Vector3 movement;
    public bool isGrounded;

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
    }

    private void Update()
    {
        CheckGrounded();

        // Get input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Store movement for use in FixedUpdate
        movement = transform.forward * verticalInput;

        // Handle rotation
        if (horizontalInput != 0)
        {
            transform.Rotate(Vector3.up * horizontalInput * rotateSpeed * Time.deltaTime);
        }

        // Handle jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        UpdateCamera();
    }

    private void CheckGrounded()
    {
        // Cast a ray straight down to check if we're grounded
        isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, groundCheckDistance + 0.1f, groundLayer);
    }

    private void FixedUpdate()
    {
        if (movement != Vector3.zero)
        {
            Vector3 targetVelocity = movement * moveSpeed;
            // Preserve current vertical velocity
            targetVelocity.y = rb.velocity.y;
            rb.velocity = targetVelocity;
        }
        else
        {
            // Keep vertical velocity but zero out horizontal movement
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        }
    }

    private void UpdateCamera()
    {
        if (playerCamera != null)
        {
            Vector3 targetPosition = transform.position - transform.forward * cameraDistance + Vector3.up * cameraHeight;
            Quaternion targetRotation = Quaternion.LookRotation(transform.position - targetPosition);

            cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPosition, Time.deltaTime * cameraSmoothSpeed);
            cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, targetRotation, Time.deltaTime * cameraSmoothSpeed);
        }
    }

    private void UpdateCameraPosition()
    {
        Vector3 targetPosition = transform.position - transform.forward * cameraDistance + Vector3.up * cameraHeight;
        cameraTransform.position = targetPosition;
        cameraTransform.LookAt(transform.position);
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


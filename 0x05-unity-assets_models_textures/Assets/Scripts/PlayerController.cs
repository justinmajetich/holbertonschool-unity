using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 velocity;
    public Transform playerSpawn;
    public Transform playerCamera;
    private Timer playerTimer;

    private Vector3 jumpVelocity;
    public float jumpHeight = 6f;
    private bool isJumping = false;
    
    public float walkSpeed = 8f;
    [Tooltip("0 = max penalty; 1 = no penalty.")]
    [Range(0,1)]
    public float airborneDampener = 0.5f;
    public float rotateSpeed = 15f;
    public float gravity = 1f;


    void Start()
    {
        // Assign character controller component
        controller = gameObject.GetComponent<CharacterController>();

        // Assign reference to player timer
        playerTimer = gameObject.GetComponent<Timer>();

        // Set player's initial velocity
        velocity = new Vector3(0, 0, 0);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Grounded movement
        if (controller.isGrounded)
        {
            velocity.y = -controller.stepOffset / Time.deltaTime;

            // Get left/right/forward/backward input
            velocity.x = Input.GetAxis("Horizontal") * walkSpeed;
            velocity.z = Input.GetAxis("Vertical") * walkSpeed;
            
            // Determine when jump ends
            if (isJumping)
                isJumping = false;

            // Get jump input
            if (Input.GetButtonDown("Jump") && controller.isGrounded)
            {
                isJumping = true;

                velocity.y = jumpHeight;

                // Strip current velocity vector of speed scalar when recording jump velocity
                jumpVelocity = new Vector3(velocity.x / walkSpeed, velocity.y, velocity.z / walkSpeed);
            }
        }
        else // Airborne movement
        {
            // This dampens the influence of movement controls on the player while airborne.
            // 1. Add current vector to new dampened input vector.
            // 2. Re-apply speed scalar. 
            velocity.x = ((jumpVelocity.x) + Input.GetAxis("Horizontal")  * airborneDampener) * walkSpeed;
            velocity.z = ((jumpVelocity.z) + Input.GetAxis("Vertical") * airborneDampener) * walkSpeed;
            
            ApplyGravity();
        }


        // Rotates player transform to match look direction of camera (mouse X)
        // transform.rotation = Quaternion.Euler(0, playerCamera.eulerAngles.y, 0);
        // Get rotation from mouse axis
        float yRotation = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
        transform.Rotate(0f, yRotation, 0f);

        // Convert local space movement to world space vector
        Vector3 move = transform.TransformDirection(velocity);

        // Apply movement to character controller, if there's movement to apply
        if (move.magnitude >= 0.1f)
            controller.Move(move * Time.deltaTime);

        // Check if out of bounds
        if (transform.position.y < -45)
            RespawnPlayer();

        Debug.Log(velocity.y);
    }
    
    private void ApplyGravity()
    {
        // Simulate gravity
        if (velocity.y >= -30)
        {
            // As jump approaches and passes peak, exponentially increase fall velocity
            if (isJumping && velocity.y < 1.4)
                velocity.y += -gravity * 1.4f;
            else
                velocity.y += -gravity;
        }
    }

    private void RespawnPlayer()
    {
        float fallSpeed = velocity.y;

        // Reset velocity x/z, maintain speed of fall
        velocity = jumpVelocity = new Vector3(0f, fallSpeed, 0f);

        // Move player to spawn position
        transform.position = playerSpawn.position;

        // Reset player timer
        playerTimer.Reset();
    }
}

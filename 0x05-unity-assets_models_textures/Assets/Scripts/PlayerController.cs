using UnityEngine;

public class PlayerController : MonoBehaviour
{
 
    public Transform playerSpawn;
    public Transform playerCamera;

    public float jumpHeight = 6f;
    public float walkSpeed = 8f;
    [Tooltip("0 = max penalty; 1 = no penalty.")]
    [Range(0,1)]
    public float airborneDampener = 0.5f;
    public float rotateSpeed = 15f;
    public float gravity = 1f;

    private CharacterController controller;
    private Vector3 velocity = new Vector3(0, 0, 0);
    private Timer playerTimer;
    private bool isRespawning;
    private Vector3 jumpVelocity;
    private bool isJumping = false;


    void Start()
    {
        // Assign character controller component.
        controller = gameObject.GetComponent<CharacterController>();

        // Assign reference to player timer.
        playerTimer = gameObject.GetComponent<Timer>();

        isRespawning = true;

        // Set cursor settings
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Rotates player transform according to the mouse X axis.
        transform.Rotate(0f, Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime, 0f);

        // New movement input is applied to the velocity variable.
        MovePlayer();

        // Check if player is out of bounds.
        if (transform.position.y < -40)
            RespawnPlayer();
    }
    
    private void MovePlayer()
    {
        // Grounded movement
        if (controller.isGrounded)
        {
            // End respawn state when player has touched ground.
            if (isRespawning)
                isRespawning = false;

            // End jump state when player returns to the ground.
            if (isJumping)
                isJumping = false;

            // Assign default Y velocity for grounded state.
            // This helps avoid issues with ground detection and mesh overlap.
            velocity.y = -controller.stepOffset / Time.deltaTime;

            // Get left/right/forward/backward input
            velocity.x = Input.GetAxis("Horizontal") * walkSpeed;
            velocity.z = Input.GetAxis("Vertical") * walkSpeed;

            // Get jump input
            if (Input.GetButtonDown("Jump") && controller.isGrounded)
            {
                isJumping = true;

                velocity.y = jumpHeight;

                // Strip current velocity vector of speed scalar when recording jump velocity.
                // jumpVelocity stores an initial jump trajectory which can be altered combined
                // with any new X/Z movement controls received while player is still airborne.
                jumpVelocity = new Vector3(velocity.x / walkSpeed, velocity.y, velocity.z / walkSpeed);
            }
        }
        else // Airborne movement
        {
            if (!isRespawning)
            {
                // This dampens the influence of movement controls on the player while airborne.
                // 1. Add current vector to new dampened input vector.
                // 2. Re-apply speed scalar. 
                velocity.x = ((jumpVelocity.x) + Input.GetAxis("Horizontal")  * airborneDampener) * walkSpeed;
                velocity.z = ((jumpVelocity.z) + Input.GetAxis("Vertical") * airborneDampener) * walkSpeed;
            }

            // Apply gravity effect to player's Y velocity.
            ApplyGravity();
        }

        // Convert local space movement to world space vector.
        Vector3 move = transform.TransformDirection(velocity);

        // Apply movement to character controller, if there's movement to apply.
        if (move.magnitude >= 0.1f)
            controller.Move(move * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        if (velocity.y >= -30) // -30 represents maximum downward velocity allowed.
        {
            // As jump approaches and passes peak, exponentially increase fall velocity.
            // This provides a jump arc that curves more dramatically past its peak.
            if (isJumping && velocity.y < 1.4)
                velocity.y += -gravity * 1.4f;
            else // Gravity application for a non-jump-induced airborne state.
                velocity.y += -gravity;
        }
    }

    private void RespawnPlayer()
    {
        float fallSpeed = velocity.y;

        // Reset velocity x/z, maintain speed of fall.
        velocity = jumpVelocity = new Vector3(0f, fallSpeed, 0f);

        // Move player to spawn position.
        transform.position = playerSpawn.position;

        isRespawning = true;

        // Reset player timer.
        playerTimer.Reset();
    }
}

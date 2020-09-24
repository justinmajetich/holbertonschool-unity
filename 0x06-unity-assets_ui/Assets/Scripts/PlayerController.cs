using UnityEngine;

public class PlayerController : MonoBehaviour
{
 
    public Transform playerSpawn;
    public Transform playerCamera;
    public float yBounds = -30f;
    [Range(0,20)]
    public float jumpHeight = 12f;
    [Range(0,4)]
    public float heldJumpAdditive = 3f;
    [Tooltip("Sets a threshold based on player's Y velocity to limit how long a held jump will add to current jump velocity.")]
    [Range(-1,5)]
    public float heldJumpThreshold = 0.25f;
    public float jumpTimer = 0f;
    public float walkSpeed = 10f;
    private bool isJumping = false;

    [Tooltip("Dampens airborne movement (0 = max penalty; 1 = no penalty).")]
    [Range(0,1)]
    public float airborneDampener = 0.5f;
    public float rotateSpeed = 150f;
    public float gravity = 1f;

    private CharacterController controller;
    public Vector3 velocity = new Vector3(0, 0, 0);
    private Timer playerTimer;
    private bool isRespawning;
    private Vector3 jumpVelocity;


    void Start()
    {
        // Assign character controller component.
        controller = gameObject.GetComponent<CharacterController>();

        // Assign reference to player timer.
        playerTimer = gameObject.GetComponent<Timer>();

        isRespawning = true;

        // Set cursor settings
        // Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Rotates player transform according to the mouse X axis.
        transform.Rotate(0f, Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime, 0f);

        // New movement input is applied to the velocity variable.
        MovePlayer();

        // Check if player is out of bounds.
        if (transform.position.y < yBounds)
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
            {
                isJumping = false;

                // Reset jump timer.
                jumpTimer = 0;
            }

            // Assign default Y velocity for grounded state.
            // This helps avoid issues with ground detection and mesh overlap.
            velocity.y = -10f;

            // Get left/right/forward/backward input
            velocity.x = Input.GetAxis("Horizontal") * walkSpeed;
            velocity.z = Input.GetAxis("Vertical") * walkSpeed;
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

        GetJump(); // Checks and applies jump input to Y velocity.

        // Convert local space movement to world space vector.
        Vector3 move = transform.TransformDirection(velocity);

        // Apply movement to character controller, if there's movement to apply.
        if (move.magnitude >= 0.01f)
            controller.Move(move * Time.deltaTime);
    }

    private void GetJump()
    {
        // Get input to initiate base jump mechanic.
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            isJumping = true;
            velocity.y = jumpHeight;

            // Strip current velocity vector of speed scalar when recording jump velocity.
            // jumpVelocity stores an initial jump trajectory which can be altered combined
            // with any new X/Z movement controls received while player is still airborne.
            jumpVelocity = new Vector3(velocity.x / walkSpeed, velocity.y, velocity.z / walkSpeed);
        }
        
        // Dynamically add to jump velocity the longer jump button is held.
        // Additional jump velocity will added as long as overall Y velocity
        // remains greater than given threshold (i.e. at or around jump's peak).
        if (Input.GetButton("Jump") && velocity.y > heldJumpThreshold)
        {
            // Increment timer to track jump time.
            jumpTimer += Time.deltaTime;

            // Increase jump velocity dynamically over time.
            // Jump additive is divided by the duration of the jump button press.
            // This produces an inverse correlation between jump time and jump additive.
            velocity.y += (heldJumpAdditive / jumpTimer) * Time.deltaTime;
        }
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

using UnityEngine;

public class PlayerController : MonoBehaviour
{
 
    public Transform playerSpawn;
    public Transform cameraLookDirection;
    private CharacterController controller;
    private Animator animator;

    [Tooltip("This variable allows the player input to be discontinued while pause menu is active.")]
    public bool inputEnabled = true;
    public Vector3 velocity = new Vector3(0, 0, 0);
    private Timer playerTimer;
    public bool isRespawning = false;
    public float yBounds = -40f;
    public Transform fallThreshold;

    public float walkSpeed = 10f;
    public float rotateSpeed = 250f;
    [Range(0,20)]
    public float jumpHeight = 12f;
    private Vector3 jumpVelocity;

    [Range(0,4)]
    public float heldJumpAdditive = 3f;
    public float jumpTimer = 0f;
    private bool isJumping = false;

    // isRunning only serves to inform the animator and audio controller.
    public bool isRunning = false;
    public bool isFalling = false;
    public bool madeImpact = false;


    [Tooltip("Sets a threshold based on player's Y velocity to limit how long a held jump will add to current jump velocity.")]
    [Range(-1,5)]
    public float heldJumpThreshold = 0.25f;

    [Tooltip("Dampens airborne movement (0 = max penalty; 1 = no penalty).")]
    [Range(0,1)]
    public float airborneDampener = 0.5f;
    public float gravity = 1f;


    void Start()
    {
        // Assign references to player components.
        controller = GetComponent<CharacterController>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        playerTimer = GetComponent<Timer>();

        // Set cursor settings
        Cursor.visible = false;
    }

    void Update()
    {
        isRunning = (velocity.z != 0 && controller.isGrounded);

        // Send info to animation controller.
        if (inputEnabled)
        {
            animator.SetBool("isRunning", isRunning);
            animator.SetBool("isJumping", isJumping);
        }
        else // This keep animations from looping endlessly behind the win screen.
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping", false);
        }

        isFalling = (transform.position.y < fallThreshold.position.y);
        animator.SetBool("isFalling", (isFalling || isRespawning));

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
            {
                madeImpact = true;
                isRespawning = false;
            }
            else
            {
                madeImpact = false;
            }

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

            // Get left/right/forward/backward input.
            if (inputEnabled)
            {
                // Rotate character to face direction of movement (there has to be a cleaner way of doing this...).
                if (Input.GetKey(KeyCode.W))
                    transform.rotation = Quaternion.Euler(0f, Quaternion.LookRotation(cameraLookDirection.forward).eulerAngles.y, 0f);
                if (Input.GetKey(KeyCode.S))
                    transform.rotation = Quaternion.Euler(0f, Quaternion.LookRotation(-cameraLookDirection.forward).eulerAngles.y, 0f);
                if (Input.GetKey(KeyCode.D))
                    transform.rotation = Quaternion.Euler(0f, Quaternion.LookRotation(cameraLookDirection.right).eulerAngles.y, 0f);
                if (Input.GetKey(KeyCode.A))
                    transform.rotation = Quaternion.Euler(0f, Quaternion.LookRotation(-cameraLookDirection.right).eulerAngles.y, 0f);

                // If any input is received, remove sign.
                float vertical = Mathf.Abs(Input.GetAxis("Vertical"));
                float horizontal = Mathf.Abs(Input.GetAxis("Horizontal"));

                // Take greatest input signal and apply to velocity.
                float forward = vertical > horizontal ? vertical : horizontal;
                velocity.z = forward * walkSpeed;
            }
        }
        else // Airborne movement
        {
            if (!isRespawning && inputEnabled)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    transform.rotation = Quaternion.Euler(0f, Quaternion.LookRotation(cameraLookDirection.forward).eulerAngles.y, 0f);
                }

                // This dampens the influence of movement controls on the player while airborne.
                // 1. Add current vector to new dampened input vector.
                // 2. Re-apply speed scalar. 
                // velocity.x = ((jumpVelocity.x) + Input.GetAxis("Horizontal")  * airborneDampener) * walkSpeed;
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
        if (inputEnabled && Input.GetButtonDown("Jump") && controller.isGrounded)
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
        if (inputEnabled && Input.GetButton("Jump") && velocity.y > heldJumpThreshold)
        {
            // Increment timer to track jump time.
            jumpTimer += Time.deltaTime;

            // Increase jump velocity dynamically over time.
            // Jump additive is divided by the duration of the jump button press.
            // This produces an inverse correlation between jump time and jump additive.
            velocity.y += (heldJumpAdditive / jumpTimer) * 0.035f;
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
        inputEnabled = false;

        // Reset player timer.
        playerTimer.Reset();
    }
}

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController cController;
    private Vector3 velocity;
    public Transform playerSpawn;
    public Transform playerCamera;
    private Vector3 jumpVelocity;
    private float jumpHeight = 6f;
    

    public float walkSpeed = 8f;
    [Tooltip("0 = max penalty; 1 = no penalty.")]
    [Range(0,1)]
    public float airborneDampener = 0.5f;
    public float jumpSpeed = 15f;
    public float turnSpeed = 25f;
    public float gravity = 1f;


    void Start()
    {
        // Assign character controller component
        cController = gameObject.GetComponent<CharacterController>();

        // Set player's initial velocity
        velocity = new Vector3(0, 0, 0);
    }

    void Update()
    {
        // Zero out velocity when grounded
        if (cController.isGrounded)
            velocity = Vector3.zero;

        // Grounded movement
        if (cController.isGrounded)
        {
            // Get left/right/forward/backward input
            velocity.x = Input.GetAxis("Horizontal") * walkSpeed;
            velocity.z = Input.GetAxis("Vertical") * walkSpeed;
            
            // Get jump input
            if (Input.GetButtonDown("Jump") && cController.isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * jumpSpeed);

                // Strip current velocity vector of speed scalar when recording jump velocity
                jumpVelocity = new Vector3(velocity.x / walkSpeed, 0f, velocity.z / walkSpeed);
            }
        }
        else // Airborne movement
        {
            // This dampens the influence of movement controls on the player while airborne.
            // 1. Add current vector to new dampened input vector.
            // 2. Re-apply speed scalar. 
            velocity.x = ((jumpVelocity.x) + Input.GetAxis("Horizontal")  * airborneDampener) * walkSpeed;
            velocity.z = ((jumpVelocity.z) + Input.GetAxis("Vertical") * airborneDampener) * walkSpeed;
        }

        // Simulate gravity
        if (velocity.y >= -30)
            velocity.y += -gravity;

        // Rotates player transform to match look direction of camera (mouse X)
        transform.rotation = Quaternion.Euler(0, playerCamera.eulerAngles.y, 0);
        
        // Convert local space movement to world space vector
        Vector3 move = transform.TransformDirection(velocity);

        // Apply movement to character controller, if there's movement to apply
        if (move.magnitude >= 0.1f)
            cController.Move(move * Time.deltaTime);

        // Check if out of bounds
        if (transform.position.y < -45)
            RespawnPlayer();
    }
    
    private void RespawnPlayer()
    {
        float fallSpeed = velocity.y;

        // Reset velocity x/z, maintain speed of fall
        velocity = jumpVelocity = new Vector3(0f, fallSpeed, 0f);

        // Move player to spawn position
        transform.position = playerSpawn.position;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides functionality to control a character object, including
/// horizontal movement, jumping, and collision detection/resolution.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class CharacterController2D : MonoBehaviour
{
    private string playerName = "";
    private Vector2 velocity = new Vector2(0f, 0f);
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    private Transform bulletSpawn;
    private BulletSpawn firingMechanism;
    private bool characterIsGrounded;
    private Vector3 flipVector = new Vector3(1.5f, 0f, 0f);

    [SerializeField, Tooltip("Boolean to denote which direction sprite is facing on spawn.")]
    bool characterIsRightFacing = false;

    [SerializeField, Tooltip("The maximun velocity of character movement.")]
    float speed = 8f;

    [SerializeField, Tooltip("Rate at which player velocity increases while grounded.")]
    float groundAcceleration = 75f;

    [SerializeField, Tooltip("Rate at which player velocity increases while airborne.")]
    float airborneAcceleration = 15f;

    [SerializeField, Tooltip("Rate at which player velocity decreases while grounded.")]
    float groundDeceleration = 50f;

    [SerializeField, Tooltip("Rate at which player velocity decreases while airborne.")]
    float airborneDeceleration = 8f;

    [SerializeField, Tooltip("The height of the character's jump.")]
    float jumpHeight = 6f;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        bulletSpawn = transform.GetChild(0);
        firingMechanism = bulletSpawn.GetComponent<BulletSpawn>();
        playerName = gameObject.name;
    }

    void Update()
    {
        GetVerticalInput();
        GetHorizontalInput();
        HandleCollisions();
        GetFireInput();
    }

    private void GetVerticalInput()
    {
        if (characterIsGrounded)
        {
            // Set default grounded velocity (less than one to avoid ground checking inaccuracies). 
            velocity.y = -0.1f;

            // If jump button pressed, apply jumpHeight to Y velocity.
            if (Input.GetButtonDown($"Jump{playerName}"))
            {
                velocity.y = Mathf.Sqrt(jumpHeight * Mathf.Abs(Physics2D.gravity.y));
            }
        }
        else
        {
            // If character is airborne, apply gravity.
            velocity.y += Physics2D.gravity.y * Time.deltaTime;
        }

        // If crouch button is pressed, scale character in half; if released, return to default scale.
        transform.localScale = Input.GetButton($"Crouch{playerName}") ? new Vector3(1f, 0.5f, 1f) : new Vector3(1f, 1f, 1f);
    }

    private void GetHorizontalInput()
    {
        float horizontal = Input.GetAxisRaw($"Horizontal{playerName}");

        // Scale acceleration and deceleration values is character is airborne.
        float acceleration = characterIsGrounded ? groundAcceleration : airborneAcceleration;
        float deceleration = characterIsGrounded ? groundDeceleration : airborneDeceleration;

        // If there is horizontal input, accelerate X velocity toward max speed.
        if (horizontal != 0)
        {
            velocity.x = Mathf.MoveTowards(velocity.x, horizontal * speed, acceleration * Time.deltaTime);
        }
        else // Else, decelerate X velocity towards 0.
        {
            velocity.x = Mathf.MoveTowards(velocity.x, 0f, deceleration * Time.deltaTime);
        }

        // Apply updated velocity to character.
        transform.Translate(velocity * Time.deltaTime);

        // If character is not facing direction of velocity, flip character.
        if (horizontal < 0 && characterIsRightFacing)
            FlipCharacter();
        if (horizontal > 0 && !characterIsRightFacing)
            FlipCharacter();
    }

    private void FlipCharacter()
    {
        // Flip sprite.
        spriteRenderer.flipX = !spriteRenderer.flipX;

        // Rotate the direction in which bullet spawn is rotating.
        bulletSpawn.Rotate(0f, 180f, 0f);

        // Position bullet spawn to the side which the character is facing.
        if (characterIsRightFacing)
            bulletSpawn.position = bulletSpawn.position - flipVector;
        else
            bulletSpawn.position = bulletSpawn.position + flipVector;

        // Toggle the is facing right boolean.
        characterIsRightFacing = !characterIsRightFacing;
    }

    private void HandleCollisions()
    {
        // Retrieve array of all colliders currently overlapped with character's BoxCollider.
        Collider2D[] collisions = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0);

        characterIsGrounded = false;

        foreach (Collider2D collision in collisions)
        {
            // Only resolve collisons with colliders in ground layer.
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                ColliderDistance2D colliderDistance = collision.Distance(boxCollider);

                // Make sure collider is still overlapped (This is helpful in case an priorly resolved collision already
                // moved the character's collider out of contact with the current collision being resolved.
                if (colliderDistance.isOverlapped)
                {
                    // Resolve collision by moving character's collider outside of collision.
                    transform.Translate(colliderDistance.pointA - colliderDistance.pointB);

                    // Check the normals of the surface being collided with. If the angle of normal is within 90 degress of up
                    // and the character does not currently have an upward velocity, the character is considered grounded.
                    if (Vector2.Angle(colliderDistance.normal, Vector2.up) < 90 && velocity.y < 0)
	                    characterIsGrounded = true;
                }
            }
        }
    }

    private void GetFireInput()
    {
        if (Input.GetButtonDown($"Fire{playerName}"))
            firingMechanism.Fire();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dictates bullet behavior and physics.
/// </summary>
public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 lastPosition;
    public float bulletSpeed = 25f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();   
    }
    void Start()
    {
        lastPosition = transform.position;

        // Set bullet velocity.
        rb.velocity = transform.right * bulletSpeed;
    }

    void FixedUpdate()
    {
        LayerMask layerMask = LayerMask.GetMask("Ground", "Player");

        // Get distance between last and current position.
        float rayLength = Vector3.Distance(lastPosition, transform.position);

        // Cast a ray from last position to current, determining if the bullet's trajectory
        // has intersected with a collider in the "Ground" layer.
        RaycastHit2D hit = Physics2D.Raycast(lastPosition, transform.right, rayLength, layerMask);

        if (rayLength > 0 && hit)
            Impact(hit.collider);
        else
            lastPosition = transform.position; // Otherwise, save the current position for next check.
    }

    void Impact(Collider2D receiving)
    {
        if (receiving.tag == "Player")
            receiving.gameObject.GetComponent<PlayerHealth>().TakeDamage();

        // Destroy the bullet game object.
        Destroy(gameObject);
    }
}

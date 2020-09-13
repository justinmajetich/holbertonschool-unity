using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages player health.
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    public float health = 5f;
    private Transform healthBar;
    private Vector3 initialHealthBarScale;

    void Awake()
    {
        healthBar = transform.Find("HealthBar/Bar/Fill");
        initialHealthBarScale = healthBar.localScale;
    }

    void Update()
    {
        if (health <= 0)
            Destroy(gameObject);
    }

    /// <summary>
    /// Reduces player health.
    /// </summary>
    public void TakeDamage()
    {
        health -= 1;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        // Scale health bar fill to reflect player health.
        healthBar.localScale = new Vector3((initialHealthBarScale.x / 5) * health,
                                            initialHealthBarScale.y,
                                            initialHealthBarScale.z);
    }
}

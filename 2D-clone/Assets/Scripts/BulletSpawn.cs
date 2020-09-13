using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the spawning/firing of bullets from character.
/// </summary>
public class BulletSpawn : MonoBehaviour
{
    public GameObject bullet;

    /// <summary>
    /// Instatiates a bullet prefab.
    /// </summary>
    public void Fire()
    {
        // Determine initial rotation of bullet prefab on spawn.
        Quaternion initialRotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);

        // Spawn bullet prefab on bullet spawn.
        Instantiate(bullet, transform.position, initialRotation);
    }
}

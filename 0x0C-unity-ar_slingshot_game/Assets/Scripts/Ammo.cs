using UnityEngine;

public class Ammo : MonoBehaviour
{
    public delegate void AmmoEvent();
    public static event AmmoEvent onAmmoDestroyed;
    public static event AmmoEvent onTargetHit;
    bool isBeingCleared = false;


    void Update()
    {   
        // Destroy ammo object upon traveling out of bounds, as defined along Y axis.
        if (ARSurfaceManager.gamePlane && transform.position.y <= ARSurfaceManager.gamePlane.transform.position.y) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other) {

        // Destroy ammo object upon colliding with game plane.
        if (other.tag == "ARPlane") {
            Destroy(gameObject);
        }

        // Destroy ammo and targt objects upon collision.
        if (other.tag == "Target") {
            onTargetHit();
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Destroys this gameobject without invoking onAmmoDestroyed event.
    /// </summary>
    public void Clear() {
        isBeingCleared = true;
        Destroy(gameObject);
    }

    void OnDestroy() {
        if (!isBeingCleared) {
            onAmmoDestroyed();
        }
    }
}

using UnityEngine;

public class BottleShatterTest : MonoBehaviour
{
    public GameObject shatterFX;
    public GameObject bottle;

    public void ShatterBottle() {
        Instantiate(shatterFX, bottle.transform.position, Quaternion.identity);
        Destroy(bottle);
    }
}

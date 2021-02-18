using UnityEngine;

public class AmmoDisplay : MonoBehaviour
{
    public Transform ammoList;

    /// <summary>
    /// Sets ammo counter to display ammo remaining.
    /// </summary>
    /// <param name="ammoRemaining">The amount of ammo remaining.</param>
    public void SetAmmoCount(int ammoRemaining) {

        // Set ammo icons active/inactive based on amount of ammo remaining.
        for (int i = 0; i < ammoList.childCount; i++) {
            if (i <= ammoRemaining - 1) {
                ammoList.GetChild(i).gameObject.SetActive(true);
            } else {
                ammoList.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}

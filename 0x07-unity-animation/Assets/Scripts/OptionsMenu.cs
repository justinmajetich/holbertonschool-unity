using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    Toggle invertYToggle;

    void Start()
    {   
        // Initialize any unset PlayerPref settings.
        InitializeSettings();

        // Get reference to UI toggle.
        invertYToggle = GameObject.Find("InvertYToggle").GetComponent<Toggle>();

        // Retrieve inversion setting from disk to set toggle starting state.
        // (0 = not inverted, 1 = inverted).
        invertYToggle.isOn = PlayerPrefs.GetInt(PlayerPrefKeys.invertY) == 1 ? true : false;

        // Show cursor.
        Cursor.visible = true;
    }
    
    public void Back()
    {
        // For dynamic back maybe...
        int previousScene = PlayerPrefs.GetInt(PlayerPrefKeys.previousScene);
        SceneManager.LoadScene(previousScene);
    }

    public void Apply()
    {
        // Update PlayerPrefs with current settings.
        PlayerPrefs.SetInt(PlayerPrefKeys.invertY, (invertYToggle.isOn ? 1 : 0));
    }

    private void InitializeSettings()
    {
        // Check to see if PlayerPrefs are initialized and, if not, initialize.
        if (!PlayerPrefs.HasKey(PlayerPrefKeys.invertY))
            PlayerPrefs.SetInt(PlayerPrefKeys.invertY, 0);

        if (!PlayerPrefs.HasKey(PlayerPrefKeys.previousScene))
            PlayerPrefs.SetInt(PlayerPrefKeys.previousScene, 0);
    }
}

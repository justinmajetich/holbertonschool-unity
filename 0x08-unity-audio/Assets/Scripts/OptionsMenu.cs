using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    Toggle invertYToggle;
    Slider bgmSlider;
    Slider sfxSlider;


    void Start()
    {   
        // Initialize any unset PlayerPref settings.
        InitializeSettings();

        // Get reference to UI toggle and sliders.
        invertYToggle = GameObject.Find("InvertYToggle").GetComponent<Toggle>();
        bgmSlider = GameObject.Find("BGMSlider").GetComponent<Slider>();
        sfxSlider = GameObject.Find("SFXSlider").GetComponent<Slider>();


        // Retrieve inversion settings from disk to set toggle starting state.
        // (0 = not inverted, 1 = inverted).
        invertYToggle.isOn = PlayerPrefs.GetInt(PlayerPrefKeys.invertY) == 1 ? true : false;
        
        // Retrieve slider values from disk storage.
        bgmSlider.value = PlayerPrefs.GetFloat(PlayerPrefKeys.bgmVolume);
        sfxSlider.value = PlayerPrefs.GetFloat(PlayerPrefKeys.sfxVolume);

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
        PlayerPrefs.SetFloat(PlayerPrefKeys.bgmVolume, bgmSlider.value);
        PlayerPrefs.SetFloat(PlayerPrefKeys.sfxVolume, sfxSlider.value);
    }

    private void InitializeSettings()
    {
        // Check to see if PlayerPrefs are initialized and, if not, initialize.
        if (!PlayerPrefs.HasKey(PlayerPrefKeys.invertY))
            PlayerPrefs.SetInt(PlayerPrefKeys.invertY, 0);

        if (!PlayerPrefs.HasKey(PlayerPrefKeys.previousScene))
            PlayerPrefs.SetInt(PlayerPrefKeys.previousScene, 0);

        if (!PlayerPrefs.HasKey(PlayerPrefKeys.bgmVolume))
            PlayerPrefs.SetFloat(PlayerPrefKeys.bgmVolume, 0.25f);

        if (!PlayerPrefs.HasKey(PlayerPrefKeys.sfxVolume))
            PlayerPrefs.SetFloat(PlayerPrefKeys.sfxVolume, 0.75f);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    public void Back()
    {
        // Load previous scene.
        SceneManager.LoadScene(0);

        // For dynamic back maybe...
        // int previousScene = PlayerPrefs.GetInt("PreviousScene");
        // SceneManager.LoadScene(previousScene);
    }
}

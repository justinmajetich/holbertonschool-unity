using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void LevelSelect(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void Options()
    {
        // Save current scene to PlayerPrefs for back reference.
        PlayerPrefs.SetInt(PlayerPrefKeys.previousScene, SceneManager.GetActiveScene().buildIndex);

        // Load Options menu.
        SceneManager.LoadScene(4);
    }

    public void Exit()
    {
        Debug.Log("Exited");
        Application.Quit();
    }
}

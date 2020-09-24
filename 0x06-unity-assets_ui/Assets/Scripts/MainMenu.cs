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
        SceneManager.LoadScene(4);
    }

    public void Exit()
    {
        Debug.Log("Exited");
        Application.Quit();
    }
}

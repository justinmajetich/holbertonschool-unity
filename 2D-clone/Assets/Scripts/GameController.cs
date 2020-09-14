using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages win and replay conditions.
/// </summary>
public class GameController : MonoBehaviour
{
    public Canvas menu;

    void Start()
    {
        menu.GetComponent<Canvas>().enabled = false;
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (!menu.GetComponent<Canvas>().enabled)
                menu.GetComponent<Canvas>().enabled = true;
            else
                menu.GetComponent<Canvas>().enabled = false;
        }
            


    }

    public void GameOver()
    {
        menu.GetComponent<Canvas>().enabled = true;
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        menu.GetComponent<Canvas>().enabled = false;
    }

    public void Quit()
    {
        Application.Quit();
    }
}

using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused = false;
    private Timer timer;
    private PlayerController playerController;
    private CameraController cameraController;
    public GameObject menu;

    void Start()
    {
        timer = GameObject.FindWithTag("Player").GetComponent<Timer>();
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        cameraController = GameObject.FindWithTag("MainCamera").GetComponent<CameraController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ESC pressed.");
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        // Pause timer if reference is held.
        if (timer != null)
            timer.Pause();

        // Disable player input controls and zero-out velocity.
        playerController.inputEnabled = false;
        playerController.velocity = Vector3.zero;
        cameraController.inputEnabled = false;

        // Set menu canvas to appear.
        menu.SetActive(true);

        isPaused = true;
    }

    public void Resume()
    {
        // Set menu canvas to disappear.
        menu.SetActive(false);

        // Restore player input controls.
        playerController.inputEnabled = true;
        cameraController.inputEnabled = true;

        // Run timer.
        if (timer != null)
            timer.Run();

        isPaused = false;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Options()
    {
        // Save current scene to PlayerPrefs for back reference.
        PlayerPrefs.SetInt(PlayerPrefKeys.previousScene, SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(4);
    }
}

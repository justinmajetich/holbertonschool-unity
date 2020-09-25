using UnityEngine;
using UnityEngine.UI;


public class Timer : MonoBehaviour
{
    public Text timerText;
    public Text finalTime;
    private float time = 0;
    private float minutes;
    private float seconds;
    private bool timerIsRunning = false;
    private PlayerController playerController;
    private CameraController cameraController;


    void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        cameraController = GameObject.FindWithTag("MainCamera").GetComponent<CameraController>();
    }

    void Update()
    {
        if (timerIsRunning)
        {
            time += Time.deltaTime;

            if (time >= 60.00f)
            {
                minutes++;
                seconds = time;
                time = 0;
            }

            if (minutes >= 60)
                minutes = 0;

            timerText.text = $"{minutes.ToString()}:{time.ToString("00.##")}";
        }
    }

    public void Run()
    {
        timerIsRunning = true;
    }

    public void Pause()
    {
        timerIsRunning = false;
    }

    // Stop timer and reset time to zero
    public void Reset()
    {
        timerIsRunning = false;
        time = minutes = seconds = 0;
        timerText.text = "0:00.00";
        timerText.color = Color.white;
        timerText.fontSize = 48;
    }

    public void Win()
    {
        Pause();
        
        // Disable player input controls and zero-out velocity.
        playerController.inputEnabled = false;
        playerController.velocity = Vector3.zero;
        cameraController.inputEnabled = false;
        Cursor.visible = true;

        finalTime.text = $"{minutes.ToString()}:{time.ToString("00.##")}";
    }
}

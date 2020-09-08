using UnityEngine;
using UnityEngine.UI;


public class Timer : MonoBehaviour
{
    public Text timerText;
    private float time = 0;
    private float minutes;
    private float seconds;
    private bool timerIsRunning = false;

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
}

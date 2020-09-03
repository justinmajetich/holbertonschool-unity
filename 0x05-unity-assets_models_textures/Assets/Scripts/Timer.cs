using UnityEngine;
using UnityEngine.UI;


public class Timer : MonoBehaviour
{
    public Text timerText;
    private float time;
    private float minutes;
    private float seconds;
    private bool timerIsRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        timerIsRunning = true;
    }

    // Update is called once per frame
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

    public void Pause()
    {
        timerIsRunning = false;
    }

    public void Reset()
    {
        timerIsRunning = false;
        time = minutes = seconds = 0;
    }
}

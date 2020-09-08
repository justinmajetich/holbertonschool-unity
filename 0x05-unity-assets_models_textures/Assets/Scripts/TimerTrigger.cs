using UnityEngine;

public class TimerTrigger : MonoBehaviour
{
    private Timer timer;

    void Start()
    {
        timer = GameObject.FindWithTag("Player").GetComponent<Timer>();
    }

    public void OnTriggerExit(Collider other)
    {
        Debug.Log(other.tag);
        if (other.tag == "Player")
        {
            EnableTimer();
            timer.Run();
        }
    }

    private void EnableTimer()
    {
        timer.enabled = true;
    }
}

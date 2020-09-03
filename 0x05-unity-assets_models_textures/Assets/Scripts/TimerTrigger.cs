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
        if (other.tag == "Player")
            EnableTimer();
    }

    private void EnableTimer()
    {
        timer.enabled = true;
    }
}

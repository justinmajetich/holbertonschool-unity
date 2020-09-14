using UnityEngine.UI;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    public Text timerText;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<Timer>().Pause();
            timerText.color = Color.green;
            timerText.fontSize = 60;
        }
    }
}

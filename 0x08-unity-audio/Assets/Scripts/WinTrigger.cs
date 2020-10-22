using UnityEngine.UI;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    public Text timerText;
    private Timer timer;
    public GameObject winCanvas;
    private MusicController musicController;

    void Start()
    {
        timer = GameObject.FindWithTag("Player").GetComponent<Timer>();
        musicController = GameObject.Find("BGM").GetComponent<MusicController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            timer.Win();
            timerText.enabled = false;
            winCanvas.SetActive(true);
            musicController.PlayWinBGM();
        }
    }
}

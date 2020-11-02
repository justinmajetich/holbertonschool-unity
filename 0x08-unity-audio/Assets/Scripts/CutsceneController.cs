using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{
    private Animator animator;
    public GameObject mainCamera;
    public GameObject timerCanvas;
    public GameObject menuController;
    private PlayerController playerController;
    private MusicController musicController;
    public string animationName;
    public bool skipAnimation = false;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        musicController = GameObject.Find("BGM").GetComponent<MusicController>();
    }

    void Update()
    {
        if (skipAnimation || !animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
        {
            mainCamera.SetActive(true);
            playerController.enabled = true;
            menuController.SetActive(true);
            timerCanvas.SetActive(true);
            gameObject.SetActive(false);
            musicController.TransitionFromCutscene();
        }
    }
}

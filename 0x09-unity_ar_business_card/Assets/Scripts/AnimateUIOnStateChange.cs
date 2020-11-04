using UnityEngine;
using UnityEngine.UI;

public class AnimateUIOnStateChange : MonoBehaviour
{
    private Animator animator;
    private CanvasScaler canvasScaler;
    private AudioSource audio;

    private void Awake() {
        animator = GetComponent<Animator>();
        canvasScaler = GetComponent<CanvasScaler>();
        audio = GetComponent<AudioSource>();
    }

    private void Start() {

    }

    public void PlayIntro() {
        animator.SetBool("targetHasBeenFound", true);
        audio.volume = 1f;
        audio.Play();
    }

    public void ExitIntroAnimation() {
        animator.SetBool("targetHasBeenFound", false);
        audio.volume = 0f;
        audio.Stop();
    }
}

using UnityEngine;
using UnityEngine.UI;

public class AnimateUIOnStateChange : MonoBehaviour
{
    private Animator animator;
    private CanvasScaler canvasScaler;
    private AudioSource audioSource;

    private void Awake() {
        animator = GetComponent<Animator>();
        canvasScaler = GetComponent<CanvasScaler>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start() {

    }

    public void PlayIntro() {
        animator.SetBool("targetHasBeenFound", true);
        audioSource.volume = 1f;
        audioSource.Play();
    }

    public void ExitIntroAnimation() {
        animator.SetBool("targetHasBeenFound", false);
        audioSource.volume = 0f;
        audioSource.Stop();
    }
}

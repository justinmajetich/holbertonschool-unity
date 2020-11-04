using UnityEngine;
using UnityEngine.UI;

public class AnimateUIOnStateChange : MonoBehaviour
{
    private Animator animator;
    private CanvasScaler canvasScaler;
    private int introHash;

    private void Awake() {
        animator = GetComponent<Animator>();
        canvasScaler = GetComponent<CanvasScaler>();
    }

    private void Start() {

    }

    public void PlayIntro() {
        animator.SetBool("targetHasBeenFound", true);
        canvasScaler.dynamicPixelsPerUnit += 1;
        canvasScaler.dynamicPixelsPerUnit -= 1;
    }

    public void ExitIntroAnimation() {
        animator.SetBool("targetHasBeenFound", false);
    }
}

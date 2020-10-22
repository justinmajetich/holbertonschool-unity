using UnityEngine;
using UnityEngine.Audio;


public class PlayerAudio : MonoBehaviour
{
    private PlayerController playerController;
    private AudioSource audioSource;
    public AudioMixer master;
    public AudioClip footstepsRunningGrass;
    public AudioClip footstepsLandingGrass;
    public AudioClip wilheimScream;
    private bool screamHasPlayed = false;


    void Start()
    {
        playerController = GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (playerController.isRunning)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = footstepsRunningGrass;
                audioSource.loop = true;
                audioSource.outputAudioMixerGroup = master.FindMatchingGroups("Running")[0];
                audioSource.Play();
            }
        }
        else
        {
            audioSource.loop = false;   
        }

        if (playerController.isFalling)
        {
            if (!audioSource.isPlaying && !screamHasPlayed)
            {
                audioSource.clip = wilheimScream;
                audioSource.loop = false;
                audioSource.outputAudioMixerGroup = master.FindMatchingGroups("Falling")[0];
                audioSource.Play();
                screamHasPlayed = true;
            }
        }

        if (playerController.madeImpact)
        {
            screamHasPlayed = false;

            if (!audioSource.isPlaying)
            {
                audioSource.clip = footstepsLandingGrass;
                audioSource.loop = false;
                audioSource.outputAudioMixerGroup = master.FindMatchingGroups("Landing")[0];
                audioSource.Play();
            }
        }
    }
}

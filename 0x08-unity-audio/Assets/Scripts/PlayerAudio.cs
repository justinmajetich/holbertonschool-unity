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
            if (!audioSource.isPlaying)
            {
                audioSource.clip = wilheimScream;
                audioSource.outputAudioMixerGroup = master.FindMatchingGroups("Falling")[0];
                audioSource.Play();
            }
        }

        if (playerController.madeImpact)
        {
                audioSource.clip = footstepsLandingGrass;
                audioSource.outputAudioMixerGroup = master.FindMatchingGroups("Landing")[0];
                audioSource.Play();
        }
    }
}

using UnityEngine.Audio;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioMixerSnapshot cutsceneSnapshot;
    public AudioMixerSnapshot inGameSnapshot;
    public AudioMixerSnapshot inMenuSnapshot;
    public AudioClip winBGM;
    public AudioClip levelBGM;
    public AudioMixer master;
    private AudioSource source;
    private float userSFXVolume;
    private float userBGMVolume;

    void Start()
    {
        // Get volume settings from PlayerPrefs.
        userBGMVolume = PlayerPrefs.GetFloat(PlayerPrefKeys.bgmVolume, 0.25f);
        userSFXVolume = PlayerPrefs.GetFloat(PlayerPrefKeys.sfxVolume, 0.75f);

        // Set BGM and SFX volumes according to user preference.
        master.SetFloat("bgmVolume", LinearToDecibel(userBGMVolume));
        master.SetFloat("sfxVolume", LinearToDecibel(userSFXVolume));

        source = GetComponent<AudioSource>();
        source.clip = levelBGM;
        source.loop = true;
        source.Play();
    }

    public void PlayWinBGM()
    {
        cutsceneSnapshot.TransitionTo(1.5f);
        source.Stop();
        source.clip = winBGM;
        source.loop = false;
        source.Play();
    }

    public void TransitionFromCutscene()
    {
        inGameSnapshot.TransitionTo(3f);
    }

    public void TransitionToMenu()
    {
        inMenuSnapshot.TransitionTo(0.1f);
    }

    public void TransitionFromMenu()
    {
        inGameSnapshot.TransitionTo(0.1f);
    }

    private float LinearToDecibel(float linear)
     {
         float dB;
         
         if (linear != 0)
             dB = 20.0f * Mathf.Log10(linear);
         else
             dB = -144.0f;
         
         return dB;
     }

     private float DecibelToLinear(float dB)
     {
         float linear = Mathf.Pow(10.0f, dB/20.0f);
 
         return linear;
     }
}

using UnityEngine;

public class MiniGameController : MonoBehaviour
{
    public AudioSource musicSource; // Reference to the AudioSource for background music
    public AudioSource sfxSource;   // Reference to the AudioSource for SFX
    public AudioClip bgMusic;       // Background music clip
    public AudioClip menuSFX;       // SFX for menu button interactions

    void Start()
    {
        if (musicSource != null)
        {
            musicSource.clip = bgMusic; // Set the background music clip
            musicSource.loop = true;    // Loop the background music
            musicSource.Play();         // Start playing the background music
        }
    }

    public void OnMenuButtonClick()
    {
        PlaySFX(menuSFX); // Play menu button click SFX
    }

    void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip); // Play SFX with the SFX AudioSource
        }
    }
}

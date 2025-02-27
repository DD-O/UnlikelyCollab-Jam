using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    private AudioSource audioSource;

    [Range(0f, 1f)] // Expose this in the Inspector as a slider (0 = mute, 1 = max volume)
    public float musicVolume = 1f;  // Default volume set to 1 (max volume)

    private void Awake()
    {
        // Make sure there is only one MusicManager instance.
        if (Instance == null)
        {
            Instance = this;
            audioSource = gameObject.AddComponent<AudioSource>();
            DontDestroyOnLoad(gameObject); // Persist across scenes.
        }
        else
        {
            Destroy(gameObject); // Ensure only one MusicManager exists.
        }
    }

    // Play music with the specified clip and volume (using the public musicVolume variable).
    public void PlayMusic(AudioClip musicClip)
    {
        if (audioSource.clip == musicClip) return; // Avoid replaying the same track.

        audioSource.clip = musicClip;
        audioSource.loop = true;
        audioSource.volume = musicVolume; // Set volume based on the Inspector value.
        audioSource.Play();
    }

    // Stop the currently playing music.
    public void StopMusic()
    {
        audioSource.Stop();
    }
}

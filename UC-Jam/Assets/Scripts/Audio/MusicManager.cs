using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    public AudioSource[] musicTracks; // Assign AudioSources in the inspector
    public float fadeDuration = 2f;   // Time in seconds for the fade-in effect

    private int currentTrackIndex = 0; // Tracks which song should be unmuted next

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Initialize tracks: Play all but mute them, except the first one
        for (int i = 0; i < musicTracks.Length; i++)
        {
            musicTracks[i].loop = true;
            musicTracks[i].Play();
            musicTracks[i].volume = (i == 0) ? 1f : 0f; // Set volume to 1 for the first song, 0 for the rest
        }
    }

    public void TriggerNextSong()
    {
        if (currentTrackIndex < musicTracks.Length - 1) // Ensure there's a next song
        {
            currentTrackIndex++; // Move to the next track
            StartCoroutine(FadeIn(musicTracks[currentTrackIndex], fadeDuration)); // Start fade-in effect
        }
    }

    private IEnumerator FadeIn(AudioSource track, float duration)
    {
        float startVolume = 0f;
        track.volume = startVolume;
        float targetVolume = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            track.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for next frame
        }

        track.volume = targetVolume; // Ensure final volume is set
    }
}

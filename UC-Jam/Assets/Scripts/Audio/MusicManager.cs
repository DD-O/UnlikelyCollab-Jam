using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    public AudioSource[] musicTracks; // Assign AudioSources in the inspector
    public float fadeDuration = 2f;   // Time in seconds for the fade-in effect
    public float musicVolume = 1f;    // Set this in the inspector for volume control

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
            musicTracks[i].volume = (i == 0) ? musicVolume : 0f; // Use musicVolume instead of 1f
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
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            track.volume = Mathf.Lerp(startVolume, musicVolume, elapsedTime / duration); // Fade to musicVolume
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for next frame
        }

        track.volume = musicVolume; // Ensure final volume is set to the inspector's value
    }
}

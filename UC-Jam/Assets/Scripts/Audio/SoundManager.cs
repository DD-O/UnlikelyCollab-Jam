using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("SFX Settings")]
    public List<SFXEntry> sfxClipsList;  // List to store SFX clips for easier Inspector editing
    private Dictionary<string, SFXEntry> sfxClipsDict;  // Dictionary for fast access by name

    [Header("Audio Settings")]
    [Range(0f, 1f)] public float sfxVolume = 1f;  // Global volume for sound effects

    private AudioSource sfxSource;  // The AudioSource used to play SFX globally

    private void Awake()
    {
        // Ensure that only one instance of SoundManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep the SoundManager across scenes
        }
        else
        {
            Destroy(gameObject); // If another instance exists, destroy this one
        }

        // Initialize the dictionary
        sfxClipsDict = new Dictionary<string, SFXEntry>();

        // Populate the dictionary from the List of SFX
        foreach (var entry in sfxClipsList)
        {
            sfxClipsDict[entry.soundName] = entry;
        }

        // Initialize the AudioSource (used for global SFX)
        sfxSource = gameObject.AddComponent<AudioSource>();
    }

    // Add a sound to the dictionary (can be called programmatically)
    public void AddSFX(string soundName, AudioClip clip, float volume = 1f)
    {
        if (!sfxClipsDict.ContainsKey(soundName))
        {
            sfxClipsDict.Add(soundName, new SFXEntry { soundName = soundName, clip = clip, volume = volume });
        }
    }

    // Play a sound effect by name using the provided AudioSource
    public void PlaySound(string soundName, AudioSource targetAudioSource)
    {
        if (sfxClipsDict.ContainsKey(soundName))
        {
            SFXEntry entry = sfxClipsDict[soundName];
            targetAudioSource.PlayOneShot(entry.clip, entry.volume * sfxVolume);  // Apply individual volume and global volume
        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' not found in SFX dictionary!");
        }
    }
}

[System.Serializable]
public class SFXEntry
{
    public string soundName;  // Name of the sound to reference
    public AudioClip clip;    // The actual AudioClip to play
    [Range(0f, 1f)] public float volume = 1f;  // Volume for this specific sound, default to 1 (full volume)
}

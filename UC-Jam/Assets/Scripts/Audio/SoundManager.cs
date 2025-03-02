using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioSource audioSourcePrefab;
    [SerializeField] private int initialPoolSize = 5;
    [SerializeField] private AudioClip[] soundClips; // Assign in Inspector

    private List<AudioSource> audioPool = new List<AudioSource>();
    private Dictionary<string, AudioClip> soundLibrary = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializePool();
            AutoRegisterSounds();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            AddAudioSourceToPool();
        }
    }

    private void AutoRegisterSounds()
    {
        foreach (AudioClip clip in soundClips)
        {
            soundLibrary[clip.name] = clip; // Use filename as key
        }
    }

    private AudioSource AddAudioSourceToPool()
    {
        AudioSource newSource = Instantiate(audioSourcePrefab, transform);
        newSource.gameObject.SetActive(false);
        audioPool.Add(newSource);
        return newSource;
    }

    private AudioSource GetAvailableAudioSource()
    {
        foreach (var source in audioPool)
        {
            if (!source.isPlaying)
            {
                source.gameObject.SetActive(true);
                return source;
            }
        }
        return AddAudioSourceToPool();
    }

    public void PlaySound(string soundName, bool allowOverlap = false, float volume = 1f, float pitch = 1f)
    {
        if (soundLibrary.TryGetValue(soundName, out AudioClip clip))
        {
            if (!allowOverlap)
            {
                foreach (var activeSource in audioPool) // Renamed from 'source' to 'activeSource'
                {
                    if (activeSource.isPlaying && activeSource.clip == clip)
                    {
                        return; // Exit if the sound is already playing
                    }
                }
            }

            // Get an available AudioSource and play the sound
            AudioSource newSource = GetAvailableAudioSource();
            newSource.clip = clip;
            newSource.volume = volume;
            newSource.pitch = pitch;
            newSource.Play();
            StartCoroutine(DeactivateAfterPlaying(newSource));
        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' not found!");
        }
    }


    private IEnumerator DeactivateAfterPlaying(AudioSource source)
    {
        yield return new WaitWhile(() => source.isPlaying);
        source.gameObject.SetActive(false);
    }
}

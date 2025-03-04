using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioSource audioSourcePrefab;
    [SerializeField] private int initialPoolSize = 5;

    [System.Serializable]
    public struct SoundClip
    {
        public AudioClip clip;   // Sound file
        [Range(0f, 1f)] public float volume; // Volume for this sound
    }

    [SerializeField] private SoundClip[] soundClips; // Assign in Inspector

    private List<AudioSource> audioPool = new List<AudioSource>();
    private Dictionary<string, SoundClip> soundLibrary = new Dictionary<string, SoundClip>();

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
        foreach (SoundClip sound in soundClips)
        {
            if (sound.clip != null)
            {
                soundLibrary[sound.clip.name] = sound; // Use the clip's filename as the key
            }
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

    public void PlaySound(string clipName, bool allowOverlap = false, float overrideVolume = -1f, float pitch = 1f)
    {
        if (soundLibrary.TryGetValue(clipName, out SoundClip sound))
        {
            if (!allowOverlap)
            {
                foreach (var activeSource in audioPool)
                {
                    if (activeSource.isPlaying && activeSource.clip == sound.clip)
                    {
                        return; // Exit if the sound is already playing
                    }
                }
            }

            AudioSource newSource = GetAvailableAudioSource();
            newSource.clip = sound.clip;
            newSource.pitch = pitch;

            // Use override volume if provided, otherwise use the preset volume
            newSource.volume = (overrideVolume >= 0f) ? overrideVolume : sound.volume;

            newSource.Play();
            StartCoroutine(DeactivateAfterPlaying(newSource));
        }
        else
        {
            Debug.LogWarning($"Sound '{clipName}' not found!");
        }
    }

    private IEnumerator DeactivateAfterPlaying(AudioSource source)
    {
        yield return new WaitWhile(() => source.isPlaying);
        source.gameObject.SetActive(false);
    }
}

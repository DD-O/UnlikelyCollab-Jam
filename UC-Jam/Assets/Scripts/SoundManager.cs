using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public AudioSource baselineTrack;
    public List<AudioSource> musicTracks;

    private int currentTrackIndex = 0;

    void Start()
    {
        Debug.Log("SoundManager initialized.");

        if (baselineTrack)
        {
            baselineTrack.loop = true;
            baselineTrack.mute = false;  // Always unmuted
            baselineTrack.Play();
            Debug.Log("Baseline track started.");
        }

        foreach (var track in musicTracks)
        {
            track.loop = true;
            track.Play();
            track.mute = true; // Keep all tracks muted initially
        }

        if (musicTracks.Count > 0)
        {
            musicTracks[0].mute = false;  // Start with Track A unmuted
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) CycleTrack(1);  // Right-click to go to next track
        if (Input.GetMouseButtonDown(0)) CycleTrack(-1); // Left-click to go to previous track
    }

    private void CycleTrack(int direction)
    {
        if (musicTracks.Count == 0) return;

        // Get the current track index to mute it later
        int previousTrackIndex = currentTrackIndex;

        // Calculate the new track index with cycling logic (wrap around)
        currentTrackIndex = (currentTrackIndex + direction + musicTracks.Count) % musicTracks.Count;

        Debug.Log($"Switching from track {previousTrackIndex} to track {currentTrackIndex}");

        // Mute the previous track
        musicTracks[previousTrackIndex].mute = true;

        // Unmute the new track
        musicTracks[currentTrackIndex].mute = false;
    }
}

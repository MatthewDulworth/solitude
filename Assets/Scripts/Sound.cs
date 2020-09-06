using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

/**
 * Stores the information for a sound clip.
 * Can play or pause.
 */
[Serializable]
public class Sound
{
    private AudioSource source;
    public string name;
    public AudioClip clip;
    public bool loop;
    
    [Range(0f, 1f)] public float volume;
    [Range(.1f, 3f)] public float pitch;
    
    public void Init(AudioSource src)
    {
        source = src;
        source.playOnAwake = false;
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.loop = loop;
    }

    public void Play()
    {
        if (!source.isPlaying)
            source.Play();
    }

    public void Pause()
    {
        if(source.isPlaying)
            source.Pause();
    }
}

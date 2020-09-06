using Unity.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    [SerializeField] private Sound[] sounds = null;
    
    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds) 
            s.Init(gameObject.AddComponent<AudioSource>());
    }

    private void Start()
    {
        Play("note");
        Play("wind");
    }

    public void Play(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        s?.Play();
    }

    public void Pause(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        s?.Pause();
    }
}

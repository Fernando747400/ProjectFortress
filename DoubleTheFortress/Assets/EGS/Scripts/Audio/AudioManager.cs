using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Audio[] audios;

    
    void Awake()
    {
        foreach (Audio a in audios)
        {
            a.source = gameObject.AddComponent<AudioSource>();
            a.source.clip = a.clip;

            a.source.volume = a.volume;
            a.source.pitch = a.pitch;
            a.source.loop = a.loop;
        }
    }

    public void Play(string name)
    {
        Audio a = Array.Find(audios, audio => audio.name == name);
        if (a==null)
        {
            Debug.LogWarning("Audio: " + name + " not found!");
            return;
        }
        a.source.Play();
    }
}

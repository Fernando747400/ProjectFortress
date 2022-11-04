using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Dependencies")]
    [Header("Music Clips")]
    [SerializeField] private List<AudioClip> _musicClips = new List<AudioClip>();

    [Header("Settings")]
    [SerializeField] private bool _playRandom;
    [SerializeField] private int _musicIndex;
    
    private AudioSource _musicSource;
    
    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
        } else
        {
            Destroy(this);
        }
        _musicSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        Prepare();
    }

    public void PlayAudio(AudioClip clip, float volume = 1f, Vector3 position = default(Vector3))
    {
        AudioSource.PlayClipAtPoint(clip, position, volume);
    }

    private void Prepare()
    {
        if (_musicClips.Count == 0) throw new Exception("Audio list is empty");
        if (_playRandom) PlayRandomMusic();
        if (!_playRandom) PlayFromIndex();
    }

    private void PlayRandomMusic()
    {
        _musicSource.clip = _musicClips[UnityEngine.Random.Range(0, _musicClips.Count)];
        _musicSource.loop = true;
        _musicSource.Play();
    }

    private void PlayFromIndex()
    {
        if (_musicIndex > 0 || _musicIndex > _musicClips.Count - 1) throw new IndexOutOfRangeException("Audio clip index out of range");
        _musicSource.clip = _musicClips[_musicIndex];
        _musicSource.loop = true;
        _musicSource.Play();
    }
}

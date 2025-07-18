using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum SoundType { Good, Bad, Music, UI, Highscore }

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Singleton { get; private set; }

    [Header("Current Sounds")]
    [SerializeField] private AudioClip[] usedSounds = new AudioClip[Enum.GetNames(typeof(SoundType)).Length];

    private AudioSource audioSource;
    private AudioSource musicSource;

    void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            musicSource = GameObject.Find("MUSIC_PLAYER").GetComponent<AudioSource>();
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // musicSource = GameObject.Find("MUSIC_PLAYER").GetComponent<AudioSource>();
    }

    public void UpdateSounds(SoundType sound, int theme)
    {
        foreach (SoundType soundType in Enum.GetValues(typeof(SoundType)))
        {
            if (sound == soundType && ResourceManager.Singleton.SOUNDS.TryGetValue(soundType, out AudioClip[] audioClip))
            {
                if (audioClip.Length > theme)
                {
                    // Use theme audio clip
                    usedSounds[(int)soundType] = audioClip[theme];
                }
                else
                {
                    // Default to first audio clip
                    usedSounds[(int)soundType] = audioClip[0];
                }
                return;
            }
        }
    }

    public void UpdateSounds()
    {
        foreach (SoundType soundType in Enum.GetValues(typeof(SoundType)))
        {
            if (ResourceManager.Singleton.SOUNDS.TryGetValue(soundType, out AudioClip[] audioClip))
            {
                usedSounds[(int)soundType] = audioClip[0];
            }
        }
    }

    public AudioClip GetCircleSound(bool isGoodCircle)
    {
        if (isGoodCircle)
        {
            return (usedSounds[(int)SoundType.Good]);
        }
        return (usedSounds[(int)(SoundType.Bad)]);
    }

    public void PlaySound(SoundType sound)
    {
        audioSource.PlayOneShot(usedSounds[(int)sound]);
    }

    public void PlayMusic()
    {
        musicSource.Stop();
        musicSource.clip = usedSounds[(int)SoundType.Music];
        musicSource.Play();
    }

    public void LowerBGM(bool lower = true)
    {
        if (!lower)
        {
            musicSource.volume = 1f;
            return;
        }
        musicSource.volume = 0.5f;
    }
}

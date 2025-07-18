using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum SoundType { Good, Bad, Music, UI, Highscore }

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Singleton { get; private set; }

    public bool isMuted = false;

    [Header("Current Sounds")]
    [SerializeField] private AudioClip[] usedSounds = new AudioClip[Enum.GetNames(typeof(SoundType)).Length];

    private AudioSource audioSource;

    void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void UpdateSounds(int theme)
    {
        foreach (SoundType soundType in Enum.GetValues(typeof(SoundType)))
        {
            if (ResourceManager.Singleton.SOUNDS.TryGetValue(soundType, out AudioClip[] audioClip))
            {
                Debug.Log(Enum.GetNames(typeof(SoundType)).Length);
                Debug.Log(audioClip.Length);
                Debug.Log(usedSounds.Length);
                Debug.Log((int)soundType);
                Debug.Log(audioClip[theme]);
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
            }
            // usedSounds[(int)soundType] = ResourceManager.Singleton.SOUNDS[soundType][theme];
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
        if (!isMuted)
        {
            audioSource.PlayOneShot(usedSounds[(int)sound]);   
        }
    }
}

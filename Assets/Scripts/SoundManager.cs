using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum SoundType { Good, Bad, Music, UI, Highscore, Gem}
public enum TargetType { Bad, Small, Fast, Good, NONE }

[System.Serializable]
public class TargetSound
{
    public TargetType targetType;
    public AudioClip targetSound;
    public float soundPitch;
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Singleton { get; private set; }

    public AudioClip switchModeAudio;
    public AudioClip countdownAudio;
    [SerializeField] private AudioClip[] hitSounds;
    [SerializeField] private AudioClip badSound;
    public AudioClip alarmAudio;

    [Header("Current Sounds")]
    [SerializeField] private AudioClip[] usedSounds = new AudioClip[Enum.GetNames(typeof(SoundType)).Length];

    private AudioSource audioSource;
    private AudioSource musicSource;

    [Header("Original Animal Sounds")]
    [SerializeField] private TargetSound[] targetSounds = new TargetSound[4];
    [SerializeField] private AudioClip badTargetSound;
    [SerializeField] private AudioClip smallTargetSound;  // Rabbit
    [SerializeField] private AudioClip fastTargetSound;   // Rabbit
    [SerializeField] private AudioClip goodTargetSound;   // Mole


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

    public void PlaySound(SoundType sound, float volume = 1f)
    {
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(usedSounds[(int)sound], volume);
    }

    // public void PlaySound(AudioClip sfx, float volume = 1f)
    // {
    //     if(sfx == null)
    //     {
    //         return;
    //     }
        
    //     audioSource.PlayOneShot(sfx, volume);
    // }

    public void UpdateAnimalSounds(AudioClip[] newTargetSounds, AnimalSoundSettings animal_soundSettings)
    {
        for(int i = 0; i < targetSounds.Length; i++)
        {
            if(targetSounds[i].targetSound != newTargetSounds[i])
            {
                targetSounds[i].targetSound = newTargetSounds[i];
            }

            if(targetSounds[i].soundPitch != animal_soundSettings.soundPitches[i])
            {
                targetSounds[i].soundPitch = animal_soundSettings.soundPitches[i];
            }
        }

    }

    public void PlaySound(AudioClip sfx, float volume = 1f)
    {
        if (sfx == null) return;
        
        audioSource.pitch = 1f;  // Reset to normal pitch
        audioSource.PlayOneShot(sfx);
    }

    public void PlayHitSound()
    {
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(hitSounds[UnityEngine.Random.Range(0, hitSounds.Length)], 1f);
    }

    public void PlayBadSound()
    {
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(badSound, 1f);
    }

    public void PlayTargetSound(TargetType targetType)
    {
        if(targetType == TargetType.NONE)
            return;
            
        int _type = (int)targetType;
        PlaySoundWithPitch(targetSounds[_type].targetSound, targetSounds[_type].soundPitch);
    }

    public void PlaySoundWithPitch(AudioClip clip, float pitch = 1f)
    {
        if (clip == null) return;
        
        audioSource.pitch = pitch;
        audioSource.PlayOneShot(clip);
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

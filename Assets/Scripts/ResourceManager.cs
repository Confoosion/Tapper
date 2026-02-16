using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Singleton { get; private set; }

    public Dictionary<SoundType, AudioClip[]> SOUNDS = new Dictionary<SoundType, AudioClip[]>();

    void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
    }

    void RetrieveResources()
    {
        foreach (SoundType soundType in Enum.GetValues(typeof(SoundType)))
        {
            SOUNDS.Add(soundType, Resources.LoadAll("Sounds/" + soundType.ToString(), typeof(AudioClip)).Cast<AudioClip>().ToArray());
        }

        OutputResources();
    }

    void OutputResources()
    {
        SoundManager.Singleton.UpdateSounds();
        // GameThemes.Singleton.ReceiveSprites(goodCircle_Sprites, badCircle_Sprites);
    }

    void Start()
    {
        RetrieveResources();
        // SoundManager.Singleton.PlayMusic();
    }
}

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Singleton { get; private set; }
    public Sprite[] goodCircle_Sprites;
    public Sprite[] badCircle_Sprites;

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
        goodCircle_Sprites = Resources.LoadAll("Good_Circles", typeof(Sprite)).Cast<Sprite>().ToArray();
        badCircle_Sprites = Resources.LoadAll("Bad_Circles", typeof(Sprite)).Cast<Sprite>().ToArray();

        foreach (SoundType soundType in Enum.GetValues(typeof(SoundType)))
        {
            SOUNDS.Add(soundType, Resources.LoadAll("Sounds/" + soundType.ToString(), typeof(AudioClip)).Cast<AudioClip>().ToArray());
        }

        OutputResources();
    }

    void OutputResources()
    {
        SoundManager.Singleton.UpdateSounds();
        GameThemes.Singleton.ReceiveSprites(goodCircle_Sprites, badCircle_Sprites);
    }

    void Start()
    {
        RetrieveResources();
        SoundManager.Singleton.PlayMusic();
    }
}

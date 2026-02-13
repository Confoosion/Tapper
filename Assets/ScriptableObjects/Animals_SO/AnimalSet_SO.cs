using UnityEngine;

[System.Serializable]
public class PreviewAnimalSet
{
    public Sprite[] goodTargets = new Sprite[3]; // ORDER: Small, Fast, Good
    public Sprite badTarget;
}

[System.Serializable]
public class AnimalSoundSettings
{
    [Header("Sound Pitches (1.0 = original)")]
    [Range(0f, 3f)] public float[] soundPitches = {1f, 1f, 1f, 1f};
}


[CreateAssetMenu(fileName = "AnimalSet_SO", menuName = "Scriptable Objects/Themes/AnimalSet_SO")]
public class AnimalSet_SO : ShopItem
{
    [Header("Shop Preview")]
    public PreviewAnimalSet preview_Set;
    public AudioClip[] preview_Sounds = new AudioClip[4]; // ORDER: Bad, Small, Fast, Good

    [Space]

    [Header("Animal Set")]
    public GameObject[] goodTargets = new GameObject[3];
    public GameObject badTarget;

    [Space]

    [Header("Starting Animal")]
    public Sprite enterFrame;
    public Sprite hitFrame;

    [Space]

    [Header("Sound Settings")]
    public AnimalSoundSettings soundSettings;

    public override void EquipItem()
    {
        isEquipped = true;
        ThemeManager.Singleton.EquipAnimalSet(this);
    }
}

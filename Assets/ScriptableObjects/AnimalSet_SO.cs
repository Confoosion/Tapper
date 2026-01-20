using UnityEngine;

[System.Serializable]
public class PreviewAnimalSet
{
    public Sprite[] goodTargets = new Sprite[3];
    public Sprite badTarget;
}

[CreateAssetMenu(fileName = "AnimalSet_SO", menuName = "Scriptable Objects/Themes/AnimalSet_SO")]
public class AnimalSet_SO : ScriptableObject
{
    public bool isUnlocked = false;
    public bool isEquipped = false;

    [Space]

    [Header("Shop Preview")]
    public Sprite preview_BG;
    public int preview_Price;
    public PreviewAnimalSet preview_Set;
    public AudioClip[] preview_Sounds = new AudioClip[4]; // ORDER: Bad, Small, Fast, Good

    [Space]

    [Header("Animal Set")]
    public GameObject[] goodTargets = new GameObject[3];
    public GameObject badTarget;
}

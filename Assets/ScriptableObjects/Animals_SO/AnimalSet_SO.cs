using UnityEngine;

[System.Serializable]
public class PreviewAnimalSet
{
    public Sprite[] goodTargets = new Sprite[3];
    public Sprite badTarget;
}

[CreateAssetMenu(fileName = "AnimalSet_SO", menuName = "Scriptable Objects/Themes/AnimalSet_SO")]
public class AnimalSet_SO : ShopItem
{
    // public bool isUnlocked = false;
    // public bool isEquipped = false;

    [Header("Shop Preview")]
    public Sprite preview_BG;
    public PreviewAnimalSet preview_Set;
    public AudioClip[] preview_Sounds = new AudioClip[4]; // ORDER: Bad, Small, Fast, Good

    [Space]

    [Header("Animal Set")]
    public GameObject[] goodTargets = new GameObject[3];
    public GameObject badTarget;

    public override void EquipItem()
    {
        isEquipped = true;
        ThemeManager.Singleton.EquipAnimalSet(this);
    }
}

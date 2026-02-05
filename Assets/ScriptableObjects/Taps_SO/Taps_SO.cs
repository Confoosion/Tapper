using UnityEngine;

[CreateAssetMenu(fileName = "Taps_SO", menuName = "Scriptable Objects/Themes/Taps_SO")]
public class Taps_SO : ShopItem
{
    [Header("Shop Preview")]
    public float shopPreview_Size;
    // public PreviewAnimalSet preview_Set;
    // public AudioClip[] preview_Sounds = new AudioClip[4]; // ORDER: Bad, Small, Fast, Good

    [Space]

    [Header("Tap")]
    public ParticleSystem particle;
    // public GameObject[] goodTargets = new GameObject[3];
    // public GameObject badTarget;

    // [Space]

    // [Header("Starting Animal")]
    // public Sprite enterFrame;
    // public Sprite hitFrame;

    public override void EquipItem()
    {
        isEquipped = true;
        // ThemeManager.Singleton.EquipAnimalSet(this);
    }
}

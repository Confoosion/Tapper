using UnityEngine;

public enum TapEffectType
{
    Particle,
    UniqueEffect
}

[CreateAssetMenu(fileName = "Taps_SO", menuName = "Scriptable Objects/Themes/Taps_SO")]
public class Taps_SO : ShopItem
{
    [Header("Tap Type")]
    public TapEffectType effectType;

    [Header("Tap Effect")]
    public GameObject tapPrefab;
    public string poolTag;
    public AudioClip tapSFX;

    public override void EquipItem()
    {
        isEquipped = true;
        ThemeManager.Singleton.EquipTap(this);
    }
}

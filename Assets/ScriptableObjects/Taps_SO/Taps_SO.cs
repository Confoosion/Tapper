using UnityEngine;

[CreateAssetMenu(fileName = "Taps_SO", menuName = "Scriptable Objects/Themes/Taps_SO")]
public class Taps_SO : ShopItem
{
    [Header("Tap Particle")]
    public GameObject particlePrefab;
    public string poolTag;
    public UniqueTap specialEffect;

    public override void EquipItem()
    {
        isEquipped = true;
        ThemeManager.Singleton.EquipTap(this);
    }
}

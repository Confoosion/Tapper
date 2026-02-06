using UnityEngine;

[CreateAssetMenu(fileName = "Taps_SO", menuName = "Scriptable Objects/Themes/Taps_SO")]
public class Taps_SO : ShopItem
{
    [Header("Tap")]
    public ParticleSystem particle;

    public override void EquipItem()
    {
        isEquipped = true;
        // ThemeManager.Singleton.EquipAnimalSet(this);
    }
}

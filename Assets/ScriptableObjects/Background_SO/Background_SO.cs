using UnityEngine;

[CreateAssetMenu(fileName = "Background_SO", menuName = "Scriptable Objects/Themes/Background_SO")]
public class Background_SO : ShopItem
{
    [Header("Shop Preview")]
    public Sprite preview_BG;
    public string preview_ObjectName;

    [Space]    

    [Header("Background Set")]
    public Sprite dirt;
    public Sprite grass;
    public Sprite sky;
    public Sprite mainMenuBG;
    public Sprite menuBG;
    public Sprite gameOverDetails;

    [Header("Music")]
    public AudioClip backgroundBGM;

    public override void EquipItem()
    {
        isEquipped = true;
        ThemeManager.Singleton.EquipBackgroundSet(this);
    }
}

using UnityEngine;

[System.Serializable]
public class ExtraBackgroundDetail
{
    public GameObject detailObject;
    public Vector2 detailPosition;
}

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
    public Sprite details;
    public Sprite sky;
    public Sprite mainMenuBG;
    public Sprite menuBG;
    public Sprite gameOverDetails;

    [Header("Music")]
    public AudioClip backgroundBGM;

    [Space]

    [Header("Extra Details")]
    public ExtraBackgroundDetail[] backgroundDetails;


    public override void EquipItem()
    {
        isEquipped = true;
        ThemeManager.Singleton.EquipBackgroundSet(this);
    }
}

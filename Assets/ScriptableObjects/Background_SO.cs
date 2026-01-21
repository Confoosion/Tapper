using UnityEngine;

[CreateAssetMenu(fileName = "Background_SO", menuName = "Scriptable Objects/Themes/Background_SO")]
public class Background_SO : ScriptableObject
{
    public bool isUnlocked = false;
    public bool isEquipped = false;

    [Space]

    [Header("Shop Preview")]
    public Sprite preview_BG;
    public int preview_Price;

    [Space]

    [Header("Background")]
    public Sprite ground;
    public Sprite moreGround;
    public Sprite groundDetails;
    public Sprite menuBackground;
    public Sprite sky;
}

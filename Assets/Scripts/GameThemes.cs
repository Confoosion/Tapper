using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class GameThemes : MonoBehaviour
{
    public static GameThemes Singleton { get; private set; }
    public enum CosmeticType { Good, Bad };

    [Header("Game Theme UI")]
    [SerializeField] private TextMeshProUGUI themeIndex;
    [SerializeField] private TextMeshProUGUI themeLabel;

    [SerializeField] private Image good_Image;
    [SerializeField] private Image bad_Image;
    [SerializeField] private GameObject good_Object;
    [SerializeField] private GameObject bad_Object;

    [Header("Themes")]
    [SerializeField] private CosmeticType currCosmetic;
    [SerializeField] private GameObject currentThemeObject;
    private Sprite[] goodCircles;
    [SerializeField] private int goodIndex;
    private Sprite[] badCircles;
    [SerializeField] private int badIndex;

    void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
    }

    public void ReceiveSprites(Sprite[] good, Sprite[] bad)
    {
        goodCircles = good;
        badCircles = bad;

        if (!PlayerPrefs.HasKey("GoodSkin_Index"))
        {
            PlayerPrefs.SetInt("GoodSkin_Index", 0);
        }
        SwitchSprite(0);

        SwitchAssetPicker(CosmeticType.Bad, bad_Object);

        if (!PlayerPrefs.HasKey("BadSkin_Index"))
        {
            PlayerPrefs.SetInt("BadSkin_Index", 0);
        }
        SwitchSprite(0);

        SwitchAssetPicker(CosmeticType.Good, good_Object);
        SetImages();
        SetSounds();
    }

    // SelectAsset.cs uses this to switch between cosmetics
    public void SwitchAssetPicker(CosmeticType cosmetic, GameObject newThemeObject)
    {
        currentThemeObject.SetActive(false);
        newThemeObject.SetActive(true);

        currentThemeObject = newThemeObject;
        currCosmetic = cosmetic;
        switch (cosmetic)
        {
            case CosmeticType.Good:
                {
                    themeLabel.SetText(FixSpriteName(goodCircles[PlayerPrefs.GetInt("GoodSkin_Index")].name));
                    themeIndex.SetText((PlayerPrefs.GetInt("GoodSkin_Index") + 1).ToString() + " / " + goodCircles.Length.ToString());
                    break;
                }
            case CosmeticType.Bad:
                {
                    themeLabel.SetText(FixSpriteName(badCircles[PlayerPrefs.GetInt("BadSkin_Index")].name));
                    themeIndex.SetText((PlayerPrefs.GetInt("BadSkin_Index") + 1).ToString() + " / " + badCircles.Length.ToString());
                    break;
                }
        }
    }

    public void SwitchSprite(int direction)
    {
        switch (currCosmetic)
        {
            case CosmeticType.Good:
                {
                    goodIndex = (PlayerPrefs.GetInt("GoodSkin_Index") + direction) % goodCircles.Length;
                    if (goodIndex < 0)
                    {
                        PlayerPrefs.SetInt("GoodSkin_Index", goodCircles.Length - 1);
                    }
                    else
                    {
                        PlayerPrefs.SetInt("GoodSkin_Index", goodIndex);
                    }
                    themeLabel.SetText(FixSpriteName(goodCircles[PlayerPrefs.GetInt("GoodSkin_Index")].name));
                    themeIndex.SetText(FixSpriteIndex(PlayerPrefs.GetInt("GoodSkin_Index"), goodCircles.Length));
                    good_Image.sprite = goodCircles[PlayerPrefs.GetInt("GoodSkin_Index")];
                    break;
                }
            case CosmeticType.Bad:
                {
                    badIndex = (PlayerPrefs.GetInt("BadSkin_Index") + direction) % badCircles.Length;
                    if (badIndex < 0)
                    {
                        PlayerPrefs.SetInt("BadSkin_Index", badCircles.Length - 1);
                    }
                    else
                    {
                        PlayerPrefs.SetInt("BadSkin_Index", badIndex);
                    }
                    themeLabel.SetText(FixSpriteName(badCircles[PlayerPrefs.GetInt("BadSkin_Index")].name));
                    themeIndex.SetText(FixSpriteIndex(PlayerPrefs.GetInt("BadSkin_Index"), badCircles.Length));
                    bad_Image.sprite = badCircles[PlayerPrefs.GetInt("BadSkin_Index")];
                    break;
                }
        }
    }

    private string FixSpriteName(string spriteName)
    {
        string fixedName = spriteName.Substring(2);
        return (fixedName);
    }

    private string FixSpriteIndex(int index, int length)
    {
        string fixedIndex = (index + 1).ToString() + " / " + length.ToString();
        return (fixedIndex);
    }

    public void SetImages()
    {
        UIManager.Singleton.UpdateGameTheme(good_Image.sprite, bad_Image.sprite);
    }

    public void SetSounds()
    {
        SoundManager.Singleton.UpdateSounds(SoundType.Good, PlayerPrefs.GetInt("GoodSkin_Index"));
        SoundManager.Singleton.UpdateSounds(SoundType.Bad, PlayerPrefs.GetInt("BadSkin_Index"));
    }
}

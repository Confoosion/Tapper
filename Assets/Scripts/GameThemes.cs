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
    }

    public void SwitchThemes(CosmeticType cosmetic, GameObject newThemeObject)
    {
        currentThemeObject.SetActive(false);
        newThemeObject.SetActive(true);

        currentThemeObject = newThemeObject;
        currCosmetic = cosmetic;
        switch (cosmetic)
        {
            case CosmeticType.Good:
                {
                    themeLabel.SetText(FixSpriteName(goodCircles[goodIndex].name));
                    themeIndex.SetText((goodIndex + 1).ToString() + " / " + goodCircles.Length.ToString());
                    break;
                }
            case CosmeticType.Bad:
                {
                    themeLabel.SetText(FixSpriteName(badCircles[badIndex].name));
                    themeIndex.SetText((badIndex + 1).ToString() + " / " + badCircles.Length.ToString());
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
                    goodIndex = (goodIndex + direction) % goodCircles.Length;
                    if (goodIndex < 0)
                    {
                        goodIndex = goodCircles.Length - 1;
                    }
                    themeLabel.SetText(FixSpriteName(goodCircles[goodIndex].name));
                    themeIndex.SetText((goodIndex + 1).ToString() + " / " + goodCircles.Length.ToString());
                    good_Image.sprite = goodCircles[goodIndex];
                    break;
                }
            case CosmeticType.Bad:
                {
                    badIndex = (badIndex + direction) % badCircles.Length;
                    if (badIndex < 0)
                    {
                        badIndex = badCircles.Length - 1;
                    }
                    themeLabel.SetText(FixSpriteName(badCircles[badIndex].name));
                    themeIndex.SetText((badIndex + 1).ToString() + " / " + badCircles.Length.ToString());
                    bad_Image.sprite = badCircles[badIndex];
                    break;
                }
        }
    }

    private string FixSpriteName(string spriteName)
    {
        string fixedName = spriteName.Substring(2);
        return (fixedName);
    }

    public void SetImages()
    {
        UIManager.Singleton.UpdateGameTheme(good_Image.sprite, bad_Image.sprite);
    }

    public void SetSounds()
    {
        SoundManager.Singleton.UpdateSounds(SoundType.Good, goodIndex);
        SoundManager.Singleton.UpdateSounds(SoundType.Bad, badIndex);
    }
}

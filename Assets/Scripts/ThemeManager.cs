using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class AnimalSet
{
    public GameObject[] goodTargets = new GameObject[3];
    public GameObject badTarget;
}

[System.Serializable]
public class Background
{
    public Sprite ground;
    public Sprite extraGround;
    public Sprite sky;
    public Sprite groundDetails;
    public Sprite menuBackgrounds;
}

public class ThemeManager : MonoBehaviour
{
    public static ThemeManager Singleton;

    void Awake()
    {
        if(Singleton == null)
        {
            Singleton = this;
        }
    }

    [SerializeField] AnimalSet currentAnimalSet;
    private AnimalSet_SO currentAnimalSet_SO;
    [SerializeField] Background currentBackground;

    // ========== CHANGED: Removed isEquipped modification ==========
    public void EquipAnimalSet(AnimalSet_SO animalSet)
    {
        // Just store reference and apply theme
        // ShopManager handles isEquipped flags now
        currentAnimalSet_SO = animalSet;

        currentAnimalSet.goodTargets = currentAnimalSet_SO.goodTargets;
        currentAnimalSet.badTarget = currentAnimalSet_SO.badTarget;
    }
}
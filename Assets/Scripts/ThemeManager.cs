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

    [SerializeField] AnimalSet currentAnimalSet;
    [SerializeField] Background currentBackground;

    void Awake()
    {
        if(Singleton == null)
        {
            Singleton = this;
        }
    }

    
}

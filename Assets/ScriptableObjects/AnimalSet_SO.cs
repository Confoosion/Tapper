using UnityEngine;

[CreateAssetMenu(fileName = "AnimalSet_SO", menuName = "Scriptable Objects/Themes/AnimalSet_SO")]
public class AnimalSet_SO : ScriptableObject
{
    public bool isUnlocked = false;
    public bool isEquipped = false;

    [Space]

    [Header("Shop Preview")]
    public Sprite preview_BG;
    public Sprite preview_Set;
    public int preview_Price;

    [Space]

    [Header("Animal Set")]
    public GameObject[] goodTargets = new GameObject[3];
    public GameObject badTarget;
}

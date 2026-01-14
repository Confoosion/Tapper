using UnityEngine;

[CreateAssetMenu(fileName = "AnimalSet_SO", menuName = "Scriptable Objects/Themes/AnimalSet_SO")]
public class AnimalSet_SO : ScriptableObject
{
    public GameObject[] goodTargets = new GameObject[3];
    public GameObject badTarget;
}

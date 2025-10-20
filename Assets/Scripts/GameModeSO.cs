using UnityEngine;

[CreateAssetMenu(fileName = "GameModeSO", menuName = "Scriptable Objects/GameModeSO")]
public class GameModeSO : ScriptableObject
{
    public string modeName;
    public float badSpawnPercentage;
    public float decayRate;
    public bool doGraceSpawns;
    public bool isTimed;
}

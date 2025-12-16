using UnityEngine;

[CreateAssetMenu(fileName = "GameModeSO", menuName = "Scriptable Objects/GameModeSO")]
public class GameModeSO : ScriptableObject
{
    public string modeName;
    public int lives = 3;
    public float badSpawnPercentage;
    public float mushroomSpawnPercentage;
    public float decayRate;
    public bool doGraceSpawns;
    public bool isTimed;
}

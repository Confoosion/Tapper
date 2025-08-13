using UnityEngine;

[CreateAssetMenu(menuName = "Game/Mode Config", fileName = "GameModeConfig")]
public class GameModeConfig : ScriptableObject
{
    [Header("Game Mode Name")]
    public GameMode gameMode;

    [Header("Runner prefab (has a *runner* script on it)")]
    public GameObject runnerPrefab;

    [Header("Spawn area + prefabs")]
    public RectTransform spawnAreaOverride;   // optional; otherwise SpawnManager.spawnArea is used
    public GameObject primaryPrefab;
    public GameObject alternatePrefab;        // optional (Classic uses it, Rain doesn’t)

    [Header("Intervals")]
    public float maxSpawnInterval = 1.5f;
    public float minSpawnInterval = 0.2f;
    public float decayRate = 0.05f;           // used by exp decay

    [Header("Classic-only")]
    [Range(0f, 1f)] public float badPercentage = 0.1f;

    [Header("Rain-only")]
    public int minTaps = 1;
    public int maxTaps = 1;
    public int maxTapCap = 8;
    public float tapIncreasePeriod = 3.5f;    // seconds * current maxTaps
    [Range(0f, 1f)] public float doubleSpawnChance = 0.25f;

    [Header("Flick-only")]
    public RectTransform[] spawners;
}

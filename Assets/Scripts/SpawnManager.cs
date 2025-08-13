using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Singleton { get; private set; }

    [Header("Default spawn area (can be overridden per mode)")]
    public RectTransform spawnArea;

    [Header("Active state (read-only)")]
    [SerializeField] private bool isSpawning;

    [Header("Mode assets")]
    public GameModeConfig classicConfig;
    public GameModeConfig rainConfig;

    IGameModeRunner activeRunner;
    Coroutine runRoutine;

    void Awake()
    {
        if (Singleton == null) Singleton = this;
    }

    public void StartSpawning(GameMode mode)
    {
        StopSpawning();

        GameModeConfig config = GetGameConfig(mode); 

        // instantiate the runner prefab and init
        var runnerGO = Instantiate(config.runnerPrefab, transform);
        activeRunner = runnerGO.GetComponent<IGameModeRunner>();
        if (activeRunner == null)
        {
            Debug.LogError("Runner prefab is missing an IGameModeRunner component.");
            Destroy(runnerGO);
            return;
        }

        activeRunner.Init(this, config);
        isSpawning = true;
        runRoutine = StartCoroutine(activeRunner.Run());
    }

    public void StopSpawning()
    {
        if (!isSpawning) return;

        if (activeRunner != null)
        {
            activeRunner.StopMode();
        }

        if (runRoutine != null)
        {
            StopCoroutine(runRoutine);
            runRoutine = null;
        }

        // destroy previous runner instance if we created one
        var mb = activeRunner as MonoBehaviour;
        if (mb) Destroy(mb.gameObject);

        activeRunner = null;
        isSpawning = false;
    }

    GameModeConfig GetGameConfig(GameMode mode)
    {
        switch (mode)
        {
            case GameMode.Classic:
                return classicConfig;
            case GameMode.Rain:
                return rainConfig;
        }
        return null;
    }

    // Convenience wrappers if you still want enum buttons etc.
    public void StartClassic() => StartSpawning(GameMode.Classic);
    public void StartRain()    => StartSpawning(GameMode.Rain);
}

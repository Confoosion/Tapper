using System.Collections;
using UnityEngine;

public interface IGameModeRunner
{
    void Init(SpawnManager mgr, GameModeConfig cfg);
    IEnumerator Run();         // main loop
    void StopMode();           // cleanup if needed
}

public abstract class GameModeBase : MonoBehaviour, IGameModeRunner
{
    protected GameMode mode;
    protected SpawnManager manager;
    protected GameModeConfig config;
    protected RectTransform spawnArea;
    protected bool running;
    protected float timeInMode;
    protected float currentInterval;

    public virtual void Init(SpawnManager mgr, GameModeConfig cfg)
    {
        manager = mgr;
        config = cfg;
        mode = cfg.gameMode;
        spawnArea = cfg.spawnAreaOverride ? cfg.spawnAreaOverride : mgr.spawnArea;
        timeInMode = 0f;
        currentInterval = cfg.maxSpawnInterval;
        running = true;
    }

    protected Vector2 RandomLocalPosIn(RectTransform rt)
    {
        var w = rt.rect.width;
        var h = rt.rect.height;
        return new Vector2(
            Random.Range(-w * 0.5f, w * 0.5f),
            Random.Range(-h * 0.5f, h * 0.5f)
        );
    }

    protected void TickInterval(float dt)
    {
        timeInMode += dt;
        // exp decay toward min
        currentInterval = Mathf.Max(config.minSpawnInterval,
            config.maxSpawnInterval * Mathf.Exp(-config.decayRate * timeInMode));
    }

    protected void UpdateSpawn(SpawnArea spawner)
    {
        spawner.UpdateSpawnArea(mode);
    }

    public abstract IEnumerator Run();
    public virtual void StopMode() { running = false; }
}

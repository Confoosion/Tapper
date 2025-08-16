using System.Collections;
using UnityEngine;

public class Flick_GameMode : GameModeBase
{
    float rightPercentage;
    int currentSpawns, MAX_SPAWNS;

    public override void Init(SpawnManager mgr, GameModeConfig cfg)
    {
        base.Init(mgr, cfg);
        currentSpawns = cfg.minSpawns;
        MAX_SPAWNS = cfg.maxSpawns;
        rightPercentage = 0.5f;
    }

    public override IEnumerator Run()
    {
        UpdateSpawn(spawnArea.GetComponent<SpawnArea>());
        Debug.Log("Flick mode running");
        while (running && GameManager.Singleton.CheckPlayerStatus())
        {
            // choose prefab
            for (int i = 0; i < currentSpawns; i++)
            {
                var spawnLeft = Random.Range(0f, 1f) > rightPercentage;
                var prefab = spawnLeft ? config.primaryPrefab : config.alternatePrefab;
                var go = Instantiate(prefab, spawnArea);
                go.transform.localPosition = RandomLocalPosIn(spawnArea);
            }

            yield return new WaitForSeconds(currentInterval);
            TickInterval(Time.deltaTime + currentInterval); // advance time by the interval we just waited
        }

        Debug.Log("Flick mode ended");
    }
}

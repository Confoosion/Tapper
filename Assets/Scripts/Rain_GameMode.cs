using System.Collections;
using UnityEngine;

public class Rain_GameMode : GameModeBase
{
    int minTaps, maxTaps;
    float tapTimer;

    public override void Init(SpawnManager mgr, GameModeConfig cfg)
    {
        base.Init(mgr, cfg);
        minTaps = cfg.minTaps;
        maxTaps = cfg.maxTaps;
        tapTimer = 0f;
    }

    public override IEnumerator Run()
    {
        UpdateSpawn(spawnArea.GetComponent<SpawnArea>());
        Debug.Log("Rain mode running");
        bool carryDouble = false;

        while (running && GameManager.Singleton.CheckPlayerStatus())
        {
            // spawn one
            var go = Instantiate(config.primaryPrefab, spawnArea);
            go.transform.localPosition = RandomLocalPosIn(spawnArea);

            // give taps to the behavior if present
            var rain = go.GetComponent<CircleRainBehavior>();
            if (rain) rain.SetupBehavior(minTaps, maxTaps);

            // maybe spawn a second immediately
            if (Random.value <= config.doubleSpawnChance && !carryDouble)
            {
                carryDouble = true;
            }
            else
            {
                carryDouble = false;
                yield return new WaitForSeconds(currentInterval);
                TickInterval(Time.deltaTime + currentInterval);
            }

            // handle tap escalation
            tapTimer += Time.deltaTime + currentInterval;
            var period = config.tapIncreasePeriod * Mathf.Max(1, maxTaps);
            if (tapTimer >= period && maxTaps < config.maxTapCap)
            {
                maxTaps++;
                tapTimer = 0f;
            }
        }

        Debug.Log("Rain mode ended");
    }

    public override void StopMode()
    {
        base.StopMode();
        // any cleanup if needed
    }
}

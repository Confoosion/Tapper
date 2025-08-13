using System.Collections;
using UnityEngine;

public class Classic_GameMode : GameModeBase
{
    public override IEnumerator Run()
    {
        UpdateSpawn(spawnArea.GetComponent<SpawnArea>());
        Debug.Log("Classic mode running");
        while (running && GameManager.Singleton.CheckPlayerStatus())
        {
            // choose prefab
            var spawnGood = Random.Range(0f, 1f) > config.badPercentage;
            var prefab = spawnGood ? config.primaryPrefab : config.alternatePrefab;
            var go = Instantiate(prefab, spawnArea);
            go.transform.localPosition = RandomLocalPosIn(spawnArea);

            yield return new WaitForSeconds(currentInterval);
            TickInterval(Time.deltaTime + currentInterval); // advance time by the interval we just waited
        }

        Debug.Log("Classic mode ended");
    }
}

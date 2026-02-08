using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class AnimalSet
{
    public GameObject[] goodTargets = new GameObject[3];
    public GameObject badTarget;
    public Sprite start_EnterFrame;
    public Sprite start_HitFrame;
}

[System.Serializable]
public class Background
{
    public Sprite dirt;
    public Sprite grass;
    public Sprite details;
    public Sprite sky;
    public Sprite mainMenuBG;
    public Sprite menuBG;
}

public class ThemeManager : MonoBehaviour
{
    public static ThemeManager Singleton;

    void Awake()
    {
        if(Singleton == null)
        {
            Singleton = this;
        }
    }

    [SerializeField] AnimalSet currentAnimalSet;
    private AnimalSet_SO currentAnimalSet_SO;

    [SerializeField] Background currentBackground;
    private Background_SO currentBackground_SO;

    private string currentTapPoolTag;
    private Taps_SO currentTap_SO;



    public void EquipAnimalSet(AnimalSet_SO animalSet)
    {
        // Just store reference and apply theme
        // ShopManager handles isEquipped flags now
        currentAnimalSet_SO = animalSet;

        currentAnimalSet.goodTargets = currentAnimalSet_SO.goodTargets;
        currentAnimalSet.badTarget = currentAnimalSet_SO.badTarget;
        currentAnimalSet.start_EnterFrame = currentAnimalSet_SO.enterFrame;
        currentAnimalSet.start_HitFrame = currentAnimalSet_SO.hitFrame;

        // Update object pools with new animal prefabs
        UpdateObjectPools(animalSet);
        
        // Update spawn manager references
        UpdateSpawnManager(animalSet);
    }

    public void EquipBackgroundSet(Background_SO background)
    {
        currentBackground.dirt = background.dirt;
        currentBackground.grass = background.grass;
        currentBackground.details = background.details;
        currentBackground.sky = background.sky;
        currentBackground.mainMenuBG = background.mainMenuBG;
        currentBackground.menuBG = background.menuBG;

        // Show the visuals
        UIManager.Singleton.SwitchBackgrounds(background);
    }

    public void EquipTap(Taps_SO tap)
    {
        currentTap_SO = tap;
        currentTapPoolTag = tap.poolTag;
    }

    // Updating Animal ObjectPools
    private void UpdateObjectPools(AnimalSet_SO animalSet)
    {
        if (ObjectPoolManager.Instance == null)
        {
            Debug.LogWarning("ObjectPoolManager not found! Pools not updated.");
            return;
        }
        
        // Update the pool prefabs for good targets
        for (int i = 0; i < animalSet.goodTargets.Length; i++)
        {
            string poolTag = GetGoodTargetTag(i);
            ObjectPoolManager.Instance.UpdatePoolPrefab(poolTag, animalSet.goodTargets[i]);
        }
        
        // Update bad target pool
        ObjectPoolManager.Instance.UpdatePoolPrefab("Bomb", animalSet.badTarget);
        
        Debug.Log($"Object pools updated for {animalSet.name}");
    }

    private void UpdateSpawnManager(AnimalSet_SO animalSet)
    {
        if (SpawnManager.Singleton == null)
        {
            Debug.LogWarning("SpawnManager not found! Spawn references not updated.");
            return;
        }
        
        SpawnManager.Singleton.UpdateAnimalReferences(animalSet.goodTargets, animalSet.badTarget);
        Debug.Log($"SpawnManager updated for {animalSet.name}");
    }

    private string GetGoodTargetTag(int index)
    {
        // Match the tags in ObjectPoolManager
        string[] tags = { "Mole", "Mouse", "Rabbit" };
        return index < tags.Length ? tags[index] : $"GoodTarget{index}";
    }

    // Playing equipped Tap particles
    public void PlayTapEffect(Vector2 position)
    {
        if (string.IsNullOrEmpty(currentTapPoolTag))
        {
            Debug.LogWarning("No tap effect equipped!");
            return;
        }
        
        // Spawn particle from the equipped tap's pool
        GameObject particleObj = ObjectPoolManager.Instance.SpawnFromPool(currentTapPoolTag, position);
        if(particleObj == null)
            return;
        
        if(currentTap_SO.specialEffect != null) // Play special Tap effect (Overwrites particle system)
        {
            currentTap_SO.specialEffect.PlayTap();
        }
        else // Normal particle Taps
        {   
            ParticleSystem ps = particleObj.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Clear();
                ps.Play();
                
                // Auto-return to pool when particle finishes
                StartCoroutine(ReturnTapToPool(particleObj, ps));
            }
            else
            {
                Debug.LogWarning($"No ParticleSystem found on {particleObj.name}");
                ObjectPoolManager.Instance.ReturnToPool(particleObj);
            }
        }
    }

    IEnumerator ReturnTapToPool(GameObject obj, ParticleSystem ps)
    {
        // Wait for particle to finish playing
        float duration = ps.main.duration;
        float lifetime = ps.main.startLifetime.constantMax;
        
        yield return new WaitForSeconds(duration + lifetime);
        
        // Make sure it's completely done
        while (ps.IsAlive())
        {
            yield return null;
        }
        
        // Return to pool
        ObjectPoolManager.Instance.ReturnToPool(obj);
    }

    public AnimalSet GetAnimalSet()
    {
        return(currentAnimalSet);
    }

    public Background GetBackground()
    {
        return currentBackground;
    }

    public Taps_SO GetCurrentTap()
{
    return currentTap_SO;
}
}
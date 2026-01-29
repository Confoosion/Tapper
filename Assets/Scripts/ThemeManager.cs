using UnityEngine;
using System.Collections.Generic;

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

    public void EquipBackground(Background_SO background)
    {
        currentBackground.dirt = background.dirt;
        currentBackground.grass = background.grass;
        currentBackground.details = background.details;
        currentBackground.sky = background.sky;
        currentBackground.menuBG = background.menuBG;

        // Show the visuals
        // Code here...
    }

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

    public AnimalSet GetAnimalSet()
    {
        return(currentAnimalSet);
    }
}
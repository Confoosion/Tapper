using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Coffee.UIExtensions;

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
    public Sprite sky;
    public Sprite mainMenuBG;
    public Sprite menuBG;
    public Sprite vines;
    public AudioClip BG_BGM;
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

        SoundManager.Singleton.UpdateAnimalSounds(animalSet.preview_Sounds);

        UIManager.Singleton.UpdateGameInstructions(animalSet);
    }

    public void EquipBackgroundSet(Background_SO background)
    {
        currentBackground.dirt = background.dirt;
        currentBackground.grass = background.grass;
        currentBackground.sky = background.sky;
        currentBackground.mainMenuBG = background.mainMenuBG;
        currentBackground.menuBG = background.menuBG;
        currentBackground.vines = background.gameOverDetails;
        currentBackground.BG_BGM = background.backgroundBGM;

        // Show the visuals
        UIManager.Singleton.SwitchBackgrounds(background);

        // Play the music
        SoundManager.Singleton.SwitchMusic(currentBackground.BG_BGM);
    }

    public void EquipTap(Taps_SO tap)
    {
        currentTap_SO = tap;
        currentTapPoolTag = tap.poolTag;
        SoundManager.Singleton.UpdateHitSound(tap.tapSFX);
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

    public void PlayTapEffectPreview(Vector2 position, Transform objParent)
    {
        if (currentTap_SO == null)
        {
            Debug.LogWarning("No tap effect equipped!");
            return;
        }
        
        // Instantiate directly (no pool)
        GameObject tapObj = Instantiate(
            currentTap_SO.tapPrefab,
            position,
            Quaternion.identity,
            objParent
        );
        
        if (currentTap_SO.effectType == TapEffectType.Particle)
        {
            ParticleSystem ps = tapObj.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                tapObj.GetComponent<UIParticle>().scale += 50f;
                ps.Play();
                float duration = ps.main.duration + ps.main.startLifetime.constantMax;
                Destroy(tapObj, duration + 0.5f);
            }
            else
            {
                Destroy(tapObj, 2f);
            }
        }
        else if (currentTap_SO.effectType == TapEffectType.UniqueEffect)
        {
            Tap_PawAnim pawAnim = tapObj.GetComponent<Tap_PawAnim>();
            if (pawAnim != null)
            {
                tapObj.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
                pawAnim.PlayAnim();
            }
            
            // Auto-destroy after animation
            Destroy(tapObj, 2f);
        }
    }

    // Playing equipped Tap particles
    public void PlayTapEffect(Vector2 position)
    {
        if (string.IsNullOrEmpty(currentTapPoolTag))
        {
            Debug.LogWarning("No tap effect equipped!");
            return;
        }
        
        GameObject tapObj = ObjectPoolManager.Instance.SpawnFromPool(currentTapPoolTag, position);
    
        if (tapObj != null)
        {
            if (currentTap_SO.effectType == TapEffectType.Particle)
            {
                HandleParticleEffect(tapObj);
            }
            else if (currentTap_SO.effectType == TapEffectType.UniqueEffect)
            {
                HandleUniqueEffect(tapObj);
            }

            SoundManager.Singleton.PlayHitSound();
        }
    }

    private void HandleParticleEffect(GameObject tapObj)
    {
        ParticleSystem ps = tapObj.GetComponent<ParticleSystem>();
        
        if (ps != null)
        {
            ps.Clear();
            ps.Play();
            StartCoroutine(ReturnTapToPool(tapObj, ps));
        }
        else
        {
            Debug.LogWarning($"No ParticleSystem found on {tapObj.name}");
            ObjectPoolManager.Instance.ReturnToPool(tapObj);
        }
    }

    private void HandleUniqueEffect(GameObject tapObj)
    {
        // Try to find and play the unique effect animation
        Tap_PawAnim pawAnim = tapObj.GetComponent<Tap_PawAnim>();
        
        if (pawAnim != null)
        {
            // tapObj.transform.localScale = new Vector3(1f, 1f, 1f);
            pawAnim.PlayAnim();
            
            // Return to pool after the animation duration
            StartCoroutine(ReturnUniqueEffectToPool(tapObj, 1f));
        }
        else
        {
            Debug.LogWarning($"No Tap_PawAnim found on {tapObj.name}");
            ObjectPoolManager.Instance.ReturnToPool(tapObj);
        }
    }

    private IEnumerator ReturnTapToPool(GameObject obj, ParticleSystem ps)
    {
        // Wait for particle to finish
        float duration = ps.main.duration;
        float lifetime = ps.main.startLifetime.constantMax;
        
        yield return new WaitForSeconds(duration + lifetime);
        
        while (ps.IsAlive())
        {
            yield return null;
        }
        
        ObjectPoolManager.Instance.ReturnToPool(obj);
    }

    private IEnumerator ReturnUniqueEffectToPool(GameObject obj, float duration)
    {
        yield return new WaitForSeconds(duration);
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
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Singleton { get; private set; }

    [SerializeField] private bool isSpawning;
    public RectTransform spawnArea;
    public GameObject[] goodTargets;
    public GameObject badTarget;
    public GameObject timeTarget;
    private int MAX_TARGETS = 10;
    [SerializeField] private float spawnWaitTime = 0.1f;

    private float badPercentage;
    private float mushroomPercentage;

    [SerializeField] private float gracePeriod = 3f;
    [SerializeField] private bool doGraceSpawns;
    [SerializeField] private bool isTimed;

    public float MAX_SPAWN_INTERVAL = 1.5f;
    public float MIN_SPAWN_INTERVAL = 0.15f;
    [SerializeField] float spawnInterval = 1.5f;
    public float decayRate = 0.08f;

    [SerializeField] float inGameTime = 0f;

    [SerializeField] GameObject placementTester;
    public Vector2 despawnLocation;
    [SerializeField] List<RectTransform> targets = new List<RectTransform>();

    // Pool tags for the good targets
    [Header("Object Pool Tags")]
    [SerializeField] private string[] goodTargetTags = { "Mole", "Mouse", "Rabbit" };
    [SerializeField] private string badTargetTag = "Bomb";
    [SerializeField] private string timeTargetTag = "Mushroom";

    void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
    }

    void Update()
    {
        if (isSpawning && GameManager.Singleton.CheckPlayerStatus())
        {
            inGameTime += Time.deltaTime;

            if (spawnInterval != MIN_SPAWN_INTERVAL)
            {
                spawnInterval = Mathf.Max(MIN_SPAWN_INTERVAL, MAX_SPAWN_INTERVAL * Mathf.Exp(-decayRate * inGameTime));
            }
        }
    }

    public void SetSpawnVariables(float bad, float mushroom, float decay, bool graceSpawns, bool timed)
    {
        badPercentage = bad;
        mushroomPercentage = mushroom;
        decayRate = decay;
        doGraceSpawns = graceSpawns;
        isTimed = timed;
    }

    public void StartSpawning()
    {
        inGameTime = 0f;
        spawnInterval = MAX_SPAWN_INTERVAL;
        isSpawning = true;
        StartCoroutine(SpawnCircles());
    }

    // Main spawning function
    IEnumerator SpawnCircles()
    {
        while (GameManager.Singleton.CheckPlayerStatus())
        {
            while (targets.Count >= MAX_TARGETS)
            {
                Debug.Log("TOO MANY TARGETS!!! WAITING");
                yield return new WaitForSeconds(spawnWaitTime);
            }
            
            GameObject target;
            if (CanSpawnGoodCircle())
            {
                // Spawn a random good target from the pool
                string randomTag = goodTargetTags[Random.Range(0, goodTargetTags.Length)];
                target = ObjectPoolManager.Instance.SpawnFromPool(randomTag, Vector2.zero);
            }
            else
            {
                if (isTimed && Random.Range(0f, 1f) < mushroomPercentage)
                {
                    target = ObjectPoolManager.Instance.SpawnFromPool(timeTargetTag, Vector2.zero);
                }
                else if(Random.Range(0f, 1f) < badPercentage)
                {
                    target = ObjectPoolManager.Instance.SpawnFromPool(badTargetTag, Vector2.zero);
                }
                else
                {
                    string randomTag = goodTargetTags[Random.Range(0, goodTargetTags.Length)];
                    target = ObjectPoolManager.Instance.SpawnFromPool(randomTag, Vector2.zero);
                }
            }

            if (target != null)
            {
                targets.Add(target.GetComponent<RectTransform>());
                GetRandomSpawnPosition(target);
                target.GetComponent<TargetBehavior>().ResetValues();
            }

            yield return new WaitForSeconds(spawnInterval);
        }

        Debug.Log("END OF GAME");
        isSpawning = false;
    }

    private bool CanSpawnGoodCircle()
    {   
        if (Random.Range(0f, 1f) > badPercentage || (doGraceSpawns && inGameTime < gracePeriod))
        {
            return (true);
        }
        return (false);
    }

    public void GetRandomSpawnPosition(GameObject target)
    {
        float spawnWidth = spawnArea.rect.width;
        float spawnHeight = spawnArea.rect.height;

        Vector2 position = new Vector2(
            Random.Range(-spawnWidth * 0.5f, spawnWidth * 0.5f),
            Random.Range(-spawnHeight * 0.5f, spawnHeight * 0.5f)
        );

        placementTester.transform.localPosition = position;
        if (IsOverlapping(placementTester.GetComponent<RectTransform>()))
        {
            GetRandomSpawnPosition(target);
        }
        else
        {
            target.transform.localPosition = position;
        }
    }

    private bool IsOverlapping(RectTransform targetRect)
    {
        Vector3[] targetCorners = new Vector3[4];
        Vector3[] rectCorners = new Vector3[4];
        targetRect.GetWorldCorners(targetCorners);
        Rect rect1 = new Rect(targetCorners[0], targetCorners[2] - targetCorners[0]);
        Rect rect2;

        foreach (RectTransform rect in targets)
        {
            if (rect == null)
            {
                continue;
            }

            rect.GetWorldCorners(rectCorners);
            rect2 = new Rect(rectCorners[0], rectCorners[2] - rectCorners[0]);

            if (rect1.Overlaps(rect2))
            {
                return (true);
            }
        }

        return (false);
    }

    public void RemoveALLTargets()
    {
        foreach(RectTransform target in targets)
        {
            if (target != null)
            {
                ObjectPoolManager.Instance.ReturnToPool(target.gameObject);
            }
        }
        targets.Clear();
    }

    public void RemoveTarget(RectTransform target)
    {
        if (target != null)
        {
            ObjectPoolManager.Instance.ReturnToPool(target.gameObject);
        }
        targets.Remove(target);
    }

    // Helper method to determine which pool tag to use when returning objects
    private string DeterminePoolTag(GameObject obj)
    {
        // Check against the original prefabs to determine pool tag
        for (int i = 0; i < goodTargets.Length; i++)
        {
            if (obj.name.Contains(goodTargets[i].name))
            {
                return goodTargetTags[i];
            }
        }

        if (obj.name.Contains(badTarget.name))
        {
            return badTargetTag;
        }

        if (obj.name.Contains(timeTarget.name))
        {
            return timeTargetTag;
        }

        // Default fallback
        Debug.LogWarning($"Could not determine pool tag for {obj.name}");
        return "BadTarget";
    }

    /// <summary>
    /// Updates animal references when theme changes
    /// Called by ThemeManager when equipping a new animal set
    /// </summary>
    public void UpdateAnimalReferences(GameObject[] newGoodTargets, GameObject newBadTarget)
    {
        goodTargets = newGoodTargets;
        badTarget = newBadTarget;
        
        Debug.Log($"SpawnManager animal references updated");
    }
}
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Singleton { get; private set; }

    [SerializeField] private bool isSpawning;
    public RectTransform spawnArea;
    public GameObject goodTarget;
    public GameObject badTarget;

    public float badPercentage = 0.15f;

    public float MAX_SPAWN_INTERVAL = 1.5f;
    public float MIN_SPAWN_INTERVAL = 0.25f;
    [SerializeField] float spawnInterval = 1.5f;
    public float decayRate = 0.05f;

    [SerializeField] float inGameTime = 0f;

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
            GameObject target;
            if (CanSpawnGoodCircle())
            {
                target = Instantiate(goodTarget, spawnArea);
            }
            else
            {
                target = Instantiate(badTarget, spawnArea);
            }

            target.transform.localPosition = GetRandomSpawnPosition();
            // Debug.Log("Local Position: " + target.transform.localPosition + "\nPosition: " + target.transform.position);
            // Debug.Log("Spawned");
            yield return new WaitForSeconds(spawnInterval);
        }

        Debug.Log("END OF GAME");
        isSpawning = false;
    }

    private bool CanSpawnGoodCircle()
    {
        if (Random.Range(0f, 1f) > badPercentage)
        {
            return (true);
        }
        return (false);
    }

    private Vector2 GetRandomSpawnPosition() // ADD CLAMP TO THE POSITION
    {
        float spawnWidth = spawnArea.rect.width;
        float spawnHeight = spawnArea.rect.height;

        Vector2 position = new Vector2(
            Random.Range(-spawnWidth * 0.5f, spawnWidth * 0.5f),
            Random.Range(-spawnHeight * 0.5f, spawnHeight * 0.5f)
        );

        // Debug.Log(position);
        return (position);
    }
}
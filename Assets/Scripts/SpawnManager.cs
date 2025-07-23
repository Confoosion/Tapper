using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Singleton { get; private set; }

    [SerializeField] private bool isSpawning;
    public RectTransform spawnArea;
    public GameObject goodCircle;
    public GameObject badCircle;

    public float badPercentage = 0.1f;

    public float MAX_SPAWN_INTERVAL = 1.5f;
    public float MIN_SPAWN_INTERVAL = 0.2f;
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

            spawnInterval = Mathf.Max(MIN_SPAWN_INTERVAL, MAX_SPAWN_INTERVAL * Mathf.Exp(-decayRate * inGameTime));
        }
    }

    IEnumerator SpawnCircles()
    {
        while (GameManager.Singleton.CheckPlayerStatus())
        {
            if (CanSpawnGoodCircle())
            {
                Instantiate(goodCircle, GetRandomSpawnPosition(), Quaternion.identity, spawnArea);
            }
            else
            {
                Instantiate(badCircle, GetRandomSpawnPosition(), Quaternion.identity, spawnArea);
            }

            Debug.Log("Spawned");
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
        float width = spawnArea.rect.width;
        float height = spawnArea.rect.height;

        Vector2 position = new Vector2(
            Random.Range(-width * 0.5f, width * 0.5f) + spawnArea.position.x,
            Random.Range(-height * 0.5f, height * 0.5f) + spawnArea.position.y
        );

        return (position);
    }

    public void StartSpawning()
    {
        inGameTime = 0f;
        spawnInterval = MAX_SPAWN_INTERVAL;
        isSpawning = true;
        StartCoroutine(SpawnCircles());
    }
}

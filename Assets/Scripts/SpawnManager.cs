using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum GameMode { Classic, Rain }
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

    // Classic Gamemode
    IEnumerator SpawnCircles()
    {
        while (GameManager.Singleton.CheckPlayerStatus())
        {
            GameObject circle;
            if (CanSpawnGoodCircle())
            {
                circle = Instantiate(goodCircle, spawnArea);
            }
            else
            {
                circle = Instantiate(badCircle, spawnArea);
            }

            circle.transform.localPosition = GetRandomSpawnPosition();
            Debug.Log("Local Position: " + circle.transform.localPosition + "\nPosition: " + circle.transform.position);
            // Debug.Log("Spawned");
            yield return new WaitForSeconds(spawnInterval);
        }

        Debug.Log("END OF GAME");
        isSpawning = false;
    }

    // Rain Gamemode
    IEnumerator SpawnRainCircles()
    {
        yield return null;
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

        Debug.Log(position);
        return (position);
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Singleton { get; private set; }

    [SerializeField] private GameMode currentGameMode;
    [SerializeField] private bool isSpawning;
    public RectTransform spawnArea;
    public GameObject primaryCircle;
    public GameObject alternateCircle;

    public float badPercentage = 0.1f;

    public float MAX_SPAWN_INTERVAL_CLASSIC = 1.5f;
    public float MIN_SPAWN_INTERVAL_CLASSIC = 0.2f;
    public float MAX_SPAWN_INTERVAL_RAIN = 1.5f;
    public float MIN_SPAWN_INTERVAL_RAIN = 0.5f;

    [SerializeField] private float selected_Max_Spawn_Interval;
    [SerializeField] private float selected_Min_Spawn_Interval;

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

            if (spawnInterval != selected_Min_Spawn_Interval)
            {
                spawnInterval = Mathf.Max(selected_Min_Spawn_Interval, selected_Max_Spawn_Interval * Mathf.Exp(-decayRate * inGameTime));
            }
        }
    }

    public void StartSpawning(GameMode gameMode)
    {
        inGameTime = 0f;
        currentGameMode = gameMode;

        if (gameMode == GameMode.Classic)
        {
            Classic_Setup();
        }
        else if(gameMode == GameMode.Rain)
        {
            Rain_Setup();
        }
        isSpawning = true;
    }

    private void Classic_Setup()
    {
        spawnInterval = MAX_SPAWN_INTERVAL_CLASSIC;
        primaryCircle = CirclePrefabs.Singleton.Classic_GoodCircle;
        alternateCircle = CirclePrefabs.Singleton.Classic_BadCircle;

        StartCoroutine(SpawnCircles());
    }

    private void Rain_Setup()
    {
        spawnInterval = MAX_SPAWN_INTERVAL_RAIN;
        primaryCircle = CirclePrefabs.Singleton.Rain_Circle;

        StartCoroutine(SpawnRainCircles());
    }

    // Classic Gamemode
    IEnumerator SpawnCircles()
    {
        while (GameManager.Singleton.CheckPlayerStatus())
        {
            GameObject circle;
            if (CanSpawnGoodCircle())
            {
                circle = Instantiate(primaryCircle, spawnArea);
            }
            else
            {
                circle = Instantiate(alternateCircle, spawnArea);
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

    // Spawns based on the spawnArea, which is changed based on the selected GameMode
    private Vector2 GetRandomSpawnPosition()
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

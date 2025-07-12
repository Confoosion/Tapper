using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public RectTransform canvas;
    public GameObject goodCircle;
    public GameObject badCircle;

    public float badPercentage = 0.1f;

    [SerializeField] float spawnInterval = 1f;

    IEnumerator SpawnCircles()
    {
        while (GameManager.Singleton.CheckPlayerStatus())
        {
            if (CanSpawnGoodCircle())
            {
                Instantiate(goodCircle, GetRandomSpawnPosition(), Quaternion.identity, canvas);
            }
            else
            {
                Instantiate(badCircle, GetRandomSpawnPosition(), Quaternion.identity, canvas);
            }

            Debug.Log("Spawned");
            yield return new WaitForSeconds(spawnInterval);
        }

        Debug.Log("END OF GAME");
    }

    private bool CanSpawnGoodCircle()
    {
        if (Random.Range(0f, 1f) > badPercentage)
        {
            return (true);
        }
        return (false);
    }

    private Vector2 GetRandomSpawnPosition()
    {
        float width = canvas.rect.width;
        float height = canvas.rect.height;

        Vector2 position = new Vector2(
            Random.Range(-width * 0.5f, width * 0.5f) + canvas.position.x,
            Random.Range(-height * 0.5f, height * 0.5f) + canvas.position.y
        );

        return (position);
    }

    void Start()
    {
        StartCoroutine(SpawnCircles());
    }
}

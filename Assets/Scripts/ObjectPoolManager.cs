using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages object pooling for multiple object types to optimize instantiation
/// </summary>
public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set; }

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
        [Tooltip("Enable automatic pool expansion when running out of objects")]
        public bool autoExpand = true;
        [Tooltip("How many objects to add when auto-expanding")]
        public int expandAmount = 5;
    }

    [Header("Pool Configuration")]
    public List<Pool> pools;

    [Header("Organization")]
    [Tooltip("Optional: Parent transform to organize pooled objects under")]
    public Transform poolParent;

    private Dictionary<string, Queue<GameObject>> poolDictionary;
    // private Dictionary<string, Transform> poolContainers;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializePools();
    }

    void InitializePools()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        // poolContainers = new Dictionary<string, Transform>();

        // Transform parentTransform = poolParent;

        foreach (Pool pool in pools)
        {
            // Create a container for each pool type
            // GameObject container = new GameObject($"{pool.tag}_Pool");
            // container.transform.SetParent(parentTransform);
            // poolContainers.Add(pool.tag, container.transform);

            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, poolParent);
                obj.SetActive(false);
                // obj.transform.SetParent(poolParent);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    /// <summary>
    /// Spawns an object from the pool at specified position and rotation
    /// </summary>
    public GameObject SpawnFromPool(string tag, Vector2 position)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = null;

        // Check if we need an inactive object
        Queue<GameObject> pool = poolDictionary[tag];
        int checkedCount = 0;
        int poolSize = pool.Count;

        // Look for an inactive object in the pool
        while (checkedCount < poolSize)
        {
            GameObject obj = pool.Dequeue();
            pool.Enqueue(obj);

            if (!obj.activeInHierarchy)
            {
                objectToSpawn = obj;
                break;
            }

            checkedCount++;
        }

        // If no inactive object found, auto-expand if enabled
        if (objectToSpawn == null)
        {
            Pool poolConfig = pools.Find(p => p.tag == tag);
            
            if (poolConfig != null && poolConfig.autoExpand)
            {
                Debug.Log($"Pool '{tag}' ran out of objects. Auto-expanding by {poolConfig.expandAmount}.");
                ExpandPool(tag, poolConfig.expandAmount);
                
                // Get the newly created object
                objectToSpawn = pool.Dequeue();
                pool.Enqueue(objectToSpawn);
            }
            else
            {
                Debug.LogWarning($"Pool '{tag}' ran out of inactive objects and auto-expand is disabled!");
                return null;
            }
        }

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;

        // Notify the object it was spawned (if it implements IPooledObject)
        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();
        pooledObj?.OnObjectSpawn();

        return objectToSpawn;
    }

    /// <summary>
    /// Returns an object to the pool (deactivates it)
    /// </summary>
    public void ReturnToPool(GameObject obj)
    {   
        obj.SetActive(false);
    }

    /// <summary>
    /// Expands a specific pool by adding more objects
    /// </summary>
    public void ExpandPool(string tag, int amount)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
            return;
        }

        Pool pool = pools.Find(p => p.tag == tag);
        if (pool == null) return;

        // Transform container = poolContainers[tag];

        for (int i = 0; i < amount; i++)
        {
            GameObject obj = Instantiate(pool.prefab, poolParent);
            obj.SetActive(false);
            // obj.transform.SetParent(poolParent);
            poolDictionary[tag].Enqueue(obj);
        }
    }
}

/// <summary>
/// Interface for pooled objects to receive spawn notifications
/// </summary>
public interface IPooledObject
{
    void OnObjectSpawn();
}
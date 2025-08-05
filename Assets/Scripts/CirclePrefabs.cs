using UnityEngine;

public class CirclePrefabs : MonoBehaviour
{
    public static CirclePrefabs Singleton { get; private set; }

    [Header("Classic Game Mode")]
    public GameObject Classic_GoodCircle;
    public GameObject Classic_BadCircle;

    [Header("Rain Game Mode")]
    public GameObject Rain_Circle;

    void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
    }
}

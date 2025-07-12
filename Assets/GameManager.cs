using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton { get; private set; }

    [SerializeField] private bool isAlive = true;

    [SerializeField] private int points = 0;

    void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(Singleton.gameObject);
        }
    }

    public bool CheckPlayerStatus()
    {
        return (isAlive);
    }

    public void SetPlayerStatus(bool status)
    {
        isAlive = status;
    }

    public int GetPoints()
    {
        return (points);
    }

    public void AddPoints(int toAdd)
    {
        if (points + toAdd >= 0)
        {
            points += toAdd;
        }
    }
}

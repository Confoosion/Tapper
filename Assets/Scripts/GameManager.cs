using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton { get; private set; }

    public bool isPlaying;
    [SerializeField] private bool isAlive = true;

    [SerializeField] private int lives = 3;

    void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(Singleton.gameObject);
        }
    }

    public void StartGame()
    {
        isPlaying = true;
        SpawnManager.Singleton.StartSpawning();
    }

    public bool CheckPlayerStatus()
    {
        return (isAlive);
    }

    public void SetPlayerStatus(bool status)
    {
        isAlive = status;

        if (!isAlive)
        {
            isPlaying = false;
            ScreenManager.Singleton.GoToGameOver();
        }
    }

    public int GetLives()
    {
        return (lives);
    }

    public void RemoveLives(int livesToRemove)
    {
        if (livesToRemove == 0)
        {
            SetPlayerStatus(true);
            lives = 3;
        }
        else if (lives - livesToRemove <= 0)
        {
            SetPlayerStatus(false);
            lives = 0;
        }
        else
        {
            lives -= livesToRemove;
        }

        UIManager.Singleton.UpdateHearts(lives);
    }

    public void ResetValues()
    {
        ScoreManager.Singleton.AddPoints(0);
        RemoveLives(0);
    }
}

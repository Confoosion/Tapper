using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton { get; private set; }

    public bool isPlaying;
    [SerializeField] private bool isAlive = true;

    [SerializeField] private int lives = 3;
    [SerializeField] private int points = 0;
    [SerializeField] private int highscore = 0;

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

    public int GetPoints()
    {
        return (points);
    }

    public void AddPoints(int toAdd)
    {
        // Reset points to 0
        if (toAdd == 0)
        {
            points = 0;
            UIManager.Singleton.UpdatePoints(points);
            return;
        }

        if (points + toAdd >= 0)
        {
            points += toAdd;
            UIManager.Singleton.UpdatePoints(points);
        }
    }

    public int GetHighscore()
    {
        return (highscore);
    }

    public void SetHighscore(int score)
    {
        highscore = score;
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
        AddPoints(0);
        RemoveLives(0);
    }
}

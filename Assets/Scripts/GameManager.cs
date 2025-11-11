using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton { get; private set; }

    public bool isPlaying;
    public bool isPaused;
    public GameModeSO currentGameMode;
    [SerializeField] private bool isAlive = true;

    [SerializeField] private int lives = 3;

    [SerializeField] private float fps;

    void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(Singleton.gameObject);
        }
    }

    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        InvokeRepeating("GetFPS", 1, 1);
        UpdateGameMode(currentGameMode);
    }

    public void UpdateGameMode(GameModeSO mode)
    {
        currentGameMode = mode;
        SetLives(currentGameMode.lives);
        SpawnManager.Singleton.SetSpawnVariables(currentGameMode.badSpawnPercentage, currentGameMode.decayRate, currentGameMode.doGraceSpawns, currentGameMode.isTimed);
    }

    public void PauseGame(bool pause)
    {
        isPaused = pause;

        UIManager.Singleton.ShowPauseScreen(isPaused);

        if (isPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void StartGame()
    {
        isPlaying = true;
        ScoreManager.Singleton.ResetCirclesTapped();
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
            ArcadeEnding.Singleton.StartEndingAnimation();
            // ScreenManager.Singleton.SwitchScreen(ScreenManager.Singleton.GetEndScreen());
            // ScreenManager.Singleton.GoToGameOver();
        }
    }

    public void SetLives(int amount)
    {
        lives = amount;

        if (lives < 0)
        {
            UIManager.Singleton.UpdateHearts(0);
        }
        else
        {
            UIManager.Singleton.UpdateHearts(lives);
        }
    }

    public int GetLives()
    {
        return (lives);
    }

    public void RemoveLives(int livesToRemove)
    {
        if (lives < 0)
        {
            return;
        }
        
        if (livesToRemove == 0)
        {
            SetPlayerStatus(true);
            SetLives(currentGameMode.lives);
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

    public void RestartGame()
    {
        isPlaying = false;
        isAlive = false;
        PauseGame(false);
        ScreenManager.Singleton.BeginMajorTransition(ScreenManager.Singleton.GetGameScreen());
    }
    
    public void LeaveGame()
    {
        
    }

    void GetFPS()
    {
        fps = (int)(1f / Time.unscaledDeltaTime);
    }
}

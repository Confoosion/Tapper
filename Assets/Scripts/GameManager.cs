using UnityEngine;
using System.Collections;


public enum GameMode { Classic, Rain }
public class GameManager : MonoBehaviour
{
    public static GameManager Singleton { get; private set; }

    public bool isPlaying;
    [SerializeField] private GameMode selectedGameMode;
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
    }

    public void UpdateGameMode(GameMode mode)
    {
        selectedGameMode = mode;
    }

    public GameMode GetGameMode()
    {
        return (selectedGameMode);
    }

    public void StartGame()
    {
        isPlaying = true;
        SpawnManager.Singleton.StartSpawning(selectedGameMode);
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
            ScreenManager.Singleton.SwitchScreen(ScreenManager.Singleton.GetEndScreen());
            // ScreenManager.Singleton.GoToGameOver();
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

    void GetFPS()
    {
        fps = (int)(1f / Time.unscaledDeltaTime);
    }
}

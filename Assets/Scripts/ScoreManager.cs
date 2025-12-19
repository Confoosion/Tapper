using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SocialPlatforms;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Singleton { get; private set; }

    [SerializeField] private int score;
    [SerializeField] private int circlesTapped;
    private int highestSessionScore = 0;
    private int difficultyScaleInterval = 35;
    private float difficultyScaler = 0.1f;
    private int MAX_DIFFICULTY = 10;

    void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
    }

    void Start()
    {
        Social.localUser.Authenticate(success => { if (success) { Debug.Log("==iOS GC authenticate OK"); } else { Debug.Log("==iOS GC authenticate Failed"); } });
    }

    public bool AddCircleTapped()
    {
        circlesTapped++;

        if (circlesTapped % 10 == 0)
        {
            SoundManager.Singleton.PlaySound(SoundType.Gem, 0.5f);
            AddGems(1);
            return (true);
        }
        return (false);
    }

    public void ResetCirclesTapped()
    {
        circlesTapped = 0;
    }

    public int GetPoints()
    {
        return (score);
    }

    public void AddPoints(int toAdd)
    {
        // Reset points to 0
        if (toAdd == 0)
        {
            score = 0;
            highestSessionScore = 0;
            UIManager.Singleton.UpdatePoints(score);
            return;
        }

        if (score + toAdd >= 0)
        {
            score += toAdd;
            UIManager.Singleton.UpdatePoints(score);

            if(score > highestSessionScore)
            {
                highestSessionScore = score;
            }
            else
            {
                return;
            }

            if(score % difficultyScaleInterval == 0 && GameManager.Singleton.GetGameDifficulty() < MAX_DIFFICULTY)
            {
                GameManager.Singleton.UpdateGameDifficulty(difficultyScaler);
            }
        }
    }

    public void AddGems(int toAdd)
    {
        PlayerPrefs.SetInt("SavedGems", GetGems() + toAdd);
        UIManager.Singleton.UpdateLeafUI();
    }

    public string GetHighscorePP()
    {
        GameModeSO mode = GameModeManager.Singleton.GetCurrentMode();

        return(mode.modeName + "_Highscore");
    }

    public int GetHighscore()
    {
        if (PlayerPrefs.HasKey(GetHighscorePP()))
        {
            return (PlayerPrefs.GetInt(GetHighscorePP()));
        }
        return (0);
    }

    public int GetGems()
    {
        if (PlayerPrefs.HasKey("SavedGems"))
        {
            return (PlayerPrefs.GetInt("SavedGems"));
        }
        return (0);
    }

    public void UpdateHighscore(int points)
    {
        string modePP = GetHighscorePP();
        if (PlayerPrefs.HasKey(modePP))
        {
            if (points > PlayerPrefs.GetInt(modePP))
            {
                PlayerPrefs.SetInt(modePP, points);
                // LeaderboardManager.Singleton.SetLeaderboardEntry(points);
            }
        }
        else
        {
            PlayerPrefs.SetInt(modePP, points);
            // LeaderboardManager.Singleton.SetLeaderboardEntry(points);
        }

        string iOS_LeaderboardID = "com.Confoosion.Tapper." + GameModeManager.Singleton.GetCurrentMode().modeName;
        bool isGCAuthenticated = Social.localUser.authenticated;

        if (isGCAuthenticated)
        {
            Social.ReportScore(score, iOS_LeaderboardID, success => { if (success) { Debug.Log("==iOS GC report score ok: " + score + "\n"); } else { Debug.Log("==iOS GC report score Failed: " + iOS_LeaderboardID + "\n"); } });
        }
        else
        {
            Debug.Log("==iOS GC can't report score, not authenticated\n");
        }
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Singleton { get; private set; }

    [SerializeField] private int score;
    [SerializeField] private int circlesTapped;

    void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
    }

    public bool AddCircleTapped()
    {
        circlesTapped++;

        if (circlesTapped % 10 == 0)
        {
            SoundManager.Singleton.PlaySound(SoundType.Gem, 0.25f);
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
            UIManager.Singleton.UpdatePoints(score);
            return;
        }

        if (score + toAdd >= 0)
        {
            score += toAdd;
            UIManager.Singleton.UpdatePoints(score);
        }
    }

    public void AddGems(int toAdd)
    {
        PlayerPrefs.SetInt("SavedGems", GetGems() + toAdd);
        UIManager.Singleton.UpdateGemUI();
    }

    public int GetHighscore()
    {
        if (PlayerPrefs.HasKey("SavedHighScore"))
        {
            return (PlayerPrefs.GetInt("SavedHighScore"));
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
        if (PlayerPrefs.HasKey("SavedHighScore"))
        {
            if (points > PlayerPrefs.GetInt("SavedHighScore"))
            {
                PlayerPrefs.SetInt("SavedHighScore", points);
            }
        }
        else
        {
            PlayerPrefs.SetInt("SavedHighScore", points);
        }
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Singleton { get; private set; }

    [SerializeField] private int score;

    void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
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

    public int GetHighscore()
    {
        if (PlayerPrefs.HasKey("SavedHighScore"))
        {
            return (PlayerPrefs.GetInt("SavedHighScore"));
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

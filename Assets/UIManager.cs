using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Singleton { get; private set; }

    private float countdownInterval = 0.8f;
    [Header("Game Screen UI")]
    [SerializeField] private GameObject countdownObject;
    [SerializeField] private List<Sprite> countdownImages = new List<Sprite>();

    [SerializeField] private List<GameObject> hearts = new List<GameObject>();
    [SerializeField] private TextMeshProUGUI points;

    [Header("GameOver Screen UI")]
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI highscore;
    [SerializeField] private GameObject highscoreLabel;
    [SerializeField] private GameObject celebrationLabel;
    [SerializeField] private GameObject gameOver_RetryButton;
    [SerializeField] private GameObject gameOver_BackButton;

    void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
    }

    public void UpdateHearts(int lives)
    {
        // Show lives
        for (int i = 0; i < lives; i++)
        {
            hearts[i].SetActive(true);
        }

        // Hide lives
        for (int i = lives; i < hearts.Count; i++)
        {
            hearts[i].SetActive(false);
        }
    }

    public void UpdatePoints(int num)
    {
        points.SetText(num.ToString());
    }

    public void BeginCountdown()
    {
        StartCoroutine(CountingDown());
    }

    IEnumerator CountingDown()
    {
        Image countdownImage = countdownObject.GetComponent<Image>();

        yield return new WaitForSeconds(1f);
        countdownObject.SetActive(true);

        foreach (Sprite image in countdownImages)
        {
            countdownImage.sprite = image;
            yield return new WaitForSeconds(countdownInterval);
        }

        countdownObject.SetActive(false);
        GameManager.Singleton.StartGame();
    }

    public bool UpdateEndScore(int num)
    {
        score.SetText(num.ToString());

        if (num > GameManager.Singleton.GetHighscore())
        {
            GameManager.Singleton.SetHighscore(num);
            return (true);
        }
        return (false);
    }

    public void ShowHighscore(bool show)
    {
        highscoreLabel.SetActive(show);
    }

    public void ShowCelebration(bool show)
    {
        celebrationLabel.SetActive(show);
    }

    public void ShowEndScreenButtons(bool show)
    {
        gameOver_RetryButton.SetActive(show);
        gameOver_BackButton.SetActive(show);
    }
}

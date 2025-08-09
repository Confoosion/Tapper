using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Singleton { get; private set; }

    [Header("Main Menu UI")]
    [SerializeField] private TextMeshProUGUI MM_highscore;
    [SerializeField] private TextMeshProUGUI MM_gems;
    public AnimateArrows MM_arrows;

    private float countdownInterval = 0.8f;
    [Header("Game Screen UI")]
    [SerializeField] private Image good_Image;
    [SerializeField] private Image bad_Image;
    [SerializeField] private GameObject countdownObject;
    [SerializeField] private List<Sprite> countdownImages = new List<Sprite>();

    [SerializeField] private List<GameObject> hearts = new List<GameObject>();
    [SerializeField] private TextMeshProUGUI points;
    public SpawnArea spawnArea;

    [Header("GameOver Screen UI")]
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI GO_highscore;
    [SerializeField] private GameObject highscoreLabel;
    [SerializeField] private GameObject celebrationLabel;
    [SerializeField] private GameObject gameOver_RetryButton;
    [SerializeField] private GameObject gameOver_BackButton;

    [Header("Background UI")]
    [SerializeField] private List<Image> backgroundImages = new List<Image>();
    private int backgroundIndex = 0;
    private Coroutine backgroundCoroutine;

    void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
    }

    void Start()
    {
        UpdateHighscoreUI();
        UpdateGemUI();
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

        if (num > ScoreManager.Singleton.GetHighscore(GameManager.Singleton.GetGameMode()))
        {
            ScoreManager.Singleton.UpdateHighscore(num, GameManager.Singleton.GetGameMode());
            UpdateHighscoreUI();

            return (true);
        }
        return (false);
    }

    public void UpdateHighscoreUI()
    {
        MM_highscore.SetText(ScoreManager.Singleton.GetHighscore(GameManager.Singleton.GetGameMode()).ToString());
        GO_highscore.SetText(ScoreManager.Singleton.GetHighscore(GameManager.Singleton.GetGameMode()).ToString());
    }

    public void UpdateGemCount()
    {
        MM_gems.SetText(ScoreManager.Singleton.GetGems().ToString());
    }

    public void UpdateGemUI()
    {
        if (PlayerPrefs.HasKey("SavedGems"))
        {
            MM_gems.SetText(PlayerPrefs.GetInt("SavedGems").ToString());
            return;
        }
        MM_gems.SetText("0");
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

    public void UpdateGameTheme(Sprite good, Sprite bad)
    {
        good_Image.sprite = good;
        bad_Image.sprite = bad;
    }

    public bool IsCoroutineActive()
    {
        return (backgroundCoroutine != null);
    }

    public void SwitchBackgrounds(int modeIndex, float time = 1f)
    {
        if (backgroundCoroutine != null)
        {
            return;
        }

        backgroundCoroutine = StartCoroutine(BackgroundTransition(backgroundImages[backgroundIndex], backgroundImages[modeIndex], time));
        backgroundIndex = modeIndex;
    }

    IEnumerator BackgroundTransition(Image fromBG, Image toBG, float duration)
    {
        Color toColor = toBG.color;
        toColor.a = 0f;
        toBG.color = toColor;

        float fadeTime = 0f;

        while (fadeTime < duration)
        {
            fadeTime += Time.deltaTime;
            float t = fadeTime / duration;

            Color fromColor = fromBG.color;
            fromColor.a = Mathf.Lerp(1f, 0f, t);
            toColor.a = Mathf.Lerp(0f, 1f, t);

            fromBG.color = fromColor;
            toBG.color = toColor;

            yield return null;
        }

        Color fColor = fromBG.color;
        fColor.a = 0f;
        fromBG.color = fColor;

        Color tColor = toBG.color;
        tColor.a = 1f;
        toBG.color = tColor;

        backgroundCoroutine = null;
    }
}

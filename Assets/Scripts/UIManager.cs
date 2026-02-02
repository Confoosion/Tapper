using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Singleton { get; private set; }

    [Header("Main Menu UI")]
    [SerializeField] private Image arcadeButtonImage;
    // [SerializeField] private TextMeshProUGUI MM_highscore;
    // [SerializeField] private TextMeshProUGUI MM_gems;
    [SerializeField] private TextMeshProUGUI MM_gameMode;

    [Header("Settings UI")]
    [SerializeField] private Sprite onToggle;
    [SerializeField] private Sprite offToggle;
    public Image SFXToggle;
    public Image MusicToggle;
    public Image VibrationToggle;

    [Header("Game Screen UI")]
    [SerializeField] private GameObject pauseScreen;
    // [SerializeField] private Image good_Image;
    // [SerializeField] private Image bad_Image;
    [SerializeField] private GameObject timeAttackTimer;
    [SerializeField] private GameObject countdownObject;
    [SerializeField] private FrameAnimation[] countdownAnims;
    [SerializeField] private GameObject goTextObject;
    [SerializeField] private List<GameObject> hearts = new List<GameObject>();
    [SerializeField] private TextMeshProUGUI points;
    [SerializeField] private Transform leafCountTransform;
    [SerializeField] private TextMeshProUGUI gameLeafCounter;
    // public SpawnArea spawnArea;

    [Header("GameOver Screen UI")]
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI GO_highscore;
    [SerializeField] private GameObject bestLabel;
    [SerializeField] private GameObject newBestLabel;
    [SerializeField] private FrameAnimation GO_ShopButton;
    [SerializeField] private Image GO_GameModeImage;

    [Header("Background UI")]
    [SerializeField] private Image[] BG_dirts;
    [SerializeField] private Image[] BG_menuBGs;
    [SerializeField] private Image BG_mainMenuBG;
    [SerializeField] private Image BG_grass;
    [SerializeField] private Image BG_details;
    [SerializeField] private Image BG_sky;

    [Header("Extra UI")]
    [SerializeField] private GameObject settingsButton;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private Sprite[] MM_GM_Sprites;
    [SerializeField] private Sprite[] GO_GM_Sprites;
    [SerializeField] private TextMeshProUGUI[] leafCounters;
    [SerializeField] private GameObject comingSoonText;
    [SerializeField] private Transform comingSoonParent;

    void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
    }

    void Start()
    {
        UpdateLeafUI();
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
        countdownObject.SetActive(true);

        foreach(FrameAnimation anim in countdownAnims)
        {
            anim.StartFullAnimation();
        }

        yield return new WaitForSeconds(1.95f);

        SoundManager.Singleton.PlaySound(SoundManager.Singleton.countdownAudio, 0.5f);

        yield return new WaitForSeconds(3f);

        goTextObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        countdownObject.SetActive(false);
        goTextObject.SetActive(false);

        countdownObject.SetActive(false);
        GameManager.Singleton.StartGame();
    }

    public bool UpdateEndScore(int num)
    {
        bool hasHighscore = false;
        score.SetText(num.ToString());

        if (num > ScoreManager.Singleton.GetHighscore())
        {
            ScoreManager.Singleton.UpdateHighscore(num);
            hasHighscore = true;
        }

        GO_highscore.SetText(ScoreManager.Singleton.GetHighscore().ToString());

        return (hasHighscore);
    }

    public void ShopButtonAnimation(bool start)
    {
        if(start)
            GO_ShopButton.StartIdleAnimation();
        else
            GO_ShopButton.StopALLAnimations();
    }

    public void ShowPauseScreen(bool show)
    {
        pauseScreen.SetActive(show);
    }

    public void UpdateHighscoreLabelUI(bool gotHighscore)
    {
        bestLabel.SetActive(!gotHighscore);
        newBestLabel.SetActive(gotHighscore);
    }

    public void SwitchBackgrounds(Background_SO background)
    {
        foreach(Image dirt in BG_dirts)
        {
            dirt.sprite = background.dirt;
        }
        foreach(Image menu in BG_menuBGs)
        {
            menu.sprite = background.menuBG;
        }

        BG_mainMenuBG.sprite = background.mainMenuBG;
        BG_grass.sprite = background.grass;
        BG_details.sprite = background.details;
        BG_sky.sprite = background.sky;
    }

    public void ChangeGameModeUI(GameModeSO mode, int modeIndex, bool arcadeMode, bool isHighscore)
    {
        if(arcadeMode)
        {
            arcadeButtonImage.sprite = MM_GM_Sprites[modeIndex];
            MM_gameMode.SetText(mode.modeName);
        }
        GO_GameModeImage.sprite = GO_GM_Sprites[modeIndex];

        GO_highscore.SetText(PlayerPrefs.GetInt(ScoreManager.Singleton.GetHighscorePP()).ToString());
        UpdateHighscoreLabelUI(isHighscore);
    }

    public void ShowSettingsOrPauseIcon(bool showSettings)
    {
        settingsButton.SetActive(showSettings);
        pauseButton.SetActive(!showSettings);
    }

    public void HideSettingsAndPauseIcon()
    {
        settingsButton.SetActive(false);
        pauseButton.SetActive(false);
    }

    public void UpdateToggle(Image toggleImage, bool toggle)
    {
        if (toggle)
        {
            toggleImage.sprite = onToggle;
        }
        else
        {
            toggleImage.sprite = offToggle;
        }
    }

    public void ShowTimeAttackTimer(bool show)
    {
        timeAttackTimer.SetActive(show);
    }

    public Transform GetLeafCounterTransform()
    {
        return(leafCountTransform);
    }

    public void SetGameScreenLeafUI(int amount)
    {
        gameLeafCounter.SetText(amount.ToString());
    }

    public void UpdateLeafUI()
    {
        foreach(TextMeshProUGUI leafCount in leafCounters)
        {
            leafCount.SetText(ScoreManager.Singleton.GetGems().ToString());
        }
    }

    public void NewFeatureComingSoon()
    {
        GameObject comingSoon = Instantiate(comingSoonText, new Vector2(Screen.width * 0.5f, Screen.height * 0.5f), Quaternion.identity, comingSoonParent);
        Vector3 moveTo = comingSoon.transform.position + new Vector3(0f, 20f, 0f);
        LeanTween.move(comingSoon, moveTo, 1f).setEase(LeanTweenType.easeOutQuad);
        StartCoroutine(Dissipate(comingSoon, 2f));
    }

    IEnumerator Dissipate(GameObject _object, float duration)
    {
        TextMeshProUGUI txt = _object.GetComponent<TextMeshProUGUI>();
        Color originalColor = txt.faceColor;
        float fadeTime = 0f;

        while (fadeTime < duration)
        {
            fadeTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, fadeTime / duration);
            txt.faceColor = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        txt.faceColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        Destroy(_object);
    }
}

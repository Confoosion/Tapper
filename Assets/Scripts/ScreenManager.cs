using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager Singleton { get; private set; }
    [SerializeField] private float transitionTime = 1f;
    private Coroutine transition = null;
    // private Coroutine endGame = null;

    [Space]
    [SerializeField] private GameObject currentScreen;
    [SerializeField] private List<GameObject> cachedScreens = new List<GameObject>();

    [Space]
    [Header("Moving UI Screens")]
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject Settings, Shop, ShopCategory, ArcadeEnding, TimeEnding, GameOver, Background;

    [Header("Static UI Screens")]
    [SerializeField] private GameObject StartScreen;
    [SerializeField] private GameObject Game;

    [Header("Moving UI Elements")]
    [SerializeField] private BackgroundPositions BG_Positions;
    [SerializeField] private GameObject S_Title;
    [SerializeField] private GameObject S_StartLabel;
    [SerializeField] private GameObject S_leftLeaves_Slow;
    [SerializeField] private GameObject S_leftLeaves_Fast;
    [SerializeField] private GameObject S_rightLeaves_Slow;
    [SerializeField] private GameObject S_rightLeaves_Fast;

    void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
    }

    void Start()
    {
        currentScreen = StartScreen;
        BeginStartScreen();
    }

    public float GetTransitionTime()
    {
        return (transitionTime);
    }

    public bool IsTransitionGoing()
    {
        return(transition != null);
    }

    public void SwitchScreen(GameObject screen)
    {
        if(screen == GameOver)
        {
            UIManager.Singleton.ShowSettingsOrPauseIcon(true);
            bool gotHighscore = UIManager.Singleton.UpdateEndScore(ScoreManager.Singleton.GetPoints());
            UIManager.Singleton.UpdateHighscoreLabelUI(gotHighscore);
            if(gotHighscore)
                GameModeManager.Singleton.SetHighscoreMode();
            
        }
        else if(screen == MainMenu)
        {
            LeanTween.moveLocal(Background, BG_Positions.menuPosition, 0f);
            UIManager.Singleton.ShowSettingsOrPauseIcon(true);
            GameModeManager.Singleton.SetHighscoreMode(true);
        }

        StartCoroutine(SwapScreens(currentScreen, screen));
    }

    IEnumerator SwapScreens(GameObject swapOut, GameObject swapIn)
    {
        if (swapOut != null)
        {
            swapOut.SetActive(false);
        }

        swapIn.SetActive(true);

        currentScreen = swapIn;

        if(currentScreen == Game)
        {
            GoToGame();
        }

        yield return new WaitForSeconds(transitionTime);
    }

    public GameObject GetGameScreen()
    {
        return (Game);
    }

    public GameObject GetEndScreen()
    {
        return (GameOver);
    }

    public void GoToMainMenu()
    {
        if (transition == null)
        {
            SoundManager.Singleton.LowerBGM(false);
            ExtraMainMenu_Anim(true);
        }
    }

    private void ExtraMainMenu_Anim(bool slideIn)
    {
        if (slideIn)
        {
            LeanTween.moveLocal(Background, BG_Positions.menuPosition, transitionTime).setEase(LeanTweenType.easeOutCirc);
        }
    }

    void GoToGame()
    {
        // Ground_Anim.UpdateGroundPosition(GameManager.Singleton.GetGameMode());
        LeanTween.moveLocal(Background, BG_Positions.gamePosition, 0f);

        UIManager.Singleton.ShowTimeAttackTimer(GameManager.Singleton.currentGameMode.isTimed);
        UIManager.Singleton.HideSettingsAndPauseIcon();
        UIManager.Singleton.BeginCountdown();
        
        GameManager.Singleton.ResetValues();
    }

    public void BeginStartScreen(bool bringIn = true)
    {
        if (bringIn)
            Background.transform.localPosition = BG_Positions.beginningPosition;
        transition = StartCoroutine(Start_Anim(bringIn));
    }

    IEnumerator Start_Anim(bool bringIn)
    {
        ScreenSwapping title_Swap = S_Title.GetComponent<ScreenSwapping>();
        ScreenSwapping label_Swap = S_StartLabel.GetComponent<ScreenSwapping>();

        if (bringIn)
        {
            LeanTween.moveLocal(Background, BG_Positions.startPosition, transitionTime).setEase(LeanTweenType.easeOutCirc);

            yield return new WaitForSeconds(0.75f);

            LeanTween.moveLocal(S_Title, title_Swap.inPosition, transitionTime).setEase(LeanTweenType.easeOutCubic);
            LeanTween.moveLocal(S_StartLabel, label_Swap.inPosition, transitionTime).setEase(LeanTweenType.easeOutCubic);
        }
        else
        {
            currentScreen = MainMenu;
            BeginMajorTransition_NOAUDIO(currentScreen);

            yield return new WaitForSeconds(0.5f);

            StartScreen.SetActive(false);
        }

        transition = null;
    }

    public void BeginMajorTransition(GameObject screen)
    {
        if(transition == null)
        {
            transition = StartCoroutine(MajorTransition_Anim(screen));
            cachedScreens.Clear();
            SoundManager.Singleton.PlaySound(SoundType.UI);
        }
    }

    public void BeginMajorTransition_NOAUDIO(GameObject screen)
    {
        if(transition == null)
        {
            transition = StartCoroutine(MajorTransition_Anim(screen));
            cachedScreens.Clear();
        }
    }

    IEnumerator MajorTransition_Anim(GameObject screen)
    {
        ScreenSwapping leftLeaves_SlowSwap = S_leftLeaves_Slow.GetComponent<ScreenSwapping>();
        ScreenSwapping leftLeaves_FastSwap = S_leftLeaves_Fast.GetComponent<ScreenSwapping>();
        ScreenSwapping rightLeaves_SlowSwap = S_rightLeaves_Slow.GetComponent<ScreenSwapping>();
        ScreenSwapping rightLeaves_FastSwap = S_rightLeaves_Fast.GetComponent<ScreenSwapping>();

        LeanTween.moveLocal(S_leftLeaves_Fast, leftLeaves_FastSwap.inPosition, transitionTime).setEase(LeanTweenType.easeOutCubic);
        LeanTween.moveLocal(S_leftLeaves_Slow, leftLeaves_SlowSwap.inPosition, transitionTime).setEase(LeanTweenType.easeOutCubic);
        LeanTween.moveLocal(S_rightLeaves_Fast, rightLeaves_FastSwap.inPosition, transitionTime).setEase(LeanTweenType.easeOutCubic);
        LeanTween.moveLocal(S_rightLeaves_Slow, rightLeaves_SlowSwap.inPosition, transitionTime).setEase(LeanTweenType.easeOutCubic);

        yield return new WaitForSeconds(transitionTime);

        SwitchScreen(screen);

        yield return new WaitForSeconds(transitionTime * 0.5f);

        LeanTween.moveLocal(S_leftLeaves_Fast, leftLeaves_FastSwap.outPosition, transitionTime).setEase(LeanTweenType.easeInCubic);
        LeanTween.moveLocal(S_leftLeaves_Slow, leftLeaves_SlowSwap.outPosition, transitionTime).setEase(LeanTweenType.easeInCubic);
        LeanTween.moveLocal(S_rightLeaves_Fast, rightLeaves_FastSwap.outPosition, transitionTime).setEase(LeanTweenType.easeInCubic);
        LeanTween.moveLocal(S_rightLeaves_Slow, rightLeaves_SlowSwap.outPosition, transitionTime).setEase(LeanTweenType.easeInCubic);

        transition = null;
    }

    public void MinorTransition_Specific(GameObject screen)
    {
        if (transition != null || screen == currentScreen)
            return;

        if (cachedScreens.Contains(screen))
            transition = StartCoroutine(MinorTransition_Anim(screen, false));
        else
            transition = StartCoroutine(MinorTransition_Anim(screen, true));
        SoundManager.Singleton.PlaySound(SoundType.UI);
    }

    public void MinorTransition_Back()
    {
        if(transition != null)
        {
            return;
        }
        GameObject backScreen = cachedScreens[cachedScreens.Count - 2];
        transition = StartCoroutine(MinorTransition_Anim(backScreen, false));
        SoundManager.Singleton.PlaySound(SoundType.UI);
    }

    IEnumerator MinorTransition_Anim(GameObject screen, bool isNew)
    {
        Vector3 screenOffset = new Vector3(0f, Background.GetComponent<BackgroundPositions>().transitionDistance, 0f);
        GameObject prevScreen;

        if (isNew)
        {
            if (cachedScreens.Count == 0)
            {
                cachedScreens.Add(currentScreen);
            }
            
            prevScreen = currentScreen;

            LeanTween.moveLocal(Background, Background.transform.localPosition + screenOffset, transitionTime).setEase(LeanTweenType.easeInOutQuad);
            LeanTween.moveLocal(screen, screen.transform.localPosition + screenOffset, transitionTime).setEase(LeanTweenType.easeInOutQuad);
            LeanTween.moveLocal(prevScreen, prevScreen.transform.localPosition + screenOffset, transitionTime).setEase(LeanTweenType.easeInOutQuad);

            cachedScreens.Add(screen);
            currentScreen = screen;
        }
        else
        {
            prevScreen = currentScreen;

            LeanTween.moveLocal(Background, Background.transform.localPosition - screenOffset, transitionTime).setEase(LeanTweenType.easeInOutQuad);
            LeanTween.moveLocal(screen, screen.transform.localPosition - screenOffset, transitionTime).setEase(LeanTweenType.easeInOutQuad);
            LeanTween.moveLocal(prevScreen, prevScreen.transform.localPosition - screenOffset, transitionTime).setEase(LeanTweenType.easeInOutQuad);

            cachedScreens.Remove(currentScreen);
            currentScreen = screen;
        }
        
        yield return new WaitForSeconds(transitionTime);
        transition = null;
    }
}

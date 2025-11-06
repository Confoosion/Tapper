using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager Singleton { get; private set; }
    [SerializeField] private float transitionTime = 1f;
    private Coroutine transition = null;
    private Coroutine endGame = null;

    [Space]
    [SerializeField] private GameObject currentScreen;
    [SerializeField] private List<GameObject> cachedScreens = new List<GameObject>();

    [Space]
    [Header("Moving UI Screens")]
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject Settings, Shop, ShopCategory, ArcadeEnding, TimeEnding, GameOver, Background, Leaderboard;

    [Header("Static UI Screens")]
    [SerializeField] private GameObject StartScreen;
    [SerializeField] private GameObject Game, Pause;

    [Header("Moving UI Elements")]
    // [SerializeField] private GameObject MM_Highscore;
    [SerializeField] private CelebrationAnimation GO_Celebration;
    [SerializeField] private CurrencyAnimation MM_currencies;
    // [SerializeField] private GroundAnimation Ground_Anim;
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

    public void SwitchScreen(GameObject screen)
    {
        if (transition == null)
        {
            if (screen != GameOver && screen != ArcadeEnding && screen != TimeEnding)
            {
            SoundManager.Singleton.PlaySound(SoundType.UI);
            }

            // SoundManager.Singleton.PlaySound(SoundType.UI);

            transition = StartCoroutine(SwapScreens(currentScreen, screen));
        }
    }

    IEnumerator SwapScreens(GameObject swapOut, GameObject swapIn)
    {
        // Swaps screens and calls other animations
        if (swapOut != null)
        {
            swapOut.SetActive(false);
            // SlideScreen(swapOut, Vector3.zero, false);
            // yield return new WaitForSeconds(transitionTime);
        }

        swapIn.SetActive(true);
        // SlideScreen(swapIn, Vector3.zero, true);

        currentScreen = swapIn;

        if(currentScreen == Game)
        {
            GoToGame();
        }

        yield return new WaitForSeconds(transitionTime);
        transition = null;
    }

    // private void SlideScreen(GameObject screen, Vector3 endPosition, bool slideIn)
    // {
    //     if (screen == GameOver)
    //     {
    //         if (slideIn)
    //         {
    //             LeanTween.scale(screen, endPosition, transitionTime).setEase(LeanTweenType.easeOutCubic);
    //             GoToGameOver();
    //         }
    //         else
    //         {
    //             LeanTween.scale(screen, endPosition, transitionTime).setEase(LeanTweenType.easeInCubic);
    //             GO_Celebration.AnimateCelebration(false);
    //         }
    //     }
    //     else
    //     {
    //         if (slideIn)
    //         {
    //             LeanTween.moveLocal(screen, endPosition, transitionTime).setEase(LeanTweenType.easeOutCubic);
    //             if (screen == Game)
    //             {
    //                 MM_currencies.SlideCurrencies(false);
    //                 LeanTween.moveLocal(Background, BG_Positions.gamePosition, transitionTime).setEase(LeanTweenType.easeOutCirc);
    //                 GoToGame();
    //             }
    //             else if (screen == Leaderboard)
    //             {
    //                 MM_currencies.SlideCurrencies(false);
    //             }
    //         }
    //         else
    //         {
    //             LeanTween.moveLocal(screen, endPosition, transitionTime).setEase(LeanTweenType.easeInCubic);
    //         }

    //         if (screen == MainMenu)
    //         {
    //             MM_currencies.SlideCurrencies();
    //             ExtraMainMenu_Anim(slideIn);
    //         }
    //     }
    // }

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
        // ScreenSwapping highscore_ScreenSwapping = MM_Highscore.GetComponent<ScreenSwapping>();

        if (slideIn)
        {
            // MM_Title.AnimateTitle();
            LeanTween.moveLocal(Background, BG_Positions.menuPosition, transitionTime).setEase(LeanTweenType.easeOutCirc);
            // LeanTween.moveLocal(MM_Highscore, highscore_ScreenSwapping.inPosition, transitionTime).setEase(LeanTweenType.easeOutCubic);
            // UIManager.Singleton.MM_arrows.Animate(true);
        }
        else
        {
            // MM_Title.AnimateTitle(false);
            // LeanTween.moveLocal(MM_Highscore, highscore_ScreenSwapping.outPosition, transitionTime).setEase(LeanTweenType.easeInCubic);
            // UIManager.Singleton.MM_arrows.Animate(false);
        }
    }

    void GoToGame()
    {
        // Ground_Anim.UpdateGroundPosition(GameManager.Singleton.GetGameMode());
        LeanTween.moveLocal(Background, BG_Positions.gamePosition, 0f);
        UIManager.Singleton.BeginCountdown();
        GameManager.Singleton.ResetValues();
    }

    public void GoToGameOver()
    {
        if (endGame == null)
        {
            bool gotHighscore = UIManager.Singleton.UpdateEndScore(ScoreManager.Singleton.GetPoints());
            SoundManager.Singleton.LowerBGM(false);

            UIManager.Singleton.ShowCelebration(false);
            UIManager.Singleton.ShowHighscore(false);
            UIManager.Singleton.ShowEndScreenButtons(false);

            // Ground_Anim.UpdateGroundPosition(GameMode.Classic);

            endGame = StartCoroutine(GameOver_Anim(gotHighscore));
        }
    }

    IEnumerator GameOver_Anim(bool gotHighscore)
    {
        yield return new WaitForSeconds(transitionTime * 1.5f);
        if (gotHighscore)
        {
            SoundManager.Singleton.PlaySound(SoundType.Highscore, 0.5f);
            UIManager.Singleton.ShowCelebration(true);
            GO_Celebration.AnimateCelebration();
        }
        else
        {
            UIManager.Singleton.ShowHighscore(true);
        }

        yield return new WaitForSeconds(transitionTime * 1.5f);
        UIManager.Singleton.ShowEndScreenButtons(true);

        endGame = null;
    }

    public void BeginStartScreen(bool bringIn = true)
    {
        if (bringIn)
            Background.transform.localPosition = BG_Positions.beginningPosition;
        StartCoroutine(Start_Anim(bringIn));
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
            BeginMajorTransition(currentScreen);

            yield return new WaitForSeconds(0.5f);

            StartScreen.SetActive(false);
        }
    }

    public void BeginMajorTransition(GameObject screen)
    {
        StartCoroutine(MajorTransition_Anim(screen));
        cachedScreens.Clear();
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
    }

    public void MinorTransition_Specific(GameObject screen)
    {
        if (cachedScreens.Contains(screen))
            StartCoroutine(MinorTransition_Anim(screen, false));
        else
            StartCoroutine(MinorTransition_Anim(screen, true));
    }

    public void MinorTransition_Back()
    {
        GameObject backScreen = cachedScreens[cachedScreens.Count - 2];
        StartCoroutine(MinorTransition_Anim(backScreen, false));
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

            LeanTween.moveLocal(Background, Background.transform.localPosition + screenOffset, transitionTime);
            LeanTween.moveLocal(screen, screen.transform.localPosition + screenOffset, transitionTime);
            LeanTween.moveLocal(prevScreen, prevScreen.transform.localPosition + screenOffset, transitionTime);

            cachedScreens.Add(screen);
            currentScreen = screen;
        }
        else
        {
            prevScreen = currentScreen;

            LeanTween.moveLocal(Background, Background.transform.localPosition - screenOffset, transitionTime);
            LeanTween.moveLocal(screen, screen.transform.localPosition - screenOffset, transitionTime);
            LeanTween.moveLocal(prevScreen, prevScreen.transform.localPosition - screenOffset, transitionTime);

            cachedScreens.Remove(currentScreen);
            currentScreen = screen;
        }
        
        yield return null;
    }
}

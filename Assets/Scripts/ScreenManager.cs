using UnityEngine;
using System.Collections;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager Singleton { get; private set; }
    [SerializeField] private float transitionTime = 1f;
    private Coroutine transition = null;
    private Coroutine endGame = null;

    [Header("UI Screens")]
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject Leaderboard, Settings, Game, GameOver, Background;

    [Header("Moving UI Elements")]
    [SerializeField] private TitleAnimation S_Title;
    // [SerializeField] private GameObject MM_Highscore;
    [SerializeField] private CelebrationAnimation GO_Celebration;
    [SerializeField] private CurrencyAnimation MM_currencies;
    [SerializeField] private GroundAnimation Ground_Anim;
    [SerializeField] private BackgroundPositions BG_Positions;
    [SerializeField] private GameObject leftLeaves;
    [SerializeField] private GameObject rightLeaves;

    [Space]
    [SerializeField] private ScreenSwapping currentScreen;

    void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
    }

    void Start()
    {
        BeginStartScreen();
        // currentScreen = GameObject.Find("CANVASES/MAINMENU_CANVAS/MAIN_MENU").GetComponent<ScreenSwapping>();
        // transition = StartCoroutine(SwapScreens(null, currentScreen));
    }

    public float GetTransitionTime()
    {
        return (transitionTime);
    }

    public void SwitchScreen(ScreenSwapping screen)
    {
        if (transition == null)
        {
            if (screen != GameOver.GetComponent<ScreenSwapping>())
            {
                SoundManager.Singleton.PlaySound(SoundType.UI);
            }

            transition = StartCoroutine(SwapScreens(currentScreen, screen));
        }
    }

    IEnumerator SwapScreens(ScreenSwapping swapOut, ScreenSwapping swapIn)
    {
        // Swaps screens and calls other animations
        if (swapOut != null)
        {
            SlideScreen(swapOut.gameObject, swapOut.outPosition, false);
            yield return new WaitForSeconds(transitionTime);
        }

        SlideScreen(swapIn.gameObject, swapIn.inPosition, true);

        currentScreen = swapIn;

        yield return new WaitForSeconds(transitionTime);
        transition = null;
    }

    private void SlideScreen(GameObject screen, Vector3 endPosition, bool slideIn)
    {
        if (screen == GameOver)
        {
            if (slideIn)
            {
                LeanTween.scale(screen, endPosition, transitionTime).setEase(LeanTweenType.easeOutCubic);
                GoToGameOver();
            }
            else
            {
                LeanTween.scale(screen, endPosition, transitionTime).setEase(LeanTweenType.easeInCubic);
                GO_Celebration.AnimateCelebration(false);
            }
        }
        else
        {
            if (slideIn)
            {
                LeanTween.moveLocal(screen, endPosition, transitionTime).setEase(LeanTweenType.easeOutCubic);
                if (screen == Game)
                {
                    MM_currencies.SlideCurrencies(false);
                    LeanTween.moveLocal(Background, BG_Positions.gamePosition, transitionTime).setEase(LeanTweenType.easeOutCirc);
                    GoToGame();
                }
                else if(screen == Leaderboard)
                {
                    MM_currencies.SlideCurrencies(false);
                }
            }
            else
            {
                LeanTween.moveLocal(screen, endPosition, transitionTime).setEase(LeanTweenType.easeInCubic);
            }

            if (screen == MainMenu)
            {
                MM_currencies.SlideCurrencies();
                ExtraMainMenu_Anim(slideIn);
            }
        }
    }

    public ScreenSwapping GetEndScreen()
    {
        return (GameOver.GetComponent<ScreenSwapping>());
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
        if(bringIn)
            Background.transform.localPosition = BG_Positions.beginningPosition;
        transition = StartCoroutine(Start_Anim(bringIn));
    }

    IEnumerator Start_Anim(bool bringIn)
    {
        ScreenSwapping leftLeaves_Swap = leftLeaves.GetComponent<ScreenSwapping>();
        ScreenSwapping rightLeaves_Swap = rightLeaves.GetComponent<ScreenSwapping>();

        if (bringIn)
        {
            LeanTween.moveLocal(Background, BG_Positions.startPosition, transitionTime).setEase(LeanTweenType.easeOutCirc);

            yield return new WaitForSeconds(0.75f);
            LeanTween.moveLocal(leftLeaves, leftLeaves_Swap.inPosition, transitionTime).setEase(LeanTweenType.easeOutCubic);
            LeanTween.moveLocal(rightLeaves, rightLeaves_Swap.inPosition, transitionTime).setEase(LeanTweenType.easeOutCubic);

            S_Title.AnimateTitle();
        }
        else
        {
            LeanTween.moveLocal(leftLeaves, leftLeaves_Swap.outPosition, transitionTime).setEase(LeanTweenType.easeInCubic);
            LeanTween.moveLocal(rightLeaves, rightLeaves_Swap.outPosition, transitionTime).setEase(LeanTweenType.easeInCubic);

            yield return new WaitForSeconds(0.75f);

            // LeanTween.moveLocal(Background, BG_Positions.menuPosition, transitionTime).setEase(LeanTweenType.easeOutCirc);

            // yield return new WaitForSeconds(0.5f);

            currentScreen = GameObject.Find("CANVASES/MAINMENU_CANVAS/MAIN_MENU").GetComponent<ScreenSwapping>();
            transition = StartCoroutine(SwapScreens(null, currentScreen));

            S_Title.AnimateTitle(false);
        }
        transition = null;
    }
}

using UnityEngine;
using System.Collections;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager Singleton { get; private set; }
    [SerializeField] private float transitionTime = 1f;
    private Coroutine transition = null;

    [Header("UI Screens")]
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject Themes, Settings, Game, GameOver;

    [Header("Moving UI Elements")]
    [SerializeField] private GameObject MM_Highscore;
    [SerializeField] private GameObject GO_Celebration;

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
        currentScreen = GameObject.Find("CANVASES/MAINMENU_CANVAS/MAIN_MENU").GetComponent<ScreenSwapping>();
        transition = StartCoroutine(SwapScreens(null, currentScreen));
    }

    public void SwitchScreen(ScreenSwapping screen)
    {
        if (transition == null)
        {
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
            }
        }
        else
        {
            if (slideIn)
            {
                LeanTween.moveLocal(screen, endPosition, transitionTime).setEase(LeanTweenType.easeOutCubic);
            }
            else
            {
                LeanTween.moveLocal(screen, endPosition, transitionTime).setEase(LeanTweenType.easeInCubic);
            }

            if (screen == MainMenu)
            {
                ExtraMainMenu_Anim(slideIn);
            }
        }
    }

    // private void SlideScreenOut(GameObject screen, Vector3 endPosition)
    // {
    //     if (screen == GameOver)
    //     {
    //         LeanTween.scale(screen, endPosition, transitionTime).setEase(LeanTweenType.easeOutCubic);
    //     }
    //     else
    //     {
    //         LeanTween.moveLocal(screen, endPosition, transitionTime).setEase(LeanTweenType.easeInCubic);
    //     }
    // }


    public void GoToMainMenu()
    {
        if (transition == null)
        {
            SoundManager.Singleton.PlaySound(SoundType.UI);
            SoundManager.Singleton.LowerBGM(false);
            ExtraMainMenu_Anim(true);
        }
    }

    private void ExtraMainMenu_Anim(bool slideIn)
    {
        if (slideIn)
        {
            LeanTween.moveLocal(MM_Highscore, new Vector3(0f, 650f, 0f), transitionTime).setEase(LeanTweenType.easeOutCubic);
        }
        else
        {
            LeanTween.moveLocal(MM_Highscore, new Vector3(0f, 2400f, 0f), transitionTime).setEase(LeanTweenType.easeInCubic);
        }
        // LeanTween.moveLocal(MainMenu, new Vector3(0f, 0f, 0f), transitionTime).setEase(LeanTweenType.easeOutCubic);
    }

    // public void GoToThemes()
    // {
    //     if (transition == null)
    //     {
    //         SoundManager.Singleton.PlaySound(SoundType.UI);
    //         transition = StartCoroutine(Themes());
    //     }
    // }

    // IEnumerator Themes()
    // {
    //     mainMenu_anim.SetBool("In_MainMenu", false);
    //     yield return new WaitForSeconds(transitionTime);
    //     title_anim.speed = 0f;
    //     themes_anim.SetBool("In_Themes", true);
    //     transition = null;
    // }

    // public void GoToSettings()
    // {
    //     if (transition == null)
    //     {
    //         SoundManager.Singleton.PlaySound(SoundType.UI);
    //         transition = StartCoroutine(Settings());
    //     }
    // }

    // IEnumerator Settings()
    // {
    //     mainMenu_anim.SetBool("In_MainMenu", false);
    //     yield return new WaitForSeconds(transitionTime);
    //     title_anim.speed = 0f;
    //     settings_anim.SetBool("In_Settings", true);
    //     transition = null;
    // }

    // public void GoToGame()
    // {
    //     if (transition == null)
    //     {
    //         SoundManager.Singleton.PlaySound(SoundType.UI);
    //         SoundManager.Singleton.LowerBGM();
    //         transition = StartCoroutine(Game());
    //     }
    // }

    // IEnumerator Game()
    // {
    //     gameOver_anim.SetBool("In_GameOver", false);
    //     mainMenu_anim.SetBool("In_MainMenu", false);
    //     yield return new WaitForSeconds(transitionTime);
    //     title_anim.speed = 0f;
    //     game_anim.SetBool("In_Game", true);
    //     transition = null;
    // }

    public void GoToGameOver()
    {
        if (transition == null)
        {
            bool gotHighscore = UIManager.Singleton.UpdateEndScore(ScoreManager.Singleton.GetPoints());
            SoundManager.Singleton.LowerBGM(false);

            UIManager.Singleton.ShowCelebration(false);
            UIManager.Singleton.ShowHighscore(false);
            UIManager.Singleton.ShowEndScreenButtons(false);

            transition = StartCoroutine(GameOver_Anim(gotHighscore));
        }
    }

    IEnumerator GameOver_Anim(bool gotHighscore)
    {
        if (gotHighscore)
        {
            SoundManager.Singleton.PlaySound(SoundType.Highscore, 0.5f);
            UIManager.Singleton.ShowCelebration(true);
        }
        else
        {
            UIManager.Singleton.ShowHighscore(true);
        }

        yield return new WaitForSeconds(transitionTime);
        UIManager.Singleton.ShowEndScreenButtons(true);

        transition = null;
    }
}

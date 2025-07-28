using UnityEngine;
using System.Collections;

public enum ScreensEnum { MainMenu, Themes, Settings, Game, GameOver }
public class ScreenManager : MonoBehaviour
{
    public static ScreenManager Singleton { get; private set; }
    [SerializeField] private float transitionTime = 1f;
    private Coroutine transition = null;

    [Header("UI Screens")]
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject Themes, Settings, Game, GameOver;

    [Header("Moving UI Elements")]
    [SerializeField] private GameObject GO_Celebration;

    // [SerializeField] private Animator title_anim;
    // [SerializeField] private Animator mainMenu_anim;
    // [SerializeField] private Animator themes_anim;
    // [SerializeField] private Animator settings_anim;
    // [SerializeField] private Animator game_anim;
    // [SerializeField] private Animator gameOver_anim;

    void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
    }

    void Start()
    {
        // title_anim = GameObject.Find("SCREENS/MAIN_MENU/Title").GetComponent<Animator>();
        // mainMenu_anim = GameObject.Find("SCREENS/MAIN_MENU").GetComponent<Animator>();
        // themes_anim = GameObject.Find("SCREENS/THEMES").GetComponent<Animator>();
        // settings_anim = GameObject.Find("SCREENS/SETTINGS").GetComponent<Animator>();
        // game_anim = GameObject.Find("SCREENS/GAME").GetComponent<Animator>();
        // gameOver_anim = GameObject.Find("SCREENS/GAMEOVER").GetComponent<Animator>();
    }

    IEnumerator SwapScreens(GameObject swapOut, GameObject swapIn)
    {
        // Swaps screens and calls other animations
        yield return null;
    }

    public void GoToMainMenu()
    {
        if (transition == null)
        {
            // SoundManager.Singleton.PlaySound(SoundType.UI);
            SoundManager.Singleton.LowerBGM(false);
            transition = StartCoroutine(MainMenu_Anim());
        }
    }

    IEnumerator MainMenu_Anim()
    {
        // gameOver_anim.SetBool("In_GameOver", false);
        // themes_anim.SetBool("In_Themes", false);
        // settings_anim.SetBool("In_Settings", false);
        yield return new WaitForSeconds(transitionTime);
        LeanTween.moveLocal(MainMenu, new Vector3(0f, 0f, 0f), transitionTime).setEase(LeanTweenType.easeOutCubic);
        transition = null;
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

    // public void GoToGameOver()
    // {
    //     if (transition == null)
    //     {
    //         bool gotHighscore = UIManager.Singleton.UpdateEndScore(ScoreManager.Singleton.GetPoints());
    //         SoundManager.Singleton.LowerBGM(false);

    //         UIManager.Singleton.ShowCelebration(false);
    //         UIManager.Singleton.ShowHighscore(false);
    //         UIManager.Singleton.ShowEndScreenButtons(false);

    //         transition = StartCoroutine(GameOver(gotHighscore));
    //     }
    // }

    // IEnumerator GameOver(bool gotHighscore)
    // {
    //     game_anim.SetBool("In_Game", false);
    //     yield return new WaitForSeconds(transitionTime);
    //     gameOver_anim.SetBool("In_GameOver", true);

    //     yield return new WaitForSeconds(transitionTime);
    //     if (gotHighscore)
    //     {
    //         SoundManager.Singleton.PlaySound(SoundType.Highscore, 0.5f);
    //         UIManager.Singleton.ShowCelebration(true);
    //     }
    //     else
    //     {
    //         UIManager.Singleton.ShowHighscore(true);
    //     }

    //     yield return new WaitForSeconds(transitionTime);
    //     UIManager.Singleton.ShowEndScreenButtons(true);

    //     transition = null;
    // }
}

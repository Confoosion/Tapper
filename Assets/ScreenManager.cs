using UnityEngine;
using System.Collections;

public class ScreenManager : MonoBehaviour
{
    [SerializeField] private float transitionTime = 1f;
    private Coroutine transition = null;

    [SerializeField] private Animator title_anim;
    [SerializeField] private Animator mainMenu_anim;
    [SerializeField] private Animator themes_anim;
    [SerializeField] private Animator settings_anim;
    [SerializeField] private Animator game_anim;
    [SerializeField] private Animator gameOver_anim;

    void Start()
    {
        title_anim = GameObject.Find("SCREENS/MAIN_MENU/Title").GetComponent<Animator>();
        mainMenu_anim = GameObject.Find("SCREENS/MAIN_MENU").GetComponent<Animator>();
        themes_anim = GameObject.Find("SCREENS/THEMES").GetComponent<Animator>();
        settings_anim = GameObject.Find("SCREENS/SETTINGS").GetComponent<Animator>();
        game_anim = GameObject.Find("SCREENS/GAME").GetComponent<Animator>();
        gameOver_anim = GameObject.Find("SCREENS/GAMEOVER").GetComponent<Animator>();
    }

    public void GoToMainMenu()
    {
        if (transition == null)
        {
            transition = StartCoroutine(MainMenu());
        }
    }

    IEnumerator MainMenu()
    {
        gameOver_anim.SetBool("In_GameOver", false);
        themes_anim.SetBool("In_Themes", false);
        settings_anim.SetBool("In_Settings", false);
        yield return new WaitForSeconds(transitionTime);
        title_anim.speed = 1f;
        mainMenu_anim.SetBool("In_MainMenu", true);
        transition = null;
    }

    public void GoToThemes()
    {
        if (transition == null)
        {
            transition = StartCoroutine(Themes());
        }
    }

    IEnumerator Themes()
    {
        mainMenu_anim.SetBool("In_MainMenu", false);
        yield return new WaitForSeconds(transitionTime);
        title_anim.speed = 0f;
        themes_anim.SetBool("In_Themes", true);
        transition = null;
    }

    public void GoToSettings()
    {
        if (transition == null)
        {
            transition = StartCoroutine(Settings());
        }
    }

    IEnumerator Settings()
    {
        mainMenu_anim.SetBool("In_MainMenu", false);
        yield return new WaitForSeconds(transitionTime);
        title_anim.speed = 0f;
        settings_anim.SetBool("In_Settings", true);
        transition = null;
    }

    public void GoToGame()
    {
        if (transition == null)
        {
            transition = StartCoroutine(Game());
        }
    }

    IEnumerator Game()
    {
        mainMenu_anim.SetBool("In_MainMenu", false);
        yield return new WaitForSeconds(transitionTime);
        title_anim.speed = 0f;
        game_anim.SetBool("In_Game", true);
        transition = null;
    }

    public void GoToGameOver()
    {
        if (transition == null)
        {
            transition = StartCoroutine(GameOver());
        }
    }

    IEnumerator GameOver()
    {
        game_anim.SetBool("In_Game", false);
        yield return new WaitForSeconds(transitionTime);
        gameOver_anim.SetBool("In_GameOver", true);
        transition = null;
    }
}

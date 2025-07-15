using UnityEngine;

public class ScreenSwapping : MonoBehaviour
{
    [SerializeField] private Animator screenAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        screenAnimator = GetComponent<Animator>();
    }

    public void SwapToMainMenu()
    {
        if (screenAnimator.GetBool("In_Themes"))
        {
            screenAnimator.SetBool("In_Themes", false);
        }
        else
        {
            screenAnimator.SetBool("In_Settings", false);
        }
        screenAnimator.SetBool("In_MainMenu", true);
    }

    public void SwapToTheme()
    {
        screenAnimator.SetBool("In_MainMenu", false);
        screenAnimator.SetBool("In_Themes", true);
    }

    public void SwapToSettings()
    {
        screenAnimator.SetBool("In_MainMenu", false);
        screenAnimator.SetBool("In_Settings", true);
    }
}

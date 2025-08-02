using UnityEngine;

public class CurrencyAnimation : MonoBehaviour
{
    [SerializeField] private GameObject gemContainer;
    [SerializeField] private GameObject scoreContainer;
    private ScreenSwapping gemSwapper;
    private ScreenSwapping scoreSwapper;

    private bool isShown = false;

    void Start()
    {
        gemSwapper = gemContainer.GetComponent<ScreenSwapping>();
        scoreSwapper = scoreContainer.GetComponent<ScreenSwapping>();
    }

    public void SlideCurrencies(bool slideIn = true)
    {
        if ((slideIn && isShown) || (!slideIn && !isShown))
        {
            return;
        }

        float transition = ScreenManager.Singleton.GetTransitionTime();
        if (slideIn)
        {
            LeanTween.moveLocal(gemContainer, gemSwapper.inPosition, transition).setEase(LeanTweenType.easeOutCubic);
            LeanTween.moveLocal(scoreContainer, scoreSwapper.inPosition, transition).setEase(LeanTweenType.easeOutCubic);
            isShown = true;
        }
        else
        {
            LeanTween.moveLocal(gemContainer, gemSwapper.outPosition, transition).setEase(LeanTweenType.easeInCubic);
            LeanTween.moveLocal(scoreContainer, scoreSwapper.outPosition, transition).setEase(LeanTweenType.easeInCubic);
            isShown = false;
        }
    }
}

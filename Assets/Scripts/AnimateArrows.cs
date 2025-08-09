using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnimateArrows : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] RectTransform rectTransform;

    private float fadeDuration = 1f;
    private bool isAnimating = false;
    private Coroutine animateRoutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();

        Animate(true);
    }

    public void Animate(bool doAnim)
    {
        animateRoutine = StartCoroutine(DoAnimation(doAnim));
    }

    IEnumerator DoAnimation(bool doAnim)
    {
        if (!doAnim)
        {
            animateRoutine = null;    
        }

        isAnimating = doAnim;
        while (isAnimating)
        {
            StartCoroutine(Dissipate(fadeDuration, false));
            // StartCoroutine(MoveArrows(fadeDuration * 0.5f, true));
            LeanTween.scaleX(this.gameObject, 0.85f, fadeDuration).setEase(LeanTweenType.easeInOutCubic);
            yield return new WaitForSeconds(fadeDuration);
            StartCoroutine(Dissipate(fadeDuration, true));
            // StartCoroutine(MoveArrows(fadeDuration * 0.5f, false));
            LeanTween.scaleX(this.gameObject, 1f, fadeDuration).setEase(LeanTweenType.easeInOutCubic);
            yield return new WaitForSeconds(fadeDuration);
        }
    }

    IEnumerator Dissipate(float duration, bool fadeOut)
    {
        float a = canvasGroup.alpha;
        // Debug.Log(a);
        float fadeTime = 0f;
        float fadeTarget = 1f;

        if (fadeOut)
        {
            fadeTarget = 0f;
        }

        while (fadeTime < duration)
        {
            fadeTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(a, fadeTarget, fadeTime / duration);
            canvasGroup.alpha = newAlpha;
            yield return null;
        }

        canvasGroup.alpha = fadeTarget;
    }
}

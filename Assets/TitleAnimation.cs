using UnityEngine;
using System.Collections;

public class TitleAnimation : MonoBehaviour
{
    [SerializeField] private GameObject title;
    [SerializeField] private float transitionTime = 1f;
    [SerializeField] private Vector3 MIN_SCALE, MAX_SCALE;
    [SerializeField] private Vector3 LEFT_ROTATE, RIGHT_ROTATE;

    private Coroutine anim;
    private bool isAnimating;

    void Awake()
    {
        title = this.gameObject;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AnimateTitle(true);
    }

    public void AnimateTitle(bool doAnimation = false)
    {
        if (anim == null)
        {
            if (!doAnimation)
            {
                return;
            }

            isAnimating = doAnimation;
            anim = StartCoroutine(Title_Anim());
        }
        else
        {
            if (!doAnimation)
            {
                isAnimating = doAnimation;
            }
        }
    }

    public IEnumerator Title_Anim()
    {
        while (isAnimating)
        {
            LeanTween.scale(title, MIN_SCALE, transitionTime).setEase(LeanTweenType.easeInOutSine);
            LeanTween.rotate(title, LEFT_ROTATE, transitionTime).setEase(LeanTweenType.easeInOutSine);
            yield return new WaitForSeconds(transitionTime);

            LeanTween.scale(title, MAX_SCALE, transitionTime).setEase(LeanTweenType.easeInOutSine);
            LeanTween.rotate(title, RIGHT_ROTATE, transitionTime).setEase(LeanTweenType.easeInOutSine);
            yield return new WaitForSeconds(transitionTime);
        }

        anim = null;
    }
}

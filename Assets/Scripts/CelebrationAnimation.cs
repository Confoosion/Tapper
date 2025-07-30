using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class CelebrationAnimation : MonoBehaviour
{
    private GameObject celebrationObject;
    private TextMeshProUGUI celebrationText;
    [SerializeField] private float transitionTime = 1f;
    [SerializeField] private Vector3 MIN_SCALE, MAX_SCALE;
    [SerializeField] List<Color> colors = new List<Color>();

    private Coroutine color_anim;
    private Coroutine scale_anim;
    private bool isAnimating;

    void Awake()
    {
        celebrationObject = this.gameObject;
        celebrationText = GetComponent<TextMeshProUGUI>();
    }

    // void Start()
    // {
    //     AnimateCelebration();
    // }

    public void AnimateCelebration(bool doAnimation = true)
    {
        if (color_anim == null && scale_anim == null)
        {
            if (!doAnimation)
            {
                return;
            }

            isAnimating = doAnimation;
            color_anim = StartCoroutine(Celebration_Color(0f, transitionTime));
            scale_anim = StartCoroutine(Celebration_Scaling());
        }
        else
        {
            if (!doAnimation)
            {
                isAnimating = doAnimation;
            }
        }
    }

    IEnumerator Celebration_Color(float start, float end)
    {
        int currColorIndex = 0;
        int nextColorIndex = (currColorIndex + 1) % colors.Count;
        float timeElapsed;
        Color lerpedColor;

        while (isAnimating)
        {
            timeElapsed = start;
            while (timeElapsed < end)
            {
                float t = timeElapsed / end;
                lerpedColor = Color.Lerp(colors[currColorIndex], colors[nextColorIndex], t);
                timeElapsed += Time.deltaTime;

                celebrationText.color = lerpedColor;

                yield return null;
            }

            celebrationText.color = colors[nextColorIndex];

            currColorIndex = nextColorIndex;
            nextColorIndex = (currColorIndex + 1) % colors.Count;
        }

        color_anim = null;
    }

    IEnumerator Celebration_Scaling()
    {
        while (isAnimating)
        {
            LeanTween.scale(celebrationObject, MAX_SCALE, transitionTime).setEase(LeanTweenType.easeInOutCubic);
            yield return new WaitForSeconds(transitionTime);
            LeanTween.scale(celebrationObject, MIN_SCALE, transitionTime).setEase(LeanTweenType.easeInOutCubic);
            yield return new WaitForSeconds(transitionTime);
        }

        scale_anim = null;
    }
}

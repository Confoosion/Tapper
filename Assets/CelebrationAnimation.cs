using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CelebrationAnimation : MonoBehaviour
{
    [SerializeField] private GameObject celebration;
    [SerializeField] private float transitionTime = 1f;
    [SerializeField] private Vector3 MIN_SCALE, MAX_SCALE;
    [SerializeField] List<Color> colors = new List<Color>();

    private Coroutine anim;
    private bool isAnimating;

    void Awake()
    {
        celebration = this.gameObject;
    }

    public void AnimateCelebration(bool doAnimation = true)
    {
        if (anim == null)
        {
            if (!doAnimation)
            {
                return;
            }

            isAnimating = doAnimation;
            anim = StartCoroutine(Celebration_Anim());
        }
        else
        {
            if (!doAnimation)
            {
                isAnimating = doAnimation;
            }
        }
    }

    IEnumerator Celebration_Anim()
    {
        while (isAnimating)
        {
            for (int i = 0; i < colors.Count; i++)
            {
                LeanTween.color(celebration, colors[i], transitionTime).setDelay(transitionTime * i);
            }

            yield return new WaitForSeconds(transitionTime * colors.Count);
        }

        anim = null;
    }
}

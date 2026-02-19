using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ObjectBounceVFX : MonoBehaviour
{
    [Header("Bounce Settings")]
    [SerializeField] private float bounceScale = 1.2f;
    [SerializeField] private float bounceDuration = 0.3f;
    [SerializeField] private AnimationCurve bounceCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Coroutine currentBounce;
    private Vector3 originalScale;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = transform.localScale;
    }

    public void Bounce()
    {
        if (currentBounce != null)
        {
            StopCoroutine(currentBounce);
        }
        currentBounce = StartCoroutine(BouncyEffect());
    }

    IEnumerator BouncyEffect()
    {
        float elapsed = 0f;
        
        // Scale up phase
        while (elapsed < bounceDuration / 2)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / (bounceDuration / 2);
            float curveValue = bounceCurve.Evaluate(t);
            
            rectTransform.localScale = Vector3.Lerp(originalScale, originalScale * bounceScale, curveValue);
            yield return null;
        }
        
        elapsed = 0f;
        
        // Scale down phase
        while (elapsed < bounceDuration / 2)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / (bounceDuration / 2);
            float curveValue = bounceCurve.Evaluate(t);
            
            rectTransform.localScale = Vector3.Lerp(originalScale * bounceScale, originalScale, curveValue);
            yield return null;
        }
        
        // Ensure we end at exact original scale
        rectTransform.localScale = originalScale;
        currentBounce = null;
    }
}

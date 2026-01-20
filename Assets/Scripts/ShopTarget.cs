using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ShopTarget : MonoBehaviour, IPointerDownHandler
{
    public enum TargetType { Bad, Small, Fast, Good }
    [SerializeField] private TargetType targetType;

    [Space]
    
    [Header("Bounce Settings")]
    [SerializeField] private float bounceScale = 1.2f;
    [SerializeField] private float bounceDuration = 0.3f;
    [SerializeField] private AnimationCurve bounceCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    private RectTransform rectTransform;
    private Vector3 originalScale;
    private Coroutine currentBounce;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Stop any existing bounce to prevent overlapping animations
        if (currentBounce != null)
        {
            StopCoroutine(currentBounce);
        }
        
        currentBounce = StartCoroutine(BouncyEffect());
        ShopManager.Singleton.PreviewAnimalSound((int)targetType);
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
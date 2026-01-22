using UnityEngine;
using System.Collections;

public class TargetAnimation : FrameAnimation
{
    [SerializeField] private Sprite[] hitFrames;

    
    [Header("Jiggle Settings")]
    [SerializeField] private float jiggleIntensity = 0.15f;
    [SerializeField] private float jiggleFrequency = 20f;
    [SerializeField] private float jiggleDuration = 0.5f;
    [SerializeField] private float jiggleDamping = 3f;

    private RectTransform rectTransform;
    private Vector3 originalScale;
    private Coroutine currentJellyEffect;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            originalScale = rectTransform.localScale;
        }
    }

    IEnumerator JiggleThenHit()
    {
        // Jiggle effect if we have a RectTransform
        if (rectTransform != null)
        {
            float elapsed = 0f;
            
            while (elapsed < jiggleDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / jiggleDuration;
                
                float dampFactor = Mathf.Exp(-jiggleDamping * t);
                
                float jiggleX = Mathf.Sin(elapsed * jiggleFrequency) * jiggleIntensity * dampFactor;
                float jiggleY = Mathf.Sin(elapsed * jiggleFrequency + Mathf.PI * 0.5f) * jiggleIntensity * dampFactor;
                
                rectTransform.localScale = new Vector3(
                    originalScale.x * (1f + jiggleX),
                    originalScale.y * (1f + jiggleY),
                    originalScale.z
                );
                
                yield return null;
            }
            
            // Reset to original scale
            rectTransform.localScale = originalScale;
        }
        
        // After jiggle effect completes, play hit animation
        StartAnimation(hitFrames, false);
        
        currentJellyEffect = null;
    }

    public void ChangeAnimationTiming(float scale)
    {
        // Debug.Log("RETIMING FOR " + this.gameObject);
        if(scale == 0f)
        {
            ResetAnimValues();
            return;
        }

        frameInterval = FRAME_INTERVAL / (scale + 1f);
        idleDuration = IDLE_DURATION / (scale + 1f);
    }

    public void StartHitAnimation()
    {
        activateEndAction = true;
        // StartAnimation(hitFrames, false);
        currentJellyEffect = StartCoroutine(JiggleThenHit());
    }

    public void QueueHitAnimation()
    {
        activateEndAction = true;
        QueueAnimation(hitFrames, false);
    }

    public float GetHitTime()
    {
        // Debug.Log("HIT TIME = " + (idleDuration + (float)enterFrames.Length * 0.01f));
        return(idleDuration + (float)enterFrames.Length * 0.01f);
    }

    public void ActivateEndAction()
    {
        activateEndAction = true;
    }
}

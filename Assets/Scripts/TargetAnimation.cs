using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class TargetAnimation : MonoBehaviour
{
    public enum TargetStates { Enter, Idle, Exit, Hit }

    [Header("Enter Settings")]
    [SerializeField] private Sprite startingFrame;
    [SerializeField] private Sprite enterFrame;
    [SerializeField] private float enterTime;

    [Space]

    [Header("Idle Settings")]
    [SerializeField] private Sprite[] idleFrames;
    [SerializeField] private float idleTime;
    [SerializeField] private float frameInterval = 0.01f;

    [Space]

    [Header("Hit Settings")]
    [SerializeField] private Sprite hitFrame;
    [SerializeField] private float hitTime;

    [Space]

    [Header("Exit Settings")]
    [SerializeField] private float exitTime;
    public bool activateEndAction;
    public UnityEvent EndAction;

    [Space]

    private Queue<TargetStates> animationQueue = new Queue<TargetStates>();
    private bool isProcessingQueue = false;
    private Coroutine currentTargetAnim;
    private Coroutine targetAnim;
    private bool interrupted;

    
    // [Header("Jiggle Settings")]
    // [SerializeField] private float jiggleIntensity = 0.15f;
    // [SerializeField] private float jiggleFrequency = 20f;
    // [SerializeField] private float jiggleDuration = 0.5f;
    // [SerializeField] private float jiggleDamping = 3f;

    private RectTransform rectTransform;
    private Image targetFrame;
    private Vector3 originalScale;
    private Coroutine currentJellyEffect;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            originalScale = rectTransform.localScale;
        }
        targetFrame = GetComponent<Image>();

    }

    IEnumerator ProcessAnimationQueue()
    {
        isProcessingQueue = true;

        while (animationQueue.Count > 0)
        {
            TargetStates currentState = animationQueue.Dequeue();
            currentTargetAnim = StartCoroutine(PlayAnimation(currentState));
            yield return currentTargetAnim;
            currentTargetAnim = null;

            if(interrupted)
            {
                break;
            }
        }

        isProcessingQueue = false;
        targetAnim = null;

        if(activateEndAction && !interrupted)
        {
            EndAction.Invoke();
        }
    }

    IEnumerator PlayAnimation(TargetStates state)
    {
        Coroutine playAnim;
        interrupted = false;
        bool shouldContinue = true;
        float loopStartTime = Time.time;

        switch(state)
        {
            case TargetStates.Enter:
                {
                    playAnim = StartCoroutine(_PlayEnterAnim());
                    yield return playAnim;
                    break;
                }
            case TargetStates.Idle:
                {
                    playAnim = StartCoroutine(_PlayIdleAnim());
                    yield return playAnim;
                    break;
                }
            case TargetStates.Exit:
                {
                    playAnim = StartCoroutine(_PlayExitAnim());
                    yield return playAnim;
                    break;
                }
            case TargetStates.Hit:
                {
                    playAnim = StartCoroutine(_PlayHitAnim());
                    yield return playAnim;
                    break;
                }
        }

        // while (shouldContinue)
        // {
            // yield return new WaitForSeconds(animationDelay);
            
            // foreach (Sprite frame in animData.frames)
            // {
            //     if(interrupted)
            //     {
            //         break;
            //     }

            //     spriteImage.sprite = frame;
                
            //     yield return new WaitForSeconds(frameInterval);
            // }

            // if (!animData.loop || interrupted)
            // {
            //     shouldContinue = false;
            // }
            // else if (animData.loopDuration.HasValue)
            // {
            //     if (Time.time - loopStartTime >= animData.loopDuration.Value)
            //     {
            //         shouldContinue = false;
            //     }
            // }
            // If loop is true and loopDuration is null, continue indefinitely
        // }
    }

    public void QueueAnimation(TargetStates state)
    {
        animationQueue.Enqueue(state);

        if (!isProcessingQueue)
        {
            targetAnim = StartCoroutine(ProcessAnimationQueue());
        }
    }

    public void QueueEnterAnimation()
    {
        QueueAnimation(TargetStates.Enter);
    }

    public void QueueIdleAnimation()
    {
        QueueAnimation(TargetStates.Idle);
    }

    public void QueueExitAnimation()
    {
        QueueAnimation(TargetStates.Exit);
    }

    public void QueueHitAnimation()
    {
        QueueAnimation(TargetStates.Hit);
    }

    public void StartAnimation(TargetStates state)
    {
        interrupted = true;
        ClearQueue();
        interrupted = false;
        QueueAnimation(state);
    }

    public void StartEnterAnimation()
    {
        StartAnimation(TargetStates.Enter);
    }

    public void StartIdleAnimation()
    {
        StartAnimation(TargetStates.Idle);
    }

    public void StartExitAnimation()
    {
        StartAnimation(TargetStates.Exit);
    }

    public void StartHitAnimation()
    {
        StartAnimation(TargetStates.Hit);
    }

    public void ClearQueue()
    {
        animationQueue.Clear();
        
        if (currentTargetAnim != null)
        {
            StopCoroutine(currentTargetAnim);
            currentTargetAnim = null;
        }

        if (targetAnim != null)
        {
            StopCoroutine(targetAnim);
            targetAnim = null;
            isProcessingQueue = false;
        }    
    }

    IEnumerator _PlayEnterAnim()
    {
        yield return new WaitForSeconds(2f);    // For testing

        yield return new WaitForSeconds(frameInterval);
        rectTransform.localScale = new Vector3(0f, 0f, 0f);
        targetFrame.sprite = enterFrame;

        LeanTween.scale(this.gameObject, originalScale, enterTime).setEase(LeanTweenType.easeOutElastic);
        yield return new WaitForSeconds(enterTime);
    }

    IEnumerator _PlayIdleAnim()
    {
        if(idleFrames.Length < 2)
        {
            yield return new WaitForSeconds(idleTime);
        }
        else
        {
            float loopStartTime = Time.time;
            while(Time.time - loopStartTime < idleTime)
            {
                foreach(Sprite frame in idleFrames)
                {
                    yield return new WaitForSeconds(frameInterval);
                    targetFrame.sprite = frame;
                }
            }   
        }
    }

    IEnumerator _PlayExitAnim()
    {
        // yield return new WaitForSeconds(frameInterval);
        // rectTransform.localScale = new Vector3(0f, 0f, 0f);
        // targetFrame.sprite = enterFrame;

        LeanTween.scale(this.gameObject, new Vector3(0f, 0f, 0f), exitTime).setEase(LeanTweenType.easeOutCubic);
        yield return new WaitForSeconds(exitTime);
    }

    IEnumerator _PlayHitAnim()
    {
        targetFrame.sprite = hitFrame;
        rectTransform.localScale = originalScale * 0.75f;
        LeanTween.scale(this.gameObject, originalScale, hitTime * 0.75f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(this.gameObject, new Vector3(0f, 0f, 0f), hitTime * 0.25f).setEase(LeanTweenType.easeOutCubic).setDelay(hitTime * 0.7f);
        yield return new WaitForSeconds(hitTime);
    }

    public float GetHitTime()
    {
        // Debug.Log("HIT TIME = " + (idleDuration + (float)enterFrames.Length * 0.01f));
        return(enterTime + idleTime);
    }

    public void ActivateEndAction()
    {
        // activateEndAction = true;
    }

    public void StartFullAnimation()
    {
        QueueEnterAnimation();
        QueueIdleAnimation();
        QueueExitAnimation();
        // QueueHitAnimation();
    }
}

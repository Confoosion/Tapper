using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class FrameAnimation : MonoBehaviour
{
    public Image spriteImage;
    private Coroutine frameAnim = null;
    private bool interrupted = false;
    [SerializeField] private float animationDelay;
    [SerializeField] protected float FRAME_INTERVAL = 0.01f;
    [SerializeField] private bool animateIdleOnEnable;
    [SerializeField] protected bool activateEndAction;
    public UnityEvent EndAction;
    [SerializeField] private Sprite startingFrame;

    [SerializeField] protected Sprite[] enterFrames;
    [SerializeField] private Sprite[] idleFrames;
    [SerializeField] protected float IDLE_DURATION;
    [SerializeField] private Sprite[] exitFrames;

    protected float frameInterval;
    protected float idleDuration;

    private Queue<AnimationData> animationQueue = new Queue<AnimationData>();
    private bool isProcessingQueue = false;

    private class AnimationData
    {
        public Sprite[] frames;
        public bool loop;
        public float? loopDuration; // null means loop indefinitely

        public AnimationData(Sprite[] frames, bool loop, float? loopDuration = null)
        {
            this.frames = frames;
            this.loop = loop;
            this.loopDuration = loopDuration;
        }
    }

    void Awake()
    {
        frameInterval = FRAME_INTERVAL;
        idleDuration = IDLE_DURATION;
    }

    void Start()
    {
        spriteImage.sprite = startingFrame;
    }

    void OnEnable()
    {
        if(animateIdleOnEnable)
        {
            StartIdleAnimation();
        }
    }

    void OnDisable()
    {
        if(animateIdleOnEnable)
        {
            StopALLAnimations();
        }
    }

    IEnumerator ProcessAnimationQueue()
    {
        isProcessingQueue = true;

        while (animationQueue.Count > 0)
        {
            AnimationData currentAnim = animationQueue.Dequeue();
            yield return StartCoroutine(PlayAnimation(currentAnim));
        }

        isProcessingQueue = false;
        frameAnim = null;

        if(activateEndAction)
        {
            EndAction.Invoke();
        }

    }

    IEnumerator PlayAnimation(AnimationData animData)
    {
        interrupted = false;
        bool shouldContinue = true;
        float loopStartTime = Time.time;

        while (shouldContinue)
        {
            yield return new WaitForSeconds(animationDelay);
            
            foreach (Sprite frame in animData.frames)
            {
                spriteImage.sprite = frame;

                if(interrupted)
                {
                    break;
                }
                
                yield return new WaitForSeconds(frameInterval);
            }

            if (!animData.loop || interrupted)
            {
                shouldContinue = false;
            }
            else if (animData.loopDuration.HasValue)
            {
                if (Time.time - loopStartTime >= animData.loopDuration.Value)
                {
                    shouldContinue = false;
                }
            }
            // If loop is true and loopDuration is null, continue indefinitely
        }
    }

    public void QueueAnimation(Sprite[] frames, bool loop = false, float? loopDuration = null)
    {
        animationQueue.Enqueue(new AnimationData(frames, loop, loopDuration));

        if (!isProcessingQueue)
        {
            frameAnim = StartCoroutine(ProcessAnimationQueue());
        }
    }

    public void QueueEnterAnimation()
    {
        QueueAnimation(enterFrames, false);
    }

    public void QueueIdleAnimation(float? duration = null)
    {
        QueueAnimation(idleFrames, true, duration);
    }

    public void QueueExitAnimation()
    {
        QueueAnimation(exitFrames, false);
    }

    public void StartAnimation(Sprite[] frames, bool loop = false, float? loopDuration = null)
    {
        ClearQueue();
        interrupted = true;
        QueueAnimation(frames, loop, loopDuration);
    }

    public void StartEnterAnimation()
    {
        StartAnimation(enterFrames, false);
    }

    public void StartIdleAnimation(float? duration = null)
    {
        StartAnimation(idleFrames, true, duration);
    }

    public void StartExitAnimation()
    {
        StartAnimation(exitFrames, false);
    }

    // Clear the queue and stop current animation
    public void ClearQueue()
    {
        animationQueue.Clear();
        
        if (frameAnim != null)
        {
            StopCoroutine(frameAnim);
            frameAnim = null;
            isProcessingQueue = false;
        }
    }

    public void StartFullAnimation()
    {
        ClearQueue();
        
        if (enterFrames.Length > 0)
        {
            QueueAnimation(enterFrames, false);
        }
        if (idleFrames.Length > 0)
        {
            QueueAnimation(idleFrames, true, idleDuration);
        }
        if (exitFrames.Length > 0)
        {
            QueueAnimation(exitFrames, false);
        }
    }

    public bool IsAnimationGoing()
    {
        return frameAnim != null;
    }

    public void StopALLAnimations()
    {
        ClearQueue();
        interrupted = true;
    }

    public int GetQueuedAnimationCount()
    {
        return animationQueue.Count;
    }

    protected void ResetAnimValues()
    {
        frameInterval = FRAME_INTERVAL;
        idleDuration = IDLE_DURATION;
    }
}

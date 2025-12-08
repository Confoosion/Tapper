using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FrameAnimation : MonoBehaviour
{
    public Image spriteImage;
    private Coroutine frameAnim = null;
    // private bool isLooping = false;
    [SerializeField] private float animationDelay;
    [SerializeField] private float frameInterval;
    [SerializeField] private Sprite startingFrame;

    [SerializeField] private Sprite[] enterFrames;
    [SerializeField] private Sprite[] idleFrames;
    [SerializeField] private float idleDuration;
    [SerializeField] private Sprite[] exitFrames;

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

    void Start()
    {
        // spriteImage = GetComponent<Image>();
        spriteImage.sprite = startingFrame;
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
    }

    IEnumerator PlayAnimation(AnimationData animData)
    {
        yield return new WaitForSeconds(animationDelay);

        bool shouldContinue = true;
        float loopStartTime = Time.time;

        while (shouldContinue)
        {
            foreach (Sprite frame in animData.frames)
            {
                spriteImage.sprite = frame;
                yield return new WaitForSeconds(frameInterval);
            }

            if (!animData.loop)
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

        // spriteImage.sprite = startingFrame;
    }

    // IEnumerator AnimateFrames(Sprite[] cyclingFrames)
    // {
    //     yield return new WaitForSeconds(animationDelay);

    //     while(true)
    //     {
    //         foreach(Sprite frame in cyclingFrames)
    //         {
    //             spriteImage.sprite = frame;
    //             yield return new WaitForSeconds(frameInterval);
    //         }

    //         if(!isLooping)
    //         {
    //             break;
    //         }

    //         spriteImage.sprite = startingFrame;
    //     }

    //     frameAnim = null;
    //     yield return null;
    // }

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

    public int GetQueuedAnimationCount()
    {
        return animationQueue.Count;
    }
}

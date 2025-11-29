using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FrameAnimation : MonoBehaviour
{
    public Image spriteImage;
    private Coroutine frameAnim = null;
    private bool isLooping = false;
    [SerializeField] private float animationDelay;
    [SerializeField] private float frameInterval;
    [SerializeField] private Sprite startingFrame;

    [SerializeField] private Sprite[] enterFrames;
    [SerializeField] private Sprite[] idleFrames;
    [SerializeField] private float idleDuration;
    [SerializeField] private Sprite[] exitFrames;
    private List<Sprite[]> animationQueue = new List<Sprite[]>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteImage = GetComponent<Image>();
        spriteImage.sprite = startingFrame;
    }

    IEnumerator AnimateFrames(Sprite[] cyclingFrames)
    {
        yield return new WaitForSeconds(animationDelay);

        while(true)
        {
            foreach(Sprite frame in cyclingFrames)
            {
                spriteImage.sprite = frame;
                yield return new WaitForSeconds(frameInterval);
            }

            if(!isLooping)
            {
                break;
            }

            spriteImage.sprite = startingFrame;
        }

        frameAnim = null;
        yield return null;
    }

    private void StartAnimation(Sprite[] frames, bool loop)
    {
        if(frameAnim != null)
        {
            StopCoroutine(frameAnim);
            frameAnim = null;
        }
        isLooping = loop;
        if(loop)
            frameAnim = StartCoroutine(AnimateFrames(frames));
    }

    public void StartFullAnimation()
    {
        if(enterFrames.Length > 0)
        {
            animationQueue.Add(enterFrames);
        }
        if(idleFrames.Length > 0)
        {
            animationQueue.Add(idleFrames);
        }
        if(exitFrames.Length > 0)
        {
            animationQueue.Add(exitFrames);
        }

        StartCoroutine(FullAnimation());
    }

    IEnumerator FullAnimation()
    {
        Sprite[] currAnim;

        while(animationQueue.Count > 0)
        {
            currAnim = animationQueue[0];

            if(currAnim == idleFrames)
            {
                isLooping = true;
                StartCoroutine(IdleDuration(idleDuration));
            }

            while(true)
            {
                foreach(Sprite frame in currAnim)
                {
                    spriteImage.sprite = frame;
                    yield return new WaitForSeconds(frameInterval);
                }

                if(!isLooping)
                {
                    break;
                }
            }

            animationQueue.RemoveAt(0);
        }

        frameAnim = null;
        yield return null;
    }

    public void StartEnterAnimation()
    {
        StartAnimation(enterFrames, false);
    }

    public void StartIdleAnimation()
    {
        StartAnimation(idleFrames, true);
    }

    public void StartExitAnimation()
    {
        StartAnimation(exitFrames, false);
    }

    IEnumerator IdleDuration(float time)
    {
        yield return new WaitForSeconds(time);

        isLooping = false;
    }
}

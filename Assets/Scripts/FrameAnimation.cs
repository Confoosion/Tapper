using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FrameAnimation : MonoBehaviour
{
    private Image spriteImage;
    private Coroutine frameAnim = null;
    private bool isLooping = false;
    [SerializeField] private float animationDelay;
    [SerializeField] private float frameInterval;
    [SerializeField] private Sprite startingFrame;

    [SerializeField] private Sprite[] enterFrames;
    [SerializeField] private Sprite[] idleFrames;
    [SerializeField] private Sprite[] exitFrames;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteImage = GetComponent<Image>();
        spriteImage.sprite = startingFrame;
    }

    IEnumerator AnimateFrames(Sprite[] cyclingFrames)
    {
        while(true)
        {
            yield return new WaitForSeconds(animationDelay);

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

    // void OnEnable()
    // {
    //     StartAnimation(true);
    // }

    // void OnDisable()
    // {
    //     StartAnimation(false);
    // }
}

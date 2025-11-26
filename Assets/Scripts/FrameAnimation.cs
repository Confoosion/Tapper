using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FrameAnimation : MonoBehaviour
{
    private Image spriteImage;
    private bool isLooping = false;
    [SerializeField] private float animationDelay;
    [SerializeField] private float frameInterval;
    [SerializeField] private Sprite startingFrame;
    [SerializeField] private Sprite[] cyclingFrames;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteImage = GetComponent<Image>();
        spriteImage.sprite = startingFrame;
    }

    IEnumerator AnimateFrames()
    {
        while(isLooping)
        {
            yield return new WaitForSeconds(animationDelay);

            foreach(Sprite frame in cyclingFrames)
            {
                spriteImage.sprite = frame;
                yield return new WaitForSeconds(frameInterval);
            }

            spriteImage.sprite = startingFrame;
        }

        yield return null;
    }

    public void StartAnimation(bool loop)
    {
        isLooping = loop;
        if(loop)
            StartCoroutine(AnimateFrames());
    }

    void OnEnable()
    {
        StartAnimation(true);
    }

    void OnDisable()
    {
        StartAnimation(false);
    }
}

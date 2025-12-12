using UnityEngine;

public class TargetAnimation : FrameAnimation
{
    [SerializeField] private Sprite[] hitFrames;

    public void ChangeAnimationTiming(float scale)
    {
        frameInterval /= scale;
        idleDuration /= scale;
    }

    public void StartHitAnimation()
    {
        activateEndAction = true;
        StartAnimation(hitFrames, false);
    }

    public void QueueHitAnimation()
    {
        activateEndAction = true;
        QueueAnimation(hitFrames, false);
    }

    public float GetHitTime()
    {
        // Debug.Log(idleDuration + (float)enterFrames.Length * 0.01f);
        return(idleDuration + (float)enterFrames.Length * 0.01f);
    }
}

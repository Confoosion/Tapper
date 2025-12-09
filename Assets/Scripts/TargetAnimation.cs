using UnityEngine;

public class TargetAnimation : FrameAnimation
{
    [SerializeField] private Sprite[] hitFrames;
    
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
}

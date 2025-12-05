using UnityEngine;

public class TargetAnimation : FrameAnimation
{
    [SerializeField] private Sprite[] hitFrames;
    
    public void StartHitAnimation()
    {
        StartAnimation(hitFrames, false);
    }
}

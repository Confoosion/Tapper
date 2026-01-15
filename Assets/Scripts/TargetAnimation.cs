using UnityEngine;

public class TargetAnimation : FrameAnimation
{
    [SerializeField] private Sprite[] hitFrames;

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
        StartAnimation(hitFrames, false);
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

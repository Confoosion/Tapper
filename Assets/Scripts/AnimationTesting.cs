using UnityEngine;

public class AnimationTesting : MonoBehaviour
{
    // [SerializeField] private IdleAnimation idleAnimation;
    [SerializeField] private FrameAnimation frameAnimation;
    
    void Start()
    {
        frameAnimation.StartFullAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

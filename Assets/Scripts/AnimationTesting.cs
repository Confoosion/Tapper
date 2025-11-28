using UnityEngine;

public class AnimationTesting : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.gameObject.GetComponent<FrameAnimation>().StartAnimation(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

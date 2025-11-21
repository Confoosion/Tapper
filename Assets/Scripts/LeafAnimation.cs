using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LeafAnimation : MonoBehaviour
{
    [SerializeField] private float animationTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DoAnimation());
    }

    IEnumerator DoAnimation()
    {
        LeanTween.move(this.gameObject, UIManager.Singleton.GetLeafCounterTransform().position, animationTime).setEase(LeanTweenType.easeOutQuad);

        yield return new WaitForSeconds(animationTime);

        UIManager.Singleton.SetLeafUI(GameManager.Singleton.GetLeafAmount());

        Destroy(this.gameObject);

        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

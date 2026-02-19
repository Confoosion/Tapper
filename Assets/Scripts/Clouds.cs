using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Clouds : MonoBehaviour
{
    [SerializeField] private Vector2 startPos;
    [SerializeField] private Vector2 endPos; 
    [SerializeField] private float travelTime;
    [SerializeField] private float delayTime;

    private Coroutine loop;

    void Awake()
    {
        transform.localPosition = startPos;
    }

    void OnEnable()
    {
        loop = StartCoroutine(AnimateClouds());
    }

    IEnumerator AnimateClouds()
    {
        yield return new WaitForSeconds(delayTime);
        
        while(true)
        {
            transform.localPosition = startPos;
            LeanTween.moveLocal(this.gameObject, endPos, travelTime);
            yield return new WaitForSeconds(delayTime + travelTime);
        }
    }

    void OnDisable()
    {
        StopCoroutine(loop);
        loop = null;
    }
}

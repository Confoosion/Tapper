using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CircleFlickBehavior : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Vector2 targetPosition;
    [SerializeField] private float duration;
    [SerializeField] private GameObject gem;

    private Coroutine moving;

    private SoundType soundType;

    void Awake()
    {
        soundType = SoundType.Good;
    }

    void Update()
    {
        if (!GameManager.Singleton.CheckPlayerStatus())
        {
            Destroy(this.gameObject);
        }
    }

    public void SetupBehavior()
    {
        moving = StartCoroutine(MoveToTarget());
    }

    IEnumerator MoveToTarget()
    {
        float xPos = this.transform.position.x;
        targetPosition = new Vector2(xPos, 1000f);
        LeanTween.moveLocal(this.gameObject, targetPosition, duration);

        bool isAlive = true;

        while (isAlive)
        {
            yield return new WaitForSeconds(duration);
            isAlive = false;
        }
        Destroy(this.gameObject);
        GameManager.Singleton.RemoveLives(1);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.Singleton.isPlaying)
        {
            // if (taps == 1)
            // {
            //     ScoreManager.Singleton.AddPoints(1);
            //     if (ScoreManager.Singleton.AddCircleTapped())
            //     {
            //         Instantiate(gem, this.transform.position, Quaternion.identity, SpawnManager.Singleton.spawnArea);
            //     }
            //     moving = null;
            //     Destroy(this.gameObject);
            // }
            // else
            // {
            //     taps--;
            //     tapAmount.SetText(taps.ToString());
            // }
            // SoundManager.Singleton.PlaySound(soundType);
        }
    }
}

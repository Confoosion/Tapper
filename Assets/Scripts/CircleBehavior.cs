using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class CircleBehavior : MonoBehaviour, IPointerDownHandler
{
    public bool isGood = true;
    [SerializeField] private float timeOnScreen = 1f;
    [SerializeField] private SoundType soundType;

    void Awake()
    {
        if (isGood)
        {
            soundType = SoundType.Good;
        }
        else
        {
            soundType = SoundType.Bad;
        }
    }

    void Update()
    {
        if (timeOnScreen > 0f)
        {
            timeOnScreen -= Time.deltaTime;
        }
        else
        {
            if (isGood)
            {
                GameManager.Singleton.RemoveLives(1);
            }
            else
            {
                ScoreManager.Singleton.AddPoints(1);
            }

            Destroy(this.gameObject);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.Singleton.isPlaying && timeOnScreen > 0f)
        {
            if (isGood)
            {
                ScoreManager.Singleton.AddPoints(1);
            }
            else
            {
                GameManager.Singleton.RemoveLives(1);
            }
            SoundManager.Singleton.PlaySound(soundType);
            Destroy(this.gameObject);
        }
    }
}

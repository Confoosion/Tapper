using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class CircleBehavior : MonoBehaviour, IPointerDownHandler
{
    public bool isGood = true;
    [SerializeField] private float timeOnScreen = 1f;
    [SerializeField] private SoundType soundType;
    [SerializeField] private GameObject gem;

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
                if (ScoreManager.Singleton.AddCircleTapped())
                {
                    Instantiate(gem, this.transform.position, Quaternion.identity, SpawnManager.Singleton.spawnArea);
                }
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

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class RainCircleBehavior : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private int tapAmount;
    [SerializeField] private TextMeshProUGUI tapUI;
    [SerializeField] private SoundType soundType;
    [SerializeField] private GameObject gem;

    void Awake()
    {
        soundType = SoundType.Good;
    }

    void Update()
    {
        // if (timeOnScreen > 0f)
        // {
        //     timeOnScreen -= Time.deltaTime;
        // }
        // else
        // {
        //     if (isGood)
        //     {
        //         GameManager.Singleton.RemoveLives(1);
        //     }
        //     else
        //     {
        //         ScoreManager.Singleton.AddPoints(1);
        //     }

        //     Destroy(this.gameObject);
        // }
    }

    public void UpdateTapAmount(int amount)
    {
        tapAmount = amount;
        tapUI.SetText(tapAmount.ToString());
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.Singleton.isPlaying)
        {
            if (tapAmount == 1)
            {
                ScoreManager.Singleton.AddPoints(1);
                if (ScoreManager.Singleton.AddCircleTapped())
                {
                    Instantiate(gem, this.transform.position, Quaternion.identity, SpawnManager.Singleton.spawnArea);
                }
                Destroy(this.gameObject);
            }
            else
            {
                tapAmount--;
                UpdateTapAmount(tapAmount);

            }
            SoundManager.Singleton.PlaySound(soundType);
        }
    }
}

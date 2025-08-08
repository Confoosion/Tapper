using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CircleRainBehavior : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Vector2 targetPosition;
    [SerializeField] private int taps;
    [SerializeField] private TextMeshProUGUI tapAmount;

    [SerializeField] private float duration;
    [SerializeField] private GameObject gem;

    private SoundType soundType;

    void Awake()
    {
        soundType = SoundType.Good;
    }

    void Update()
    {
        
    }

    public void SetupBehavior(int min, int max, float time)
    {
        SetTapAmount(min, max);
        SetDuration(time);
        SetTarget();
    }

    public void SetTapAmount(int min, int max)
    {
        int randomTap = Random.Range(min, max + 1);
        taps = randomTap;
        tapAmount.SetText(taps.ToString());
    }

    public void SetDuration(float time)
    {
        duration = time;
    }

    public void SetTarget()
    {
        float randomX = Random.Range(-500f, 500f);
        targetPosition = new Vector2(randomX, -1775f);
        LeanTween.moveLocal(this.gameObject, targetPosition, duration);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.Singleton.isPlaying)
        {
            if (taps == 1)
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
                taps--;
                tapAmount.SetText(taps.ToString());
            }
            SoundManager.Singleton.PlaySound(soundType);
        }
    }
}

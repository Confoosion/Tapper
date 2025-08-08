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

    public void SetupBehavior(int min, int max)
    {
        SetTapAmount(min, max);
        moving = StartCoroutine(MoveToTarget());
    }

    public void SetTapAmount(int min, int max)
    {
        int randomTap = Random.Range(min, max + 1);
        taps = randomTap;
        tapAmount.SetText(taps.ToString());
    }

    IEnumerator MoveToTarget()
    {
        float randomX = Random.Range(-400f, 400f);
        targetPosition = new Vector2(randomX, -1775f);
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
            if (taps == 1)
            {
                ScoreManager.Singleton.AddPoints(1);
                if (ScoreManager.Singleton.AddCircleTapped())
                {
                    Instantiate(gem, this.transform.position, Quaternion.identity, SpawnManager.Singleton.spawnArea);
                }
                moving = null;
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

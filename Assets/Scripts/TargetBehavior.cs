using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TargetBehavior : MonoBehaviour, IPointerDownHandler
{
    public Sprite thumbnail;
    public TargetType targetType;
    [SerializeField] private int addTime;
    [SerializeField] private GameObject gem;
    [SerializeField] private TargetAnimation targetAnimation;
    [SerializeField] private bool tapped = false;
    [SerializeField] private bool canTap = false;
    [SerializeField] private float hitWindow;
    private float leewayTiming = 0.1f;

    [SerializeField] private bool checkStarted = false;

    void OnEnable()
    {
        StartBehavior();
    }

    public void CheckStart()
    {
        if(!checkStarted)
        {
            Debug.Log("OnEnable did not work! " + this.gameObject);

            StartBehavior();
        }
    }

    private void StartBehavior()
    {
        checkStarted = true;
        tapped = false;
        canTap = false;
        
        hitWindow = targetAnimation.GetHitTime() + leewayTiming;

        targetAnimation.StartFullAnimation();

        StartCoroutine(BeginHitWindow());
        canTap = true;
    }

    IEnumerator BeginHitWindow()
    {
        yield return new WaitForSeconds(hitWindow);

        if(!tapped)
        {
            canTap = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.Singleton.isPlaying && !tapped && canTap)
        {
            tapped = true;

            StopCoroutine(BeginHitWindow());

            if(SettingsManager.Singleton.CheckVibrations())
                Handheld.Vibrate();

            if(addTime != 0 && GameManager.Singleton.currentGameMode.isTimed)
            {
                TimeAttackClock.Singleton.AddTime(addTime);
            }

            if (targetType != TargetType.Bad)
            {
                SoundManager.Singleton.PlayHitSound();
                ThemeManager.Singleton.PlayTapEffect(transform.position);

                ScoreManager.Singleton.AddPoints(1);
                if (ScoreManager.Singleton.AddCircleTapped())
                {
                    GameManager.Singleton.AddLeaf();
                    Instantiate(gem, this.transform.position, Quaternion.identity, SpawnManager.Singleton.spawnArea);
                }
            }
            else
            {
                SoundManager.Singleton.PlayBadSound();
                LoseLife(1);
            }

            targetAnimation.StartHitAnimation();
        }
    }

    public void Finish()
    {
        if (!tapped && targetType != TargetType.Bad && GameManager.Singleton.isPlaying)
        {
            SoundManager.Singleton.PlayTargetSound(targetType);

            LoseLife(1);
        }
        
        transform.position = SpawnManager.Singleton.despawnLocation;
        SpawnManager.Singleton.RemoveTarget(GetComponent<RectTransform>());
    }

    void LoseLife(int damage)
    {
        if(GameManager.Singleton.currentGameMode.modeName != "Timed")
        {
            GameManager.Singleton.RemoveLives(damage);
            ArcadeEnding.Singleton.SetReason(thumbnail, targetType);
        }
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TargetBehavior : MonoBehaviour, IPointerDownHandler
{
    public Sprite thumbnail;
    public bool isGood = true;
    [SerializeField] private int addTime;
    [SerializeField] private AudioClip laughAudio;
    [SerializeField] private GameObject gem;
    [SerializeField] private Image tapImage;
    [SerializeField] private TargetAnimation targetAnimation;
    [SerializeField] private bool tapped = false;
    [SerializeField] private bool canTap = false;
    [SerializeField] private float hitWindow;
    private float leewayTiming = 0.25f;

    void OnEnable()
    {
        tapped = false;
        canTap = false;
        // tapImage.gameObject.SetActive(false);

        targetAnimation.ChangeAnimationTiming(GameManager.Singleton.GetGameDifficulty());
        hitWindow = targetAnimation.GetHitTime() + leewayTiming;

        // targetAnimation.StartFullAnimation();
        // StartCoroutine(BeginHitWindow());
    }

    public void ResetValues()
    {
        // tapped = false;
        canTap = true;
        tapImage.gameObject.SetActive(false);

        targetAnimation.StartFullAnimation();
        StartCoroutine(BeginHitWindow());
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
            tapImage.gameObject.SetActive(true);

            StopCoroutine(BeginHitWindow());

            if(SettingsManager.Singleton.CheckVibrations())
                Handheld.Vibrate();

            if(addTime != 0 && GameManager.Singleton.currentGameMode.isTimed)
            {
                TimeAttackClock.Singleton.AddTime(addTime);
            }

            if (isGood)
            {
                SoundManager.Singleton.PlayHitSound();
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
        if (!tapped && isGood && GameManager.Singleton.isPlaying)
        {
            SoundManager.Singleton.PlaySound(laughAudio);
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
            ArcadeEnding.Singleton.SetReason(thumbnail, laughAudio);
        }
    }
}

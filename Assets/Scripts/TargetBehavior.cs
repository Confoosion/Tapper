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
    // [SerializeField] private float timeOnScreen = 2f;
    [SerializeField] private AudioClip sfx;
    [SerializeField] private GameObject gem;
    [SerializeField] private Image tapImage;
    [SerializeField] private TargetAnimation targetAnimation;
    [SerializeField] private bool tapped = false;
    // private Coroutine run;

    public void OnEnable()
    {
        tapped = false;
        tapImage.gameObject.SetActive(false);
        targetAnimation.StartFullAnimation();
        // run = StartCoroutine(StayOnScreen());
    }

    // IEnumerator StayOnScreen()
    // {
    //     yield return new WaitForSeconds(timeOnScreen);
    // }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.Singleton.isPlaying && !tapped)
        {
            tapped = true;
            tapImage.gameObject.SetActive(true);
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
                SoundManager.Singleton.PlaySound(sfx);
                LoseLife(1);
            }
            // SoundManager.Singleton.PlaySound(sfx);
            // spriteCycler.AnimateHit();
            targetAnimation.StartHitAnimation();
        }
    }

    // void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (run == null)
    //     {
    //         Debug.Log("Object colliding");
    //         // validLocation = false;
    //     }
    // }

    // void OnTriggerExit2D(Collider2D collision)
    // {
    //     if (run == null)
    //     {
    //         Debug.Log("Object NOT colliding");
    //         // validLocation = true;
    //     }
    // }

    public void Finish()
    {
        if (!tapped && isGood && GameManager.Singleton.isPlaying)
        {
            LoseLife(1);
        }
        
        transform.position = SpawnManager.Singleton.despawnLocation;
        SpawnManager.Singleton.RemoveTarget(GetComponent<RectTransform>());
    }

    // void OnDisable()
    // {
    //     if (!tapped && isGood && GameManager.Singleton.isPlaying)
    //     {
    //         LoseLife(1);
    //     }
        
    //     if(initialized)
    //     {
    //         // transform.position = SpawnManager.Singleton.despawnLocation;
    //         SpawnManager.Singleton.RemoveTarget(GetComponent<RectTransform>());
    //     }
    //     else
    //     {
    //         initialized = true;
    //     }
    // }

    void LoseLife(int damage)
    {
        // GameManager.Singleton.RemoveLives(damage);

        if(GameManager.Singleton.currentGameMode.modeName != "Timed")
        {
            GameManager.Singleton.RemoveLives(damage);
            ArcadeEnding.Singleton.SetReason(thumbnail);
        }
    }
}

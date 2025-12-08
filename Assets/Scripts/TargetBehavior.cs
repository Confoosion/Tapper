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
    [SerializeField] private float timeOnScreen = 2f;
    [SerializeField] private AudioClip sfx;
    [SerializeField] private GameObject gem;
    [SerializeField] private Image tapImage;
    [SerializeField] private TargetAnimation targetAnimation;
    [SerializeField] private bool tapped = false;
    // [SerializeField] private bool validLocation = true;
    private Coroutine run;

    public void OnEnable()
    {
        // spriteCycler.AnimateIn();
        Debug.Log("Enabled");
        // targetAnimation.StartEnterAnimation();
        // targetAnimation.QueueIdleAnimation();
        targetAnimation.StartFullAnimation();
        run = StartCoroutine(StayOnScreen());
    }

    IEnumerator StayOnScreen()
    {
        yield return new WaitForSeconds(timeOnScreen);
        // if (!tapped)
        // {
        //     // spriteCycler.AnimateOut();
        //     targetAnimation.StartExitAnimation();
        // }
    }

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
                ScoreManager.Singleton.AddPoints(1);
                if (ScoreManager.Singleton.AddCircleTapped())
                {
                    GameManager.Singleton.AddLeaf();
                    Instantiate(gem, this.transform.position, Quaternion.identity, SpawnManager.Singleton.spawnArea);
                }
            }
            else
            {
                LoseLife(1);
            }
            SoundManager.Singleton.PlaySound(sfx);
            // spriteCycler.AnimateHit();
            targetAnimation.StartHitAnimation();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (run == null)
        {
            Debug.Log("Object colliding");
            // validLocation = false;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (run == null)
        {
            Debug.Log("Object NOT colliding");
            // validLocation = true;
        }
    }

    void OnDestroy()
    {
        if (!tapped && isGood && GameManager.Singleton.isPlaying)
        {
            LoseLife(1);
        }
        SpawnManager.Singleton.RemoveTarget(GetComponent<RectTransform>());
    }

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

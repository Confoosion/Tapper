using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TargetBehavior : MonoBehaviour, IPointerDownHandler
{
    public bool isGood = true;
    [SerializeField] private float timeOnScreen = 2f;
    [SerializeField] private AudioClip audio;
    [SerializeField] private GameObject gem;
    [SerializeField] private Image tapImage;
    [SerializeField] private SpriteCycler spriteCycler;

    // public void OnEnable()
    // {
    //     spriteCycler.AnimateIn();
    //     StartCoroutine(StayOnScreen());
    // }

    IEnumerator StayOnScreen()
    {
        yield return new WaitForSeconds(timeOnScreen);
        if (spriteCycler.canTap)
        {
            spriteCycler.AnimateOut();
        }
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (timeOnScreen > 0f && spriteCycler.canTap)
        {
            spriteCycler.canTap = false;
            tapImage.gameObject.SetActive(true);
            spriteCycler.AnimateHit();
            // if (isGood)
            // {
            //     ScoreManager.Singleton.AddPoints(1);
            //     if (ScoreManager.Singleton.AddCircleTapped())
            //     {
            //         Instantiate(gem, this.transform.position, Quaternion.identity, SpawnManager.Singleton.spawnArea);
            //     }
            // }
            // else
            // {
            //     GameManager.Singleton.RemoveLives(1);
            // }
            // SoundManager.Singleton.PlaySound(soundType);
            // Destroy(this.gameObject);
        }
    }
}

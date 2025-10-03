using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TargetBehavior : MonoBehaviour, IPointerDownHandler
{
    public bool isGood = true;
    [SerializeField] private float timeOnScreen = 2f;
    [SerializeField] private AudioClip sfx;
    [SerializeField] private GameObject gem;
    [SerializeField] private Image tapImage;
    [SerializeField] private SpriteCycler spriteCycler;
    [SerializeField] private bool tapped = false;
    // [SerializeField] private bool validLocation = true;
    private Coroutine run;

    public void OnEnable()
    {
        spriteCycler.AnimateIn();
        run = StartCoroutine(StayOnScreen());
    }

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
        if (GameManager.Singleton.isPlaying && spriteCycler.canTap)
        {
            spriteCycler.canTap = false;
            tapped = true;
            tapImage.gameObject.SetActive(true);

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
            SoundManager.Singleton.PlaySound(sfx);
            spriteCycler.AnimateHit();
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
            GameManager.Singleton.RemoveLives(1);
        }
        SpawnManager.Singleton.RemoveTarget(GetComponent<RectTransform>());
    }
}

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class MoleStartButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject moleObject;
    [SerializeField] private Image tapImage;
    [SerializeField] private TargetAnimation targetAnimation;
    private float delay = 1.5f;

    void Start()
    {
        StartCoroutine(MoleAnimation());
    }

    IEnumerator MoleAnimation()
    {
        yield return new WaitForSeconds(delay);
        moleObject.SetActive(true);
        targetAnimation.StartEnterAnimation();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!targetAnimation.IsAnimationGoing() && !ScreenManager.Singleton.IsTransitionGoing())
        {
            UIManager.Singleton.HideSettingsAndPauseIcon();
        
            tapImage.gameObject.SetActive(true);

            SoundManager.Singleton.PlayHitSound();
            targetAnimation.QueueHitAnimation();
        }
    }

    public void Finish()
    {
        gameObject.SetActive(false);
        if(ScreenManager.Singleton != null)
            ScreenManager.Singleton.BeginStartScreen(false);
        
    }

    // void OnDisable()
    // {
    //     if(ScreenManager.Singleton != null)
    //         ScreenManager.Singleton.BeginStartScreen(false);
    // }
}

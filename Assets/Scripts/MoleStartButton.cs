using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class MoleStartButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image moleImage;
    [SerializeField] private Image tapImage;
    public TargetAnimation targetAnimation;

    [SerializeField] private GameObject tapStart_GO;

    private float delay = 1.5f;
    // private float waitTapTime = 0.3f;

    void Awake()
    {
        tapStart_GO.SetActive(false);
    }

    void Start()
    {
        StartCoroutine(MoleAnimation());
    }

    IEnumerator MoleAnimation()
    {
        yield return new WaitForSeconds(delay);
        var animalSet = ThemeManager.Singleton.GetAnimalSet();
        targetAnimation.SetStartingFrames(animalSet.start_EnterFrame, animalSet.start_HitFrame);
        moleImage.enabled = true;
        targetAnimation.StartEnterAnimation();

        yield return new WaitForSeconds(0.25f);
        tapStart_GO.SetActive(true);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!ScreenManager.Singleton.IsTransitionGoing())
        {
            UIManager.Singleton.HideSettingsAndPauseIcon();
        
            tapImage.gameObject.SetActive(true);

            SoundManager.Singleton.PlayHitSound();
            targetAnimation.StartHitAnimation();
            // targetAnimation.StartHitAnimation();
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

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TimeAttackEnding : MonoBehaviour
{
    public static TimeAttackEnding Singleton;
    [SerializeField] private GameObject clockObject;
    [SerializeField] private Vector3 awayPosition;
    [SerializeField] private Vector3 dropPosition;
    [SerializeField] private float dropTime = 1.5f;

    void Awake()
    {
        if(Singleton == null)
        {
            Singleton = this;
        }
    }

    void Start()
    {
        HideClock();
    }

    private void HideClock()
    {
        clockObject.GetComponent<ClockVibrate>().canVibrate = false;
        clockObject.transform.localPosition = awayPosition;
        clockObject.SetActive(false);
    }

    public void StartEndingAnimation()
    {
        UIManager.Singleton.HideSettingsAndPauseIcon();
        StartCoroutine(EndingAnimation());
    }
    
    IEnumerator EndingAnimation()
    {
        clockObject.SetActive(true);
        clockObject.GetComponent<ClockVibrate>().canVibrate = true;
        LeanTween.moveLocal(clockObject, dropPosition, dropTime).setEase(LeanTweenType.easeOutBounce);

        yield return new WaitForSeconds(2.5f);
        ScreenManager.Singleton.BeginMajorTransition_NOAUDIO(ScreenManager.Singleton.GetEndScreen());

        yield return new WaitForSeconds(0.75f);
        HideClock();
    }
}

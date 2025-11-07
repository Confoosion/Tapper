using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ArcadeEnding : MonoBehaviour
{
    public static ArcadeEnding Singleton;

    [SerializeField] private GameObject[] lossReasons = new GameObject[3];
    private Vector3 homeLocation = new Vector3(0f, 1600f, 0f);
    private Vector3 dropLocation = new Vector3(0f, -550f, 0f);
    private Vector3 dropOffset = new Vector3(0f, 500f, 0f);
    private float dropTime = 1.5f;
    private float waitTime = 0.5f;

    void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
    }

    void Start()
    {
        ResetReasons();
    }

    public void SetReason(int livesLeft, Sprite reason)
    {
        lossReasons[livesLeft].GetComponent<Image>().sprite = reason;
        lossReasons[livesLeft].SetActive(true);
    }

    void ResetReasons()
    {
        for(int i = lossReasons.Length - 1; i >= 0; i--)
        {
            GameObject reason = lossReasons[i];
            if(reason != null)
            {
                reason.transform.localPosition = homeLocation + dropOffset * (lossReasons.Length - 1 - i);
                reason.SetActive(false);
            }
        }
    }

    public void StartEndingAnimation()
    {
        UIManager.Singleton.HideSettingsAndPauseIcon();
        StartCoroutine(EndingAnimation());
    }
    
    IEnumerator EndingAnimation()
    {
        for (int i = lossReasons.Length - 1; i >= 0; i--)
        {
            GameObject reason = lossReasons[i];
            if(reason != null)
                LeanTween.moveLocal(reason, dropLocation + dropOffset * (lossReasons.Length - 1 - i), dropTime).setEase(LeanTweenType.easeOutBounce);

            yield return new WaitForSeconds(waitTime);
        }

        yield return new WaitForSeconds(2f);
        ScreenManager.Singleton.BeginMajorTransition(ScreenManager.Singleton.GetEndScreen());

        yield return new WaitForSeconds(0.75f);
        ResetReasons();

    }
}

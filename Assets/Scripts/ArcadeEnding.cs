using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ArcadeEnding : MonoBehaviour
{
    public static ArcadeEnding Singleton;

    [SerializeField] private GameObject[] lossReasons = new GameObject[3];
    private AudioClip[] lossLaughs = new AudioClip[3];
    private int reasonCount = 0;
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

    public void SetReason(Sprite reason, AudioClip laugh)
    {
        lossReasons[reasonCount].GetComponent<Image>().sprite = reason;
        lossReasons[reasonCount].SetActive(true);
        lossLaughs[reasonCount] = laugh;
        reasonCount += 1;
    }

    public void ResetReasons()
    {
        for(int i = 0; i < lossReasons.Length; i++)
        {
            GameObject reason = lossReasons[i];
            if(reason != null)
            {
                reason.transform.localPosition = homeLocation + dropOffset * i;
                reason.SetActive(false);
                lossLaughs[i] = null;
            }
        }
        reasonCount = 0;
    }

    public void StartEndingAnimation()
    {
        UIManager.Singleton.HideSettingsAndPauseIcon();
        StartCoroutine(EndingAnimation());
    }
    
    IEnumerator EndingAnimation()
    {
        for (int i = 0; i < lossReasons.Length; i++)
        {
            GameObject reason = lossReasons[i];
            if(reason != null)
            {
                LeanTween.moveLocal(reason, dropLocation + dropOffset * i, dropTime).setEase(LeanTweenType.easeOutBounce);

                yield return new WaitForSeconds(0.5f);

                SoundManager.Singleton.PlaySound(lossLaughs[i]);
            }

            yield return new WaitForSeconds(waitTime);
        }

        yield return new WaitForSeconds(1f);
        ScreenManager.Singleton.BeginMajorTransition_NOAUDIO(ScreenManager.Singleton.GetEndScreen());

        yield return new WaitForSeconds(0.75f);
        ResetReasons();

    }
}

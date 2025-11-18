using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class MoleStartButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image tapImage;
    [SerializeField] private AudioClip sfx;
    [SerializeField] private SpriteCycler spriteCycler;
    private float delay = 1.5f;

    void Start()
    {
        StartCoroutine(MoleAnimation());
    }

    IEnumerator MoleAnimation()
    {
        yield return new WaitForSeconds(delay);
        transform.GetChild(1).gameObject.SetActive(true);
        spriteCycler.AnimateIn();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (spriteCycler.canTap && !ScreenManager.Singleton.IsTransitionGoing())
        {
            UIManager.Singleton.HideSettingsAndPauseIcon();
            spriteCycler.canTap = false;
            tapImage.gameObject.SetActive(true);

            SoundManager.Singleton.PlaySound(sfx);
            spriteCycler.AnimateHit();
        }
    }

    void OnDestroy()
    {
        if(ScreenManager.Singleton != null)
            ScreenManager.Singleton.BeginStartScreen(false);
    }
}

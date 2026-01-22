using UnityEngine;
using UnityEngine.EventSystems;

public class TapStart : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] MoleStartButton moleStartButton;
    [SerializeField] AudioClip laughAudio;


    public void OnPointerDown(PointerEventData eventData)
    {
        if (gameObject.activeSelf)
        {
            UIManager.Singleton.HideSettingsAndPauseIcon();
        
            moleStartButton.targetAnimation.QueueExitAnimation();

            SoundManager.Singleton.PlaySound(laughAudio);

            gameObject.SetActive(false);
        }
    }
}

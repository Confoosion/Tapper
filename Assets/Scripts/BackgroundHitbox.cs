using UnityEngine;
using UnityEngine.EventSystems;

public class BackgroundHitbox : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.Singleton.isPlaying && GameManager.Singleton.GetGameMode() == GameMode.Classic)
        {
            ScoreManager.Singleton.AddPoints(-1);
        }
    }
}

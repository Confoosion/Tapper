using UnityEngine;
using UnityEngine.EventSystems;

public class BackgroundHitbox : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.Singleton.isPlaying)
        {
            ScoreManager.Singleton.AddPoints(-1);
        }
    }
}

using UnityEngine;
using UnityEngine.EventSystems;

public class ShopParticle : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        ShopManager.Singleton.PreviewTapEffect();
    }
}

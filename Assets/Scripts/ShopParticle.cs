using UnityEngine;
using UnityEngine.EventSystems;

public class ShopParticle : MonoBehaviour, IPointerDownHandler
{
    public ParticleSystem particleSystem;

    public void OnPointerDown(PointerEventData eventData)
    {
        // particleSystem.Play();
        ShopManager.Singleton.PreviewTapEffect();
    }
}

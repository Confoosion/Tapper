using UnityEngine;
using UnityEngine.EventSystems;

public class ShopParticle : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private ParticleSystem particleSystem;

    public void OnPointerDown(PointerEventData eventData)
    {
        particleSystem.Play();
    }
}

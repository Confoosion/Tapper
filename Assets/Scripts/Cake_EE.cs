using UnityEngine;
using UnityEngine.EventSystems;

public class Cake_EE : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private AudioClip cakeSound;
    [SerializeField] private ObjectBounceVFX bounceVFX;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (gameObject.activeSelf)
        {
            bounceVFX.Bounce();

            SoundManager.Singleton.PlaySoundWithRandomPitch(cakeSound, 1f, 2f);
        }
    }
}

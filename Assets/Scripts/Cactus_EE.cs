using UnityEngine;
using UnityEngine.EventSystems;

public class Cactus_EE : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private float animTime;
    [SerializeField] private ParticleSystem tumbleweed_PS;

    private RectTransform rectTransform;
    private Vector3 originalScale;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if(rectTransform != null)
            originalScale = rectTransform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (gameObject.activeSelf)
        {
            rectTransform.localScale = originalScale * 0.75f;
            LeanTween.scale(this.gameObject, originalScale, animTime).setEase(LeanTweenType.easeOutElastic);

            tumbleweed_PS.Emit(1);
        }
    }
}

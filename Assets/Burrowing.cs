using UnityEngine;
using UnityEngine.EventSystems;

public class Burrowing : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    [SerializeField] private float moveSpeed = 100f;
    [SerializeField] private float momentumDecay = 5f;
    private bool isDragging = false;
    private float momentum = 0f;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (!isDragging)
        {
            rectTransform.anchoredPosition += Vector2.up * moveSpeed * Time.deltaTime;
        }

        // Apply momentum after flick
        if (Mathf.Abs(momentum) > 0.01f)
        {
            rectTransform.anchoredPosition += new Vector2(momentum, 0f) * Time.deltaTime;

            // Gradually reduce momentum
            momentum = Mathf.Lerp(momentum, 0f, momentumDecay * Time.deltaTime);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        momentum = 0f;
        Debug.Log("Drag started!");
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += new Vector2(eventData.delta.x, 0f);
        momentum = eventData.delta.x / Time.deltaTime;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        Debug.Log("Drag ended!");
    }
}

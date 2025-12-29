using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class SeedSlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image seedImage;
    [SerializeField] private TextMeshProUGUI quantityText;
    [SerializeField] private GameObject seedSelectionMenu;
    
    private Canvas canvas;
    private GameObject dragIcon;
    private bool isDragging = false;
    
    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        UpdateDisplay();
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isDragging)
        {
            seedSelectionMenu.SetActive(true);
        }
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        SeedData current = SeedInventory.Instance.GetCurrentSeed();
        if (current != null && current.quantity > 0)
        {
            isDragging = true;
            
            dragIcon = new GameObject("DragIcon");
            dragIcon.transform.SetParent(canvas.transform);
            dragIcon.transform.SetAsLastSibling();
            
            Image img = dragIcon.AddComponent<Image>();
            img.sprite = current.seedSprite;
            img.raycastTarget = false;
            
            RectTransform rt = dragIcon.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(50, 50);
            
            UpdateDragPosition(eventData);
        }
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging && dragIcon != null)
        {
            UpdateDragPosition(eventData);
        }
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragIcon != null)
        {
            Destroy(dragIcon);
        }
        
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        
        foreach (RaycastResult result in results)
        {
            FarmlandTile tile = result.gameObject.GetComponent<FarmlandTile>();
            if (tile != null)
            {
                tile.PlantSeed();
                break;
            }
        }
        
        isDragging = false;
    }
    
    private void UpdateDragPosition(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out pos
        );
        dragIcon.transform.position = canvas.transform.TransformPoint(pos);
    }
    
    public void UpdateDisplay()
    {
        SeedData current = SeedInventory.Instance.GetCurrentSeed();
        if (current != null)
        {
            seedImage.sprite = current.seedSprite;
            seedImage.enabled = true;
            quantityText.text = current.quantity.ToString();
        }
        else
        {
            seedImage.enabled = false;
            quantityText.text = "0";
        }
    }
}
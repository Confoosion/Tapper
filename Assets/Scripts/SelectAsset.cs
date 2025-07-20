using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class SelectAsset : MonoBehaviour, IPointerClickHandler
{
    private GameObject selectedHighlight;
    public GameThemes.CosmeticType cosmetic;

    void Start()
    {
        selectedHighlight = transform.GetChild(0).gameObject;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameThemes.Singleton.SwitchThemes(cosmetic, selectedHighlight);
    }
}

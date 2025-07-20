using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerTap : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && GameManager.Singleton.isPlaying)
        {
            if (!ClickedUI())
            {
                GameManager.Singleton.AddPoints(-1);
                GameManager.Singleton.RemoveLives(1);
                Debug.Log("Clicked on nothing!");
            }
        }
    }

    private bool ClickedUI()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        return (results.Count > 0);
    }
}

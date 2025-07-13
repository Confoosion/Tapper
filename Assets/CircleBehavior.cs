using UnityEngine;
using UnityEngine.EventSystems;

public class CircleBehavior : MonoBehaviour, IPointerClickHandler
{
    public bool isGood = true;
    [SerializeField] private float timeOnScreen = 1f;

    void Update()
    {
        if (timeOnScreen > 0f)
        {
            timeOnScreen -= Time.deltaTime;
        }
        else
        {
            if (isGood)
            {
                GameManager.Singleton.RemoveLives(1);
            }

            Destroy(this.gameObject);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isGood)
        {
            GameManager.Singleton.AddPoints(1);
        }
        else
        {
            GameManager.Singleton.AddPoints(-1);
            GameManager.Singleton.RemoveLives(1);
        }
        Destroy(this.gameObject);
    }
}

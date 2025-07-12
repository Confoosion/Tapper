using UnityEngine;

public class PlayerTap : MonoBehaviour
{
    [SerializeField] private bool circleClicked;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (circleClicked)
            {
                circleClicked = false;
            }
            else
            {
                GameManager.Singleton.AddPoints(-1);
                Debug.Log("Clicked on nothing!");
            }
        }
    }

    public void GoodCircleClicked(GameObject circle)
    {
        circleClicked = true;
        Destroy(circle);
        GameManager.Singleton.AddPoints(1);
    }

    public void BadCircleClicked(GameObject circle)
    {
        circleClicked = true;
        Destroy(circle);
        GameManager.Singleton.SetPlayerStatus(false);
    }
}

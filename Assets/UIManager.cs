using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Singleton { get; private set; }

    [SerializeField] private List<GameObject> hearts = new List<GameObject>();
    [SerializeField] private TextMeshProUGUI points;

    void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
    }

    public void UpdateHearts(int lives)
    {
        // Show lives
        for (int i = 0; i < lives; i++)
        {
            hearts[i].SetActive(true);
        }

        // Hide lives
        for (int i = lives; i < hearts.Count; i++)
        {
            hearts[i].SetActive(false);
        }
    }

    public void UpdatePoints(int num)
    {
        points.SetText(num.ToString());
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Singleton { get; private set; }

    private float countdownInterval = 0.8f;
    [SerializeField] private GameObject countdownObject;
    [SerializeField] private List<Sprite> countdownImages = new List<Sprite>();

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

    public void BeginCountdown()
    {
        StartCoroutine(CountingDown());
    }

    IEnumerator CountingDown()
    {
        Image countdownImage = countdownObject.GetComponent<Image>();

        yield return new WaitForSeconds(1f);
        countdownObject.SetActive(true);

        foreach (Sprite image in countdownImages)
        {
            countdownImage.sprite = image;
            yield return new WaitForSeconds(countdownInterval);
        }

        countdownObject.SetActive(false);
        GameManager.Singleton.StartGame();
    }
}

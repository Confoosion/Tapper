using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ArcadeEnding : MonoBehaviour
{
    public static ArcadeEnding Singleton;

    [SerializeField] private GameObject[] lossReasons = new GameObject[3];

    void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
    }

    void Start()
    {
        foreach(GameObject reason in lossReasons)
        {
            reason.SetActive(false);
        }
    }

    public void SetReason(int livesLeft, Sprite reason)
    {
        lossReasons[livesLeft].GetComponent<Image>().sprite = reason;
        lossReasons[livesLeft].SetActive(true);
    }
}

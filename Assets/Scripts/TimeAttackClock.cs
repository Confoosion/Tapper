using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class TimeAttackClock : MonoBehaviour
{
    public static TimeAttackClock Singleton;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] private int START_TIME = 60;
    [SerializeField] private int time;
    private int tickRate = 1;

    void Awake()
    {
        if(Singleton == null)
        {
            Singleton = this;
        }
    }

    public void AddTime(int seconds)
    {
        if (time + seconds < 0)
        {
            time = 0;
        }
        else
        {
            time += seconds;
        }
        timerText.SetText(time.ToString());
    }

    public void StartClock()
    {
        time = START_TIME;
        StartCoroutine(ClockTicking());
        timerText.SetText(time.ToString());
    }
    
    IEnumerator ClockTicking()
    {
        while (time > 0)
        {
            yield return new WaitForSeconds(tickRate);
            time--;
            timerText.SetText(time.ToString());
        }
    }
}

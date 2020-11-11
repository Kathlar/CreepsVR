using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTimer : MonoBehaviour
{
    public bool timerOn;
    public float timerValue = -1;

    public List<GameObject> digitNumbers = new List<GameObject>();
    public List<GameObject> decimalNumbers = new List<GameObject>();
    private int digitNumber, decimalNumber;

    public delegate void TimerEnd();
    public TimerEnd timerEnd;

    private void Awake()
    {
        TurnOffTimer();
    }

    private void Update()
    {
        if (!timerOn) return;

        float newTimerValue = timerValue - Time.deltaTime;
        if(newTimerValue <= 0)
        {
            Debug.Log(timerValue);
            Debug.Log("TEST");
            TurnOffTimer();
            timerEnd();
        }
        else
        {
            if (Mathf.Floor(newTimerValue) != Mathf.Floor(timerValue))
            {
                SetObjects((int)newTimerValue);
            }
            timerValue = newTimerValue;
        }
    }

    public void Set(TimerEnd onTimerEnd)
    {
        timerEnd = onTimerEnd;
    }

    public void SetTimer(float time)
    {
        if (!LevelFlow.TimerGame) return;
        timerOn = true;
        timerValue = time;
        SetObjects((int)timerValue);
    }

    private void SetObjects(int number)
    {
        foreach (GameObject t in digitNumbers)
            t.SetActive(false);
        foreach (GameObject t in decimalNumbers)
            t.SetActive(false);
        int digitN = (int)number % 10;
        int decimalN = (int)(number / 10) % 10;
        digitNumbers[digitN].SetActive(true);
        decimalNumbers[decimalN].SetActive(true);
    }

    public void TurnOffTimer()
    {
        timerValue = 0;
        timerOn = false;
        foreach (GameObject t in digitNumbers)
            t.SetActive(false);
        foreach (GameObject t in decimalNumbers)
            t.SetActive(false);
    }
}

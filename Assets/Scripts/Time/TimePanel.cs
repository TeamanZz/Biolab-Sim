using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class TimePanel : MonoBehaviour
{
    public static TimePanel singleton { get; private set; }

    public GameObject NewsPanel;
    public TextMeshProUGUI minutesText;
    public TextMeshProUGUI hoursText;
    public TextMeshProUGUI daysText;
    public bool isPaused;
    public float param = 1f;
    public int minutes;
    public int hours;
    public int days;

    private void Awake()
    {
        singleton = this;
    }

    private void FixedUpdate()
    {
        // SecondPauseChanger();
    }

    void Update()
    {
        if (isPaused == false)
            ChangeTimeValues();

        ChangeTextOClockText();
    }

    private void ChangeTimeValues()
    {
        param -= Time.deltaTime;
        if (param <= 0)
        {
            param = 1;
            minutes += 10;
        }

        if (minutes >= 60)
        {
            hours++;
            minutes = minutes % 60;
        }

        if (hours >= 3)
        {
            days++;
            hours = 0;
        }
    }

    private void ChangeTextOClockText()
    {
        minutesText.text = minutes.ToString();
        hoursText.text = hours.ToString();
        daysText.text = days.ToString();
    }

    public void SecondPauseChanger()
    {
        if (OpenWindowsManager.singletone.iconsList.Count > 0)
            isPaused = true;
        else
            isPaused = false;
    }

    public void ToggleNewsPanel()
    {
        if (NewsPanel.activeSelf == true)
        {
            NewsManager.singleton.HideAllNewButtons();
        }
        NewsPanel.SetActive(!NewsPanel.activeSelf);
    }
}
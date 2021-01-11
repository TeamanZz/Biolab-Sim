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
    public OrdersManager ordersManager;
    public TextMeshProUGUI minutesText;
    public TextMeshProUGUI hoursText;
    public TextMeshProUGUI daysText;
    public TextMeshProUGUI timesOfDay;
    public bool isPaused;
    public int minutes;
    public int hours;
    public int days;

    private float param = 1.25f;

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
    }

    private void ChangeTimeValues()
    {
        param -= Time.deltaTime;
        if (param <= 0)
        {
            param = 1;
            minutes += 10;
            ChangeTextOClockText();
        }

        if (minutes >= 60)
        {
            hours++;
            minutes = minutes % 60;
            ChangeTextOClockText();
            ordersManager.CheckOrdersOnSpawn(hours, days);
        }

        if (hours >= 24)
        {
            days++;
            hours = 0;
            ChangeTextOClockText();
        }
    }

    private void ChangeTextOClockText()
    {
        minutesText.text = minutes.ToString();
        if (minutesText.text == "0")
        {
            string newMinutesValue = minutesText.text.Insert(0, "0");
            minutesText.text = newMinutesValue;
        }
        hoursText.text = hours.ToString();
        if (hours.ToString().Length < 2)
        {
            string newHoursValue = hoursText.text.Insert(0, "0");
            hoursText.text = newHoursValue;
        }

        daysText.text = days.ToString();
        ChangeTimesOfDay(hours);
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

    public void ChangeTimesOfDay(int hours)
    {
        if (hours >= 0 && hours <= 6)
        {
            timesOfDay.text = "Night";
        }
        else
        {
            timesOfDay.text = "Day";
        }
    }
}
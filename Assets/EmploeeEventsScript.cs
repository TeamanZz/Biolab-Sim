using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class EmploeeEventsScript : MonoBehaviour
{
    public static EmploeeEventsScript singleton { get; private set; }

    private Worker currentWorker;
    public Data data;

    public Worker Worker
    {
        set
        {
            currentWorker = value;
        }
        get
        {
            return currentWorker;
        }
    }

    public List<EmploeeAction> emploeeHappenings = new List<EmploeeAction>();

    private void Awake()
    {
        singleton = this;
    }

    public void GetMoney()
    {
        Debug.Log("Работник получил валюты!(200)");
        data.currencyData.moneyCount -= 200;
    }

    public void KillWorker()
    {
        Debug.Log("Вы убили работника " + currentWorker.name);
    }

    public void FeedWorkersDog()
    {
        Debug.Log("Вы покормили собаку работника");
    }

    public void CloseWindow()
    {
        EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.SetActive(false);
        GameObject statsPanel = GameObject.Find("StatsPanel(Clone)");
        statsPanel.GetComponent<WorkerStatsPanel>().whatHappenedButton.SetActive(false);

    }
}

[Serializable]
public class EmploeeAction
{
    public String actionDescription;

    public UnityEvent firstButtonEvent;
    public UnityEvent secondButtonEvent;
    public UnityEvent thirdButtonEvent;

    public String firstButtonText;
    public String secondButtonText;
    public String thirdButtonText;
}
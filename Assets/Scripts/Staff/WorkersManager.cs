using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WorkersManager : MonoBehaviour
{
    public Data data;
    //Префаб работника, который располагается на нижней панели
    public GameObject addedWorkerPrefab;
    public GameObject newWorkerPrefab;
    public GameObject workerPanel;
    public GameObject buyWorkerPanel;
    public List<Worker> workers;

    //Текущая открытая панель с вакансией]
    [HideInInspector] public GameObject infoAboutNewWorkerPanel;

    //Текущая открытая панель со статами персонажа]
    [HideInInspector] public GameObject statsOfWorker;

    private GameObject newAvailableWorker;
    private GameObject addedWorker;

    private void Start()
    {
        SpawnWorkersInShop();
    }

    //Создаёт в нижней панели иконку работника
    public void BuyWorker(Worker worker)
    {
        addedWorker = Instantiate(addedWorkerPrefab, workerPanel.transform);
        addedWorker.GetComponent<WorkerScript>().Worker = worker;
        addedWorker.GetComponent<WorkerScript>().Worker.mood = Worker.Mood.Good;
        addedWorker.GetComponent<WorkerScript>().Worker.moodImage = data.equipmentData.workersMoodImages[0];
        buyWorkerPanel.SetActive(false);
        OpenWindowsManager.singletone.AddOrRemovePanelFromList(buyWorkerPanel);
        infoAboutNewWorkerPanel.SetActive(false);
    }

    //Хард код способ спавна рабочих для покупки
    private void SpawnWorkersInShop()
    {
        foreach (Worker worker in this.workers)
        {
            newAvailableWorker = Instantiate(newWorkerPrefab, buyWorkerPanel.transform);
            newAvailableWorker.GetComponent<WorkerScript>().Worker = worker;
        }
    }
}

[Serializable]
public class Worker
{
    public enum Profession
    {
        Assistant,
        Professor
    }

    public enum Education
    {
        WithoutEducation,
        PHD
    }

    public enum Status
    {
        Busy,
        Free,
        NotPurchased
    }

    public enum Resposibility
    {
        Responsible,
        Helper
    }

    public enum Mood
    {
        Good,
        Frustrated,
        Bad
    }

    public Order currentOrder;
    public Mood mood;
    public GameObject orderStepsPanel;
    public Sprite photo;
    [HideInInspector] public Sprite moodImage;
    public String name;
    public int salary;
    public Profession profession;
    public Education education;
    public Status status;
    public String description;
    public int completedOrdersCount;
    public Resposibility resposibility;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

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
    private WorkerScript addedWorkerScript;

    private void Start()
    {
        SpawnWorkersInShop();
    }

    //Создаёт в нижней панели иконку работника
    public void BuyWorker(Worker worker)
    {
        addedWorker = Instantiate(addedWorkerPrefab, workerPanel.transform);
        addedWorkerScript = addedWorker.GetComponent<WorkerScript>();
        addedWorkerScript.Worker = worker;
        addedWorkerScript.Worker.mood = Worker.Mood.Good;
        addedWorkerScript.Worker.moodImage = data.equipmentData.workersMoodImages[0];
        buyWorkerPanel.SetActive(false);
        OpenWindowsManager.singletone.AddOrRemovePanelFromList(buyWorkerPanel);
        infoAboutNewWorkerPanel.SetActive(false);
        ToggleBlueLight();
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

    private void ToggleBlueLight()
    {
        StartCoroutine(addedWorker.GetComponent<WorkerScript>().ToggleBlueLight());
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

    public enum Specialization
    {
        Microbiology
    }

    public enum Qualificaton
    {
        FirstLevel,
        SecondLevel,
        ThirdLevel
    }

    public enum Status
    {
        Free,
        Busy,
        VeryBusy
    }

    public enum Responsibility
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

    [HideInInspector]
    public GameObject orderStepsPanel;
    [HideInInspector]
    public GameObject workerSlotContainer;
    public int workerIndex;
    public int salary;
    public int completedOrdersCount;
    public int enhancementTime;
    public int enhancementCost;
    public int currentHappeningIndex;
    public String name;
    public String fullName;
    public String description;
    public Sprite photo;
    public Order currentOrder;
    public Mood mood;
    public Profession profession;
    public Education education;
    public Specialization specialization;
    public Responsibility responsibility;
    public Qualificaton qualificaton;
    public Status status;
    [HideInInspector]
    public bool isEnhancementProcess;
    [HideInInspector]
    public float enhancementFillAmount = 0;

    [HideInInspector] public Sprite moodImage;

}
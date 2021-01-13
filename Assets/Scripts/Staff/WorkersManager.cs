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
        // OpenWindowsManager.singletone.AddOrRemovePanelFromList(buyWorkerPanel);
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
    [Serializable]
    public enum Profession
    {
        Laboratory_assistant,
        Junior_researcher,
        Senior_researcher,
        Scientist,
        Professor
    }

    public enum Education
    {
        Without_education,
        Specialized_secondary,
        Higher,
        Aspirate,
        Assistant_professor,
        PHD,
        Academician
    }
    [Serializable]
    public enum Specialization
    {
        Bioengineering,
        Virology,
        Bacteriology,
        Neurobiology,
        Medical_engineering
    }

    public enum Qualificaton
    {
        Low,
        Neutral,
        High
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

    public int workerIndex;
    public Sprite photo;
    public String name;
    public String fullName;
    public String description;
    public int salary;
    public int completedOrdersCount;

    [Header("Competitions")]
    public List<Specialization> specialization;
    public Profession profession;
    public Education education;
    public Responsibility responsibility;
    public Qualificaton qualificaton;
    public Status status;
    public Mood mood;

    [Header("Enhancement")]
    public int enhancementTime;
    public int enhancementCost;

    [HideInInspector] public GameObject orderStepsPanel;
    [HideInInspector] public GameObject workerSlotContainer;
    [HideInInspector] public bool isEnhancementProcess;
    [HideInInspector] public float enhancementFillAmount = 0;
    [HideInInspector] public int currentHappeningIndex;
    [HideInInspector] public Order currentOrder;
    [HideInInspector] public Sprite moodImage;

}
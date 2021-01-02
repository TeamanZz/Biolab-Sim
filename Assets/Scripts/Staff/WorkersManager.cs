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
        StartCoroutine(ToggleBlueLight());
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

    IEnumerator ToggleBlueLight()
    {
        addedWorker.GetComponent<Image>().sprite = data.workerData.workerFrames[3];
        yield return new WaitForSeconds(3);
        addedWorker.GetComponent<Image>().sprite = data.workerData.workerFrames[0];
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
    public int workerIndex;
    public int salary;
    public int completedOrdersCount;
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
    public Status status;

    [HideInInspector] public Sprite moodImage;

}
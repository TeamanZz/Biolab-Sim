using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class WorkerScript : MonoBehaviour
{
    public Image characterImage;
    public Image stateFrame;
    public Image responsibilityStar;
    public TextMeshProUGUI workerName;
    [HideInInspector]
    public GameObject newWorkerDescriptionPanel;

    [SerializeField] private Worker worker;
    private GameObject newWorker;
    private GameObject manager;
    private Data data;

    public Worker Worker
    {
        set
        {
            worker = value;
            characterImage.sprite = worker.photo;
            workerName.text = worker.name;
        }
        get { return worker; }
    }

    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Manager");
        data = manager.GetComponent<WorkersManager>().data;

        if (GetComponent<MoodScript>())
        {
            GetComponent<MoodScript>().ChangeWorkerMood(worker, Worker.Mood.Good);
        }
    }

    private void FixedUpdate()
    {
        if (GetComponent<MoodScript>())
        {
            GetComponent<MoodScript>().SetMoodImage(this);
        }
    }

    //Открыть окно с описанием покупаемого персонажа
    public void ShowInfoAboutNewWorker()
    {
        if (newWorker == null)
        {
            //Если есть уже открытое окно, удаляем его
            if (manager.GetComponent<WorkersManager>().infoAboutNewWorkerPanel != null)
            {
                Destroy(manager.GetComponent<WorkersManager>().infoAboutNewWorkerPanel);
            }

            newWorker = Instantiate(newWorkerDescriptionPanel, GameObject.FindGameObjectWithTag("Canvas").transform, false);
            newWorker.SetActive(false);
            manager.GetComponent<WorkersManager>().infoAboutNewWorkerPanel = newWorker;

            NewWorkerDescription newWorkerDescription = newWorker.GetComponent<NewWorkerDescription>();

            newWorkerDescription.Worker = this.worker;
            newWorkerDescription.fullName.text = newWorkerDescription.Worker.fullName;
            newWorkerDescription.nameBoxText.text = newWorkerDescription.Worker.name;
            newWorkerDescription.availableWorker = gameObject;
        }
        newWorker.SetActive(!newWorker.activeSelf);
    }

    // Изменяем параметр занятости у работника и его рамку
    public void ChangeStateImage(Worker.Status status)
    {
        worker.status = status;
        stateFrame.sprite = data.workerData.workerFrames[(int)status];
    }

    // Изменяем параметр настроения и изображение смайлика
    public void ChangeMood(Worker.Mood mood)
    {
        worker.mood = mood;
        worker.moodImage = data.workerData.workerMoods[(int)mood];
    }

    //Изменяем параметр ответственности за проект
    public void ChangeResponsibility(Worker.Responsibility responsibility)
    {
        worker.responsibility = responsibility;
        if (responsibility == Worker.Responsibility.Responsible)
        {
            responsibilityStar.gameObject.SetActive(true);
        }
        else
        {
            responsibilityStar.gameObject.SetActive(false);
        }
    }
}
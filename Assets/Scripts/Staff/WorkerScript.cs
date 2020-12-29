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
    [HideInInspector]
    public GameObject newWorkerDescriptionPanel;

    [SerializeField] private Worker worker;
    private GameObject newWorker;
    private GameObject manager;

    public Worker Worker
    {
        set
        {
            worker = value;
            characterImage.sprite = worker.photo;
        }
        get { return worker; }
    }

    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Manager");

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

            newWorker.GetComponent<NewWorkerDescription>().Worker = this.worker;
            newWorker.GetComponent<NewWorkerDescription>().nameText.text = newWorker.GetComponent<NewWorkerDescription>().Worker.name;
            newWorker.GetComponent<NewWorkerDescription>().availableWorker = gameObject;
        }
        newWorker.SetActive(!newWorker.activeSelf);
    }

    //Изменяем цвет индикатора в зависимости от текущего заказа работника
    // public void ChangeStateImage()
    // {
    //     if (worker.status == Worker.Status.Free)
    //     {
    //         stateImage.GetComponent<Image>().color = Color.green;
    //     }
    //     else if (worker.status == Worker.Status.Busy)
    //     {
    //         stateImage.GetComponent<Image>().color = Color.red;
    //     }
    // }
}
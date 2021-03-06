﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using System;

public class WorkerScript : MonoBehaviour
{
    public Image characterImage;
    public Image stateFrame;
    public Image responsibilityStar;
    public TextMeshProUGUI workerName;
    [HideInInspector]
    public GameObject newWorkerDescriptionPanel;
    [HideInInspector]
    public GameObject enhancementButton;

    [HideInInspector] public Image enhanceLoadBar;

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

    private void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("Manager");
        data = manager.GetComponent<WorkersManager>().data;
    }

    private void Start()
    {
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

            newWorker.transform.SetAsLastSibling();
        }
        DarkBackground.singletone.staffPanel.transform.GetChild(0).GetComponent<AddStaffButton>().descriptionPanel = newWorker;
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

    public IEnumerator IBlueLightFrameOn()
    {
        Image image = GetComponent<Image>();
        image.sprite = data.workerData.workerFrames[3];
        yield return new WaitForSeconds(3);

        if (this != null && image.sprite == data.workerData.workerFrames[3])
            image.sprite = data.workerData.workerFrames[0];
    }

    public void BlueLightFrameOff()
    {
        StopAllCoroutines();
    }

    public void StartEnhanceProcess(int time)
    {
        StartCoroutine(IFillingLoadBar(time));
        StartCoroutine(IStartEnhanceProcess(time));
    }

    public IEnumerator IStartEnhanceProcess(float time)
    {
        DragWorker dragWorker = GetComponent<DragWorker>();
        dragWorker.enabled = false;
        worker.isEnhancementProcess = true;
        yield return new WaitForSeconds(time * TimePanel.singleton.param);
        worker.qualificaton += 1;
        worker.isEnhancementProcess = false;
        dragWorker.enabled = true;

        //Если есть куда апаться, то возвращаем кнопке интерактивность
        if (((int)worker.qualificaton + 1 < Enum.GetNames(typeof(Worker.Qualificaton)).Length) && enhancementButton)
        {
            enhancementButton.GetComponent<Button>().interactable = true;
        }
    }

    public IEnumerator IFillingLoadBar(int time)
    {
        float newFillAmount = 0;
        while (worker.enhancementFillAmount < 1f)
        {
            yield return new WaitForSeconds(0.1f);
            newFillAmount += (0.1f / time * TimePanel.singleton.param);
            if (enhanceLoadBar)
            {
                enhanceLoadBar.fillAmount = newFillAmount;
            }
            worker.enhancementFillAmount = newFillAmount;
        }
        worker.enhancementFillAmount = 0;
        enhanceLoadBar.fillAmount = 0;
    }
}
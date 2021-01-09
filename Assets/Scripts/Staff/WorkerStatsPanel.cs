using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class WorkerStatsPanel : MonoBehaviour
{
    public GameObject statsPanel;
    public GameObject awardPanel;
    public GameObject whatHappenedPanel;
    public Image loadBarImage;
    public Image workerImage;
    public Image workerFrameImage;
    public Image responsibilityStar;
    public TextMeshProUGUI fullName;
    public TextMeshProUGUI nameBoxText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI professionText;
    public TextMeshProUGUI educationText;
    public TextMeshProUGUI specializationText;
    public TextMeshProUGUI completedOrdersCountText;
    public TextMeshProUGUI currentOrderText;

    [Header("Enhancement Panel")]
    public GameObject enhancementButton;
    public GameObject enhancementPanel;
    public TextMeshProUGUI enhancementCostText;
    public TextMeshProUGUI enhancementTimeText;
    public TextMeshProUGUI qualificaton;

    [Header("What Happened Panel")]
    public GameObject whatHappenedButton;
    public Button firstChoose;
    public Button secondChoose;
    public Button thirdChoose;
    public TextMeshProUGUI firstChooseText;
    public TextMeshProUGUI secondChooseText;
    public TextMeshProUGUI thirdChooseText;

    public TextMeshProUGUI happeningDescription;

    [Space]
    [HideInInspector] public OrderScript orderScript;

    private GameObject manager;
    private GameObject canvas;
    private GameObject newStatsPanel;
    private Data data;

    [HideInInspector]
    [SerializeField]
    private GameObject clickedWorkerFrameButton; //Должно быть сериализовано

    [SerializeField]
    private Worker worker;

    public Worker Worker
    {
        set
        {
            data = GameObject.FindGameObjectWithTag("Manager").gameObject.GetComponent<WorkersManager>().data;
            worker = value;
            fullName.text = worker.fullName;
            nameBoxText.text = worker.name;
            workerImage.sprite = worker.photo;

            workerFrameImage.sprite = data.workerData.workerFrames[(int)worker.status];
            descriptionText.text = worker.description;
            professionText.text = worker.profession.ToString();
            educationText.text = worker.education.ToString();
            specializationText.text = worker.specialization.ToString();
            completedOrdersCountText.text = worker.completedOrdersCount.ToString();

            if (worker.status == Worker.Status.Free)
            {
                currentOrderText.text = "Не занят";
            }
            else if (worker.status == Worker.Status.Busy)
            {
                currentOrderText.text = "Текущий проект: " + worker.currentOrder.orderHeading;
                if (worker.responsibility == Worker.Responsibility.Responsible)
                {
                    responsibilityStar.gameObject.SetActive(true);
                }
                else
                {
                    responsibilityStar.gameObject.SetActive(false);
                }
            }

            //Если апаться некуда или идёт процесс повышения, отключаем возможность повышения (кнопку), иначе меняем текст квалификации
            if ((int)worker.qualificaton + 1 == Enum.GetNames(typeof(Worker.Qualificaton)).Length || worker.isEnhancementProcess || worker.orderStepsPanel != null)
            {
                enhancementButton.GetComponent<Button>().interactable = false;
            }
            else
            {
                enhancementButton.GetComponent<Button>().interactable = true;
                qualificaton.text = (worker.qualificaton + 1).ToString();
            }

            if (worker.mood == Worker.Mood.Bad)
            {
                whatHappenedPanel.SetActive(true);
            }
        }
        get
        {
            return worker;
        }
    }

    private void Awake()
    {
        //Передаём значения лоад бара и кнопки с повышением работнику при открытии панели с его статами
        clickedWorkerFrameButton.GetComponent<WorkerScript>().enhanceLoadBar = loadBarImage;
        clickedWorkerFrameButton.GetComponent<WorkerScript>().enhancementButton = enhancementButton;
    }

    private void Start()
    {
        if (worker.currentOrder != null && worker.currentOrder.orderStepsPanel != null)
        {
            orderScript = worker.currentOrder.orderStepsPanel.GetComponent<OrderScript>();
        }
    }

    private void FixedUpdate()
    {
        FillImageBar();
    }

    //Показать описание купленного персонажа
    public void ShowStatsOfWorker()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        manager = GameObject.FindGameObjectWithTag("Manager");
        clickedWorkerFrameButton = EventSystem.current.currentSelectedGameObject;

        if (manager.GetComponent<WorkersManager>().statsOfWorker == null)
        {
            newStatsPanel = Instantiate(statsPanel, canvas.transform, false);
            manager.GetComponent<WorkersManager>().statsOfWorker = newStatsPanel;
            OpenWindowsManager.singletone.AddOrRemovePanelFromList(newStatsPanel);

            //Заполняем панель описания купленного работника актуальной информацией.
            for (int i = 0; i < manager.GetComponent<WorkersManager>().workers.Count; i++)
            {
                if (manager.GetComponent<WorkersManager>().workers[i].description == clickedWorkerFrameButton.GetComponent<WorkerScript>().Worker.description)
                {
                    newStatsPanel.GetComponent<WorkerStatsPanel>().Worker = manager.GetComponent<WorkersManager>().workers[i];
                }
            }
        }
    }

    public void Dismiss()
    {
        //Проверяем, закреплен ли заказ за персонажем
        if (clickedWorkerFrameButton.GetComponent<WorkerScript>().Worker.orderStepsPanel != null)
        {
            GameObject stepsPanel = clickedWorkerFrameButton.GetComponent<WorkerScript>().Worker.orderStepsPanel;
            //Если в процессе выполнения или находится на панели, удаляем из панелей.
            if (worker.currentOrder != null)
            {
                stepsPanel.GetComponent<OrderScript>().DismissWorker(Worker);

                //Если заказ в процессе, паузим его.
                if (worker.currentOrder.stateOfOrder == Order.StateOfOrder.InProcess)
                {
                    switch (worker.currentOrder.currentStep)
                    {
                        //ИЗМЕНИ ТУТ ПРИ ДОБАВЛЕНИИ ЭТАПОВ
                        case Order.CurrentStep.Research:
                            stepsPanel.transform.GetChild(0).GetComponent<ResearchPanelScript>().PauseOrder();
                            break;
                        case Order.CurrentStep.Development:
                            stepsPanel.transform.GetChild(1).GetComponent<ResearchPanelScript>().PauseOrder();
                            break;
                        case Order.CurrentStep.Testing:
                            stepsPanel.transform.GetChild(2).GetComponent<ResearchPanelScript>().PauseOrder();
                            break;
                    }
                }
            }
            else
            {
                Destroy(clickedWorkerFrameButton);
            }
        }
        else
        {
            Destroy(clickedWorkerFrameButton);
        }
        //И панель удалить не забываем
        StopCoroutine(clickedWorkerFrameButton.GetComponent<WorkerScript>().ToggleBlueLight());
        Debug.Log("Stopped");
        OpenWindowsManager.singletone.iconsList.Remove(gameObject);
        worker.currentOrder = null;
        Destroy(gameObject);

    }

    private void FillImageBar()
    {
        //Заполняем прогресс бар при выполнении заказа
        if (worker.currentOrder != null && worker.currentOrder.orderHeading != "" && worker.currentOrder.orderStepsPanel != null)
        {
            //Заполняем прогресс бар в панели
            loadBarImage.fillAmount = orderScript.orderFillAmount;
        }
        if (worker.currentOrder == null)
        {
            loadBarImage.fillAmount = 0;
        }

        //Заполняем прогресс бар при повышении
        if (worker.isEnhancementProcess)
        {
            //Заполняем прогресс бар в панели
            loadBarImage.fillAmount = worker.enhancementFillAmount;
        }
        else if (worker.currentOrder == null)
        {
            loadBarImage.fillAmount = 0;
        }
    }

    public void HidePanelWithCross()
    {
        manager = GameObject.FindGameObjectWithTag("Manager");
        OpenWindowsManager.singletone.iconsList.Remove(gameObject);
        Destroy(manager.GetComponent<WorkersManager>().statsOfWorker);
    }

    public void TogglePanel(GameObject togglePanel)
    {
        togglePanel.SetActive(!togglePanel.activeSelf);
        SetWorkersHappening(togglePanel);
    }

    private void SetWorkersHappening(GameObject togglePanel)
    {
        if (togglePanel.gameObject.name == "WhatHappened Panel")
        {
            if (worker.currentHappeningIndex == 0)
            {
                worker.currentHappeningIndex = UnityEngine.Random.Range(1, EmploeeEventsScript.singleton.emploeeHappenings.Count);
            }
            int happeningIndex = worker.currentHappeningIndex;

            EmploeeAction currentEmploeeAction = EmploeeEventsScript.singleton.emploeeHappenings[happeningIndex - 1];
            EmploeeEventsScript.singleton.Worker = worker;

            happeningDescription.text = currentEmploeeAction.actionDescription;

            firstChoose.onClick.AddListener(currentEmploeeAction.firstButtonEvent.Invoke);
            secondChoose.onClick.AddListener(currentEmploeeAction.secondButtonEvent.Invoke);
            thirdChoose.onClick.AddListener(currentEmploeeAction.thirdButtonEvent.Invoke);

            firstChooseText.text = currentEmploeeAction.firstButtonText;
            secondChooseText.text = currentEmploeeAction.secondButtonText;
            thirdChooseText.text = currentEmploeeAction.thirdButtonText;
        }
    }

    public void CloseWindow()
    {
        //Улучшить настроение работника, чтобы не появлялась опять кнопка
        Debug.Log("clos");
        EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.SetActive(false);
        whatHappenedButton.SetActive(false);
    }

    public void AwardWorker(int awardSum)
    {
        if (awardSum <= data.currencyData.moneyCount)
        {
            data.currencyData.moneyCount -= awardSum;
            //ТУТ УВЕЛИЧЕНИЕ СТАТОВ ПРОПИСЫВАЕТСЯ
            awardPanel.SetActive(false);
        }
    }

    public void EnhanceWorker()
    {
        if (data.currencyData.moneyCount >= worker.enhancementCost)
        {
            GameObject staffPanel = GameObject.Find("Staff Panel");

            for (int i = 1; i < staffPanel.transform.childCount; i++)
            {
                if (staffPanel.transform.GetChild(i).GetComponent<WorkerScript>().Worker.workerIndex == worker.workerIndex)
                {
                    if (worker.workerSlotContainer)
                    {
                        worker.workerSlotContainer.GetComponent<WorkerSlot>().DestroyWorkerIcon();
                    }
                    data.currencyData.moneyCount -= worker.enhancementCost;
                    staffPanel.transform.GetChild(i).GetComponent<WorkerScript>().StartEnhanceProcess(worker.enhancementTime);
                    enhancementButton.GetComponent<Button>().interactable = false;
                    TogglePanel(enhancementPanel);
                }
            }
        }
    }
}
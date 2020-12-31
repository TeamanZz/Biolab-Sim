using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WorkerStatsPanel : MonoBehaviour
{
    public GameObject statsPanel;
    public Image loadBarImage;
    public Image workerImage;
    public Image workerFrameImage;
    public TextMeshProUGUI fullName;
    public TextMeshProUGUI nameBoxText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI professionText;
    public TextMeshProUGUI educationText;
    public TextMeshProUGUI specializationText;
    public TextMeshProUGUI completedOrdersCountText;
    public TextMeshProUGUI currentOrderText;

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

            workerFrameImage.sprite = data.workerData.workerFrames[worker.frameImageIndex];
            descriptionText.text = worker.description;
            professionText.text = worker.profession.ToString();
            educationText.text = worker.education.ToString();
            completedOrdersCountText.text = worker.completedOrdersCount.ToString();

            if (worker.status == Worker.Status.Free)
            {
                currentOrderText.text = "Не занят";
            }
            else if (worker.status == Worker.Status.Busy)
            {
                currentOrderText.text = "Текущий проект: " + worker.currentOrder.orderHeading;
                ChangeStateImage();
            }

        }
        get
        {
            return worker;
        }
    }

    private void Start()
    {
        if (worker.currentOrder != null && worker.currentOrder.orderStepsPanel != null)
            orderScript = worker.currentOrder.orderStepsPanel.GetComponent<OrderScript>();
    }

    private void FixedUpdate()
    {
        FillImageBar();
        GetComponent<MoodScript>().SetMoodImage(this);
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

    private void ChangeStateImage()
    {
        // if (worker.currentOrder == null || worker.currentOrder.orderHeading == "")
        // {
        //     stateImage.GetComponent<Image>().color = Color.green;
        // }
        // else
        // {
        //     stateImage.GetComponent<Image>().color = Color.red;
        // }
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
        OpenWindowsManager.singletone.iconsList.Remove(gameObject);
        worker.currentOrder = null;
        Destroy(gameObject);
    }

    private void FillImageBar()
    {
        if (worker.currentOrder != null && worker.currentOrder.orderHeading != "" && worker.currentOrder.orderStepsPanel != null)
        {
            //Заполняем прогресс бар в панели
            loadBarImage.fillAmount = orderScript.orderFillAmount;
        }
        if (worker.currentOrder == null)
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
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ResearchPanelScript : MonoBehaviour
{
    public Data data;
    public GameObject equipmentSuccessIcon;

    public GameObject setPanel;
    public GameObject duringPanel;
    public GameObject endPanel;
    public GameObject pauseButton;
    public GameObject continueButton;

    [Header("Set Panel")]
    public Image setPanelCustomerTypeImg;
    public Image setPanelIconImg;
    public TextMeshProUGUI setPanelTaskText;
    public TextMeshProUGUI setPanelResearchTermText;
    public TextMeshProUGUI setPanelRewardText;
    public TextMeshProUGUI additionalText;

    [Header("During Panel")]
    public Image duringPanelCustomerTypeImg;
    public Image duringPanelIconImg;
    public Image loadBarImage;
    public TextMeshProUGUI duringPanelResearchTermText;
    public TextMeshProUGUI duringPanelRewardText;


    [Header("End Panel")]
    public Image endPanelCustomerTypeImg;
    public Image endPanelIconImg;
    public TextMeshProUGUI endPanelResearchTermText;
    public TextMeshProUGUI endPanelRewardText;

    [Space]
    private SlotsInOrder slots;
    [SerializeField] private Order order;
    private GameObject canvas;

    public Order Order
    {
        set
        {
            //Заполняем панель в соответствии с информацией из заказа
            order = value;
            transform.parent.GetComponent<OrderScript>().AddLoadBarImageInArray(loadBarImage);

            setPanelIconImg.sprite = data.ordersData.orderIconsImages[(int)order.developmentSphere];
            setPanelCustomerTypeImg.sprite = data.ordersData.orderCustomerTypeImages[(int)order.customerType];
            setPanelTaskText.text = "Задача: " + order.research.taskText;
            setPanelResearchTermText.text = order.research.leadTime.ToString() + " cекунды";
            setPanelRewardText.text = order.reward.ToString() + " $";
            additionalText.text = order.research.additionalText;

            duringPanelIconImg.sprite = data.ordersData.orderIconsImages[(int)order.developmentSphere];
            duringPanelCustomerTypeImg.sprite = data.ordersData.orderCustomerTypeImages[(int)order.customerType];
            duringPanelResearchTermText.text = order.research.leadTime.ToString() + " cекунды";
            duringPanelRewardText.text = order.reward.ToString() + " $";

            endPanelIconImg.sprite = data.ordersData.orderIconsImages[(int)order.developmentSphere];
            endPanelCustomerTypeImg.sprite = data.ordersData.orderCustomerTypeImages[(int)order.customerType];
            endPanelResearchTermText.text = order.research.leadTime.ToString() + " cекунды";
            endPanelRewardText.text = order.reward.ToString() + " $";
        }
        get { return order; }
    }

    private void Awake()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas");
    }

    private void Start()
    {
        //Спавним слоты в панели заказа для работников и оборудования
        slots = GetComponent<SlotsInOrder>();
        slots.SpawnEquipmentSlots(order.research);
        slots.SpawnWorkersSlots(Order.research.requirementsForEmployees.Count);
        order.currentStep = Order.CurrentStep.Research;
    }

    private void MakeEquipmentBusy()
    {
        for (int i = 0; i < Enum.GetNames(typeof(Building.Type)).Length; i++)
        {
            if (order.research.requirementEquipmentList.Contains((Building.Type)i))
            {
                GameObject newEquipmentObject = ShopEquipmentManager.singleton.availableEquipment.Find(x => x.GetComponent<EquipmentInfo>().equipmentObject.equipmentType == (Building.Type)i);
                ShopEquipmentManager.singleton.availableEquipment.Remove(newEquipmentObject);
                newEquipmentObject.GetComponent<EquipmentInfo>().currentOrder = Order;
                ShopEquipmentManager.singleton.busyEquipment.Add(newEquipmentObject);
                order.research.usedEquipment.Add(newEquipmentObject);
            }
        }
    }

    public void SpawnEquipmentSuccessIcon()
    {
        for (int i = 0; i < order.research.usedEquipment.Count; i++)
        {
            GameObject sucIcon = Instantiate(equipmentSuccessIcon, canvas.transform);

            //first you need the RectTransform component of your canvas
            RectTransform CanvasRect = canvas.GetComponent<RectTransform>();

            Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(order.research.usedEquipment[i].transform.GetChild(1).transform.position);
            Vector2 WorldObject_ScreenPosition = new Vector2(
            ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
            ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

            //now you can set the position of the ui element
            sucIcon.GetComponent<RectTransform>().anchoredPosition = WorldObject_ScreenPosition;
            sucIcon.transform.SetAsFirstSibling();
        }
    }

    public void MakeEquipmentFree()
    {
        foreach (GameObject item in Order.research.usedEquipment)
        {
            item.GetComponent<EquipmentInfo>().currentOrder = null;
            ShopEquipmentManager.singleton.availableEquipment.Add(item);
            ShopEquipmentManager.singleton.busyEquipment.Remove(item);
        }
        order.research.usedEquipment.Clear();
    }

    //Запускает заказ в исполнение
    public void StartOrder()
    {
        // Debug.Log(IsHaveEquipment(order.research));
        if (IsHaveEquipment(order.research) && WorkersIsSet(gameObject, Order.research))
        {
            order.orderButtonIcon.GetComponent<OrderIcon>().ChangeCurrentActionText(order.currentStep);
            order.currentStepPanel = gameObject;
            duringPanel.SetActive(true);
            SetCurrentOrder(gameObject);
            SetWorkersStateIcon(Worker.Status.Busy);
            SetResponsibleWorker();
            ActiveOrdersManager.singleton.ExecuteOrder(Order, gameObject);
            MakeEquipmentBusy();

            InstantiateWorkersInSecondWindow();
            slots.HideDismissButtons();
            order.stateOfOrder = Order.StateOfOrder.InProcess;
            transform.parent.gameObject.SetActive(false);
            OpenWindowsManager.singletone.ShowResearchAcceptMessage();
            OpenWindowsManager.singletone.AddOrRemovePanelFromList(transform.parent.gameObject);
            setPanel.SetActive(false);
        }
    }

    public void ContinueOrder()
    {
        if (IsHaveEquipment(order.research) && WorkersIsSet(gameObject, order.research))
        {
            SetCurrentOrder(gameObject);
            SetWorkersStateIcon(Worker.Status.Busy);
            SetResponsibleWorker();
            ActiveOrdersManager.singleton.ExecuteOrder(Order, gameObject);
            MakeEquipmentBusy();
            // SetWorkersStateIcon(Worker.Status.Busy);
            slots.HideDismissButtons();
            order.stateOfOrder = Order.StateOfOrder.InProcess;
            pauseButton.SetActive(true);
            continueButton.SetActive(false);
        }
    }

    //Паузим проект
    public void PauseOrder()
    {
        Order.stateOfOrder = Order.StateOfOrder.Paused;
        ActiveOrdersManager.singleton.PauseOrder(Order, gameObject);

        //Указываем новое время
        Order.research.leadTime = ((1 - loadBarImage.GetComponent<Image>().fillAmount) * Order.research.leadTime);

        DeleteWorkersInSecondWindow();
        slots.ShowDismissButtons();
        ActiveOrdersManager.singleton.ClearCurrentOrders(gameObject);
        SetWorkersStateIcon(Worker.Status.Free);
        ResetResponsibility();
        MakeEquipmentFree();
        transform.parent.GetComponent<OrderScript>().assignedEmployees.Clear();
        transform.parent.GetComponent<OrderScript>().boughtedEmployees.Clear();

        continueButton.SetActive(true);
        pauseButton.SetActive(false);
    }

    //Удаляем иконки рабочих во втором окне заказа
    private void DeleteWorkersInSecondWindow()
    {
        for (int i = 0; i < order.research.requirementsForEmployees.Count; i++)
        {
            transform.GetChild(1).GetChild(0).GetChild(i).GetComponent<WorkerSlot>().isBusy = false;
            // Debug.Log(transform.GetChild(1).GetChild(0).GetChild(i).GetChild(0).gameObject);
            Destroy(transform.GetChild(1).GetChild(0).GetChild(i).GetChild(0).gameObject);
        }
    }

    //Создаём иконки рабочих во втором окне заказа
    private void InstantiateWorkersInSecondWindow()
    {
        for (int i = 0; i < transform.parent.GetComponent<OrderScript>().assignedEmployees.Count; i++)
        {
            GameObject newWorker = Instantiate(transform.parent.GetComponent<OrderScript>().assignedEmployees[i], transform.GetChild(1).GetChild(0).GetChild(i).transform);
            transform.GetChild(1).GetChild(0).GetChild(i).GetComponent<WorkerSlot>().isBusy = true;
            newWorker.transform.SetAsFirstSibling();
            newWorker.GetComponent<WorkerScript>().Worker.status = Worker.Status.Busy;
        }
    }

    //Устанавливаем цвет индикаторов для работников 
    public void SetWorkersStateIcon(Worker.Status status)
    {
        foreach (GameObject employer in transform.parent.GetComponent<OrderScript>().assignedEmployees)
        {
            WorkerScript employerWorkerScript = employer.GetComponent<WorkerScript>();
            employerWorkerScript.ChangeStateImage(status);
        }

        foreach (GameObject employer in transform.parent.GetComponent<OrderScript>().boughtedEmployees)
        {
            WorkerScript employerWorkerScript = employer.GetComponent<WorkerScript>();
            employerWorkerScript.ChangeStateImage(status);
        }
    }

    public void SetResponsibleWorker()
    {
        transform.parent.GetComponent<OrderScript>().assignedEmployees[0].GetComponent<WorkerScript>().ChangeResponsibility(Worker.Responsibility.Responsible);
        transform.parent.GetComponent<OrderScript>().boughtedEmployees[0].GetComponent<WorkerScript>().ChangeResponsibility(Worker.Responsibility.Responsible);

        WorkerScript assignedWorkerScript = transform.parent.GetComponent<OrderScript>().assignedEmployees[0].GetComponent<WorkerScript>();

        GameObject workerStatsPanel = GameObject.Find("StatsPanel(Clone)");
        if (workerStatsPanel)
        {
            WorkerStatsPanel workerStatsPanelScript = workerStatsPanel.GetComponent<WorkerStatsPanel>();
            if (workerStatsPanelScript.Worker.workerIndex == assignedWorkerScript.Worker.workerIndex)
            {
                workerStatsPanelScript.responsibilityStar.gameObject.SetActive(true);
            }
        }
    }

    public void ResetResponsibility()
    {
        transform.parent.GetComponent<OrderScript>().assignedEmployees[0].GetComponent<WorkerScript>().ChangeResponsibility(Worker.Responsibility.Helper);
        transform.parent.GetComponent<OrderScript>().boughtedEmployees[0].GetComponent<WorkerScript>().ChangeResponsibility(Worker.Responsibility.Helper);

        WorkerScript assignedWorkerScript = transform.parent.GetComponent<OrderScript>().assignedEmployees[0].GetComponent<WorkerScript>();

        GameObject workerStatsPanel = GameObject.Find("StatsPanel(Clone)");
        if (workerStatsPanel)
        {
            WorkerStatsPanel workerStatsPanelScript = workerStatsPanel.GetComponent<WorkerStatsPanel>();
            if (workerStatsPanelScript.Worker.workerIndex == assignedWorkerScript.Worker.workerIndex)
            {
                workerStatsPanelScript.responsibilityStar.gameObject.SetActive(false);
            }
        }
    }

    //Для каждого работника указываем его текущий проект
    public void SetCurrentOrder(GameObject panel)
    {
        WorkersManager manager = GameObject.FindObjectOfType<WorkersManager>();
        for (int j = 0; j < panel.transform.parent.GetComponent<OrderScript>().assignedEmployees.Count; j++)
        {
            for (int i = 0; i < manager.workers.Count; i++)
            {
                if (manager.workers[i].description == panel.transform.parent.GetComponent<OrderScript>().assignedEmployees[j].GetComponent<WorkerScript>().Worker.description)
                {
                    //Указываем что ребята в панели и просто купленные теперь заняты
                    manager.workers[i].status = Worker.Status.Busy;
                    panel.transform.parent.GetComponent<OrderScript>().assignedEmployees[j].GetComponent<WorkerScript>().Worker.status = Worker.Status.Busy;
                    manager.workers[i].currentOrder = panel.transform.parent.GetComponent<OrderScript>().Order;
                }
            }
        }
    }

    //Кнопка "Отложить"
    public void HideOrderSteps()
    {
        transform.parent.gameObject.SetActive(false);
        OpenWindowsManager.singletone.AddOrRemovePanelFromList(transform.parent.gameObject);
    }

    //Проверяет, совпадает ли количество установленных в панель рабочих с необходимым количеством
    public bool WorkersIsSet(GameObject stepPanel, Research research)
    {
        return (stepPanel.transform.parent.GetComponent<OrderScript>().assignedEmployees.Count == research.requirementsForEmployees.Count);
    }

    //Проверяет какое оборудование нужно для текущего заказа и возвращает true, если есть свободное
    public bool IsHaveEquipment(Research research)
    {
        for (int i = 0; i < Enum.GetNames(typeof(Building.Type)).Length; i++)
        {
            if (research.requirementEquipmentList.Contains((Building.Type)i))
            {
                GameObject newEquipment = ShopEquipmentManager.singleton.availableEquipment.Find(x => x.GetComponent<EquipmentInfo>().equipmentObject.equipmentType == (Building.Type)i);
                if (newEquipment == null)
                {
                    return false;
                }
            }
        }
        return true;
    }
}
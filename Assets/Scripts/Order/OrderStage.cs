using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class OrderStage : MonoBehaviour
{
    public Data data;
    public GameObject equipmentSuccessIcon;

    public GameObject setPanel;
    public GameObject duringPanel;
    public GameObject endPanel;

    [Header("Set Panel")]
    public Image setPanelCustomerTypeImg;
    public Image setPanelIconImg;
    public TextMeshProUGUI setPanelTaskText;
    public TextMeshProUGUI setPanelTermText;
    public TextMeshProUGUI setPanelTotalTimeText;
    public TextMeshProUGUI setPanelRewardText;
    public TextMeshProUGUI additionalText;

    [Header("During Panel")]
    public GameObject pauseButton;
    public GameObject continueButton;

    public Image duringPanelCustomerTypeImg;
    public Image duringPanelIconImg;
    public Image loadBarImage;
    public TextMeshProUGUI duringPanelTermText;
    public TextMeshProUGUI duringPanelTotalTimeText;
    public TextMeshProUGUI duringPanelRewardText;

    [Header("End Panel")]
    public GameObject chooseContainer;
    public GameObject choosePrefab;
    public Image endPanelCustomerTypeImg;
    public Image endPanelIconImg;
    public TextMeshProUGUI endPanelTotalTimeText;
    public TextMeshProUGUI endPanelRewardText;

    [Space]
    //research,development,testing
    public dynamic currentStage;
    private SlotsInOrder slots;
    private GameObject canvas;
    public GameObject sucIcon;
    [SerializeField] private Order order;

    public Order Order
    {
        set
        {
            order = value;
            DetermineCurrentStep();
            //Заполняем панель в соответствии с информацией из заказа
            SetPanelValues();
        }
        get { return order; }
    }

    private void Awake()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas");
    }

    private void Start()
    {
        DetermineCurrentStep();
        transform.parent.GetComponent<OrderScript>().AddLoadBarImageInArray(loadBarImage);
        //Спавним слоты в панели заказа для работников и оборудования
        slots = GetComponent<SlotsInOrder>();
        slots.SpawnEquipmentSlots(currentStage);
        slots.SpawnWorkersSlots(currentStage.requirementsForEmployees.Count);
        order.orderButtonIcon.GetComponent<OrderIcon>().ChangeCurrentActionText(order.currentStep);
        setPanel.gameObject.SetActive(true);
    }



    //Запускает заказ в исполнение
    public void StartOrder()
    {
        if (IsHaveEquipment() && WorkersIsSet())
        {
            OrderScript orderScript = transform.GetComponentInParent<OrderScript>();

            order.orderButtonIcon.GetComponent<OrderIcon>().ChangeCurrentActionText(order.currentStep);
            orderScript.remainingStageTime = currentStage.leadTime * TimePanel.singleton.param;
            order.currentStepPanel = gameObject;
            duringPanel.SetActive(true);
            SetCurrentOrder(gameObject);
            SetWorkersStateIcon(Worker.Status.Busy);
            InstantiateWorkersInSecondWindow();
            SetResponsibleWorker();
            ActiveOrdersManager.singleton.ExecuteOrder(Order, gameObject);

            MakeEquipmentBusy();

            slots.HideDismissButtons();
            order.stateOfOrder = Order.StateOfOrder.InProcess;
            transform.parent.gameObject.SetActive(false);
            // OpenWindowsManager.singletone.ShowResearchAcceptMessage();
            // OpenWindowsManager.singletone.AddOrRemovePanelFromList(transform.parent.gameObject);
            DarkBackground.singletone.UnFadeBackground();
            setPanel.SetActive(false);
        }
    }

    public void ContinueOrder()
    {
        if (IsHaveEquipment() && WorkersIsSet())
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
        currentStage.leadTime = ((1 - loadBarImage.GetComponent<Image>().fillAmount) * currentStage.leadTime);

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

    public dynamic DetermineCurrentStep()
    {
        Order parentOrder = transform.parent.GetComponent<OrderScript>().Order;

        if (parentOrder.requiredStages.Contains(Order.RequiredStages.ResearchStage) && parentOrder.research.completed == false)
        {
            currentStage = parentOrder.research;
            parentOrder.currentStep = Order.CurrentStep.Research;
            return currentStage;
        }
        else if (parentOrder.requiredStages.Contains(Order.RequiredStages.DevelopmentStage) && parentOrder.development.completed == false)
        {
            currentStage = parentOrder.development;
            parentOrder.currentStep = Order.CurrentStep.Development;
            return currentStage;
        }
        else if (parentOrder.requiredStages.Contains(Order.RequiredStages.TestingStage) && parentOrder.testing.completed == false)
        {
            currentStage = parentOrder.testing;
            parentOrder.currentStep = Order.CurrentStep.Testing;
            return currentStage;
        }
        else
        {
            return null;
        }
    }

    public void SetStepCompleted()
    {
        Order parentOrder = transform.parent.GetComponent<OrderScript>().Order;
        if (currentStage is Research)
        {
            parentOrder.research.completed = true;
            order.research.completed = true;
            currentStage = order.research;
        }
        else if (currentStage is Development)
        {
            parentOrder.development.completed = true;
            order.development.completed = true;
            currentStage = order.development;
        }
        else
        {
            parentOrder.testing.completed = true;
            order.testing.completed = true;
            currentStage = order.testing;
        }
    }

    public void StopOrder()
    {
        if (order.stateOfOrder == Order.StateOfOrder.InProcess)
        {
            ActiveOrdersManager.singleton.PauseOrder(Order, gameObject);
        }

        DeleteWorkersInSecondWindow();
        slots.ShowDismissButtons();
        ActiveOrdersManager.singleton.ClearCurrentOrders(gameObject);
        SetWorkersStateIcon(Worker.Status.Free);
        ResetResponsibility();
        MakeEquipmentFree();
        transform.parent.GetComponent<OrderScript>().assignedEmployees.Clear();
        transform.parent.GetComponent<OrderScript>().boughtedEmployees.Clear();

        Destroy(order.orderButtonIcon.gameObject);
        Destroy(gameObject.transform.parent.gameObject);
    }

    private void MakeEquipmentBusy()
    {
        var usedEquipment = transform.parent.GetComponent<OrderScript>().usedEquipment;
        for (int i = 0; i < Enum.GetNames(typeof(Building.Type)).Length; i++)
        {
            if (currentStage.requirementEquipmentList.Contains((Building.Type)i))
            {
                GameObject newEquipmentObject = ShopEquipmentManager.singleton.availableEquipment.Find(x => x.GetComponent<EquipmentInfo>().equipmentObject.equipmentType == (Building.Type)i);
                ShopEquipmentManager.singleton.availableEquipment.Remove(newEquipmentObject);
                newEquipmentObject.GetComponent<EquipmentInfo>().currentOrder = Order;
                ShopEquipmentManager.singleton.busyEquipment.Add(newEquipmentObject);
                usedEquipment.Add(newEquipmentObject);
            }
        }
    }

    public void SpawnEquipmentSuccessIcon()
    {
        var usedEquipment = transform.parent.GetComponent<OrderScript>().usedEquipment;
        //Заспавнить над оборудованием заказа по значку
        sucIcon = Instantiate(equipmentSuccessIcon, canvas.transform);

        //first you need the RectTransform component of your canvas
        RectTransform CanvasRect = canvas.GetComponent<RectTransform>();

        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(usedEquipment[0].transform.GetChild(1).transform.position);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

        //now you can set the position of the ui element
        sucIcon.GetComponent<RectTransform>().anchoredPosition = WorldObject_ScreenPosition;
        sucIcon.transform.SetAsFirstSibling();
        sucIcon.GetComponent<Button>().onClick.AddListener(order.orderButtonIcon.GetComponent<OrderIcon>().ShowHideSteps);
    }

    public void SpawnChoosePanels()
    {
        foreach (OrderChooseStep orderChooseStep in currentStage.chooseSteps)
        {
            GameObject newChoose = Instantiate(choosePrefab, chooseContainer.transform);
            newChoose.GetComponent<ChooseNextStep>().OrderChooseStep = orderChooseStep;
            newChoose.GetComponent<ChooseNextStep>().order = order;
        }
    }

    public void MakeEquipmentFree()
    {
        var usedEquipment = transform.parent.GetComponent<OrderScript>().usedEquipment;
        foreach (GameObject item in usedEquipment)
        {
            item.GetComponent<EquipmentInfo>().currentOrder = null;
            ShopEquipmentManager.singleton.availableEquipment.Add(item);
            ShopEquipmentManager.singleton.busyEquipment.Remove(item);
        }
        usedEquipment.Clear();
    }

    //Удаляем иконки рабочих во втором окне заказа
    private void DeleteWorkersInSecondWindow()
    {
        for (int i = 0; i < currentStage.requirementsForEmployees.Count; i++)
        {
            transform.GetChild(1).GetChild(0).GetChild(i).GetComponent<WorkerSlot>().isBusy = false;
            Destroy(transform.GetChild(1).GetChild(0).GetChild(i).GetChild(0).gameObject);
        }
    }

    //Создаём иконки рабочих во втором окне заказа
    private void InstantiateWorkersInSecondWindow()
    {
        int assingedWorkersCount = transform.parent.GetComponent<OrderScript>().assignedEmployees.Count;

        for (int i = 0; i < assingedWorkersCount; i++)
        {
            GameObject oldWorker = setPanel.transform.GetChild(0).GetChild(i).GetChild(0).gameObject;
            GameObject newWorker = Instantiate(oldWorker, transform.GetChild(1).GetChild(0).GetChild(i).transform);
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
        Worker firstSlotWorker = duringPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<WorkerScript>().Worker;
        List<GameObject> boughtedWorkersList = transform.parent.GetComponent<OrderScript>().boughtedEmployees;
        GameObject workerStatsPanel = GameObject.Find("StatsPanel(Clone)");

        for (int i = 0; i < boughtedWorkersList.Count; i++)
        {
            if (boughtedWorkersList[i].GetComponent<WorkerScript>().Worker.workerIndex == firstSlotWorker.workerIndex)
            {
                transform.parent.GetComponent<OrderScript>().boughtedEmployees[i].GetComponent<WorkerScript>().Worker.responsibility = Worker.Responsibility.Responsible;
                break;
            }
        }

        //Если открыта панель с работником и если открыта панель ответственного работника, спавним там звёздочку
        if (workerStatsPanel)
        {
            WorkerStatsPanel workerStatsPanelScript = workerStatsPanel.GetComponent<WorkerStatsPanel>();
            if (workerStatsPanelScript.Worker.workerIndex == firstSlotWorker.workerIndex)
            {
                workerStatsPanelScript.responsibilityStar.gameObject.SetActive(true);
            }
        }
    }

    public void ResetResponsibility()
    {
        if (transform.parent.GetComponent<OrderScript>().assignedEmployees.Count != 0)
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
        DarkBackground.singletone.UnFadeBackground();
        // OpenWindowsManager.singletone.AddOrRemovePanelFromList(transform.parent.gameObject);
    }

    //Проверяет, совпадает ли количество установленных в панель рабочих с необходимым количеством
    public bool WorkersIsSet()
    {
        int requirmentCountOfWorkers = transform.parent.GetComponent<OrderScript>().assignedEmployees.Count;
        return (requirmentCountOfWorkers == currentStage.requirementsForEmployees.Count);
    }

    //Проверяет какое оборудование нужно для текущего заказа и возвращает true, если есть свободное
    public bool IsHaveEquipment()
    {
        for (int i = 0; i < Enum.GetNames(typeof(Building.Type)).Length; i++)
        {
            if (currentStage.requirementEquipmentList.Contains((Building.Type)i))
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

    public void SetPanelValues()
    {
        setPanelIconImg.sprite = data.ordersData.orderIconsImages[(int)order.developmentSphere];
        setPanelCustomerTypeImg.sprite = data.ordersData.orderCustomerTypeImages[(int)order.customerType];
        setPanelTaskText.text = order.orderDescription;

        setPanelTermText.text = currentStage.leadTime.ToString() + " часов";
        setPanelTotalTimeText.text = order.totalTime.ToString() + " часов";
        setPanelRewardText.text = order.reward.ToString() + " $";
        additionalText.text = currentStage.additionalText;

        duringPanelIconImg.sprite = data.ordersData.orderIconsImages[(int)order.developmentSphere];
        duringPanelCustomerTypeImg.sprite = data.ordersData.orderCustomerTypeImages[(int)order.customerType];
        duringPanelTermText.text = currentStage.leadTime.ToString() + " часов";
        duringPanelTotalTimeText.text = order.totalTime.ToString() + " часов";
        duringPanelRewardText.text = order.reward.ToString() + " $";

        endPanelIconImg.sprite = data.ordersData.orderIconsImages[(int)order.developmentSphere];
        endPanelCustomerTypeImg.sprite = data.ordersData.orderCustomerTypeImages[(int)order.customerType];
        endPanelTotalTimeText.text = order.totalTime.ToString() + " часов";
        endPanelRewardText.text = order.reward.ToString() + " $";
    }
}

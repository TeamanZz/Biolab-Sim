using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResearchPanelScript : MonoBehaviour
{
    public Data data;

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

    public Order Order
    {
        set
        {
            //Заполняем панель в соответствии с информацией из заказа
            order = value;
            transform.parent.GetComponent<OrderScript>().AddLoadBarImageInArray(loadBarImage);

            setPanelIconImg.sprite = order.iconImg;
            setPanelCustomerTypeImg.sprite = order.customerTypeImg;
            setPanelTaskText.text = "Задача: " + order.research.taskText;
            setPanelResearchTermText.text = "Срок исследования: " + order.research.leadTime.ToString() + " cекунды";
            setPanelRewardText.text = order.reward.ToString() + " $";

            // duringPanelImage.sprite = order.iconImg;
            // duringPanelTaskText.text = "Задача: " + order.research.taskText;
            // duringPanelResearchTermText.text = "Срок исследования: " + order.research.leadTime.ToString() + " cекунды";
            // duringPanelRewardText.text = order.reward.ToString() + " $";

            // endPanelImage.sprite = order.iconImg;
            // endPanelResearchTermText.text = "Срок исследования: " + order.research.leadTime.ToString() + " cекунды";
            // endPanelRewardText.text = order.reward.ToString() + " $";
        }
        get { return order; }
    }

    private void Start()
    {
        //Спавним слоты в панели заказа для работников и оборудования
        slots = GetComponent<SlotsInOrder>();
        slots.SpawnEquipmentSlots(order.research);
        slots.SpawnWorkersSlots(Order.research.neededWorkers);
        order.currentStep = Order.CurrentStep.Research;
    }

    private void MakeEquipmentBusy()
    {
        if (order.research.needReagentTable)
        {
            GameObject tableObject = ShopEquipmentManager.singleton.availableEquipment.Find(x => x.GetComponent<EquipmentInfo>().equipmentObject.equipmentType == Building.Type.Table);
            tableObject.GetComponent<EquipmentInfo>().currentOrder = Order;
            ShopEquipmentManager.singleton.busyEquipment.Add(tableObject);
            order.research.usedEquipment.Add(tableObject);
            ShopEquipmentManager.singleton.availableEquipment.Remove(ShopEquipmentManager.singleton.availableEquipment.Find(x => x.GetComponent<EquipmentInfo>().equipmentObject.equipmentType == Building.Type.Table));
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
    public void Execute()
    {
        if (IsHaveEquipment(order.research) && WorkersIsSet(gameObject, Order.research))
        {
            Debug.Log("Executed");
            order.currentStepPanel = gameObject;
            duringPanel.SetActive(true);
            SetCurrentOrder(gameObject);
            SetWorkersStateIcon(gameObject);
            ActiveOrdersManager.singleton.StartOrder(Order, gameObject);
            MakeEquipmentBusy();
            SetWorkersStateIcon(gameObject);
            InstantiateWorkersInSecondWindow();
            slots.HideCrosses();
            //Удаляем из data стол, который юзали для заказа;
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
            SetWorkersStateIcon(gameObject);
            ActiveOrdersManager.singleton.StartOrder(Order, gameObject);
            SetWorkersStateIcon(gameObject);
            slots.HideCrosses();
            MakeEquipmentBusy();

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
        slots.ShowCrosses();
        ActiveOrdersManager.singleton.ClearCurrentOrders(gameObject);
        SetWorkersStateIcon(gameObject);
        MakeEquipmentFree();
        transform.parent.GetComponent<OrderScript>().assignedEmployees.Clear();
        transform.parent.GetComponent<OrderScript>().boughtedEmployees.Clear();

        continueButton.SetActive(true);
        pauseButton.SetActive(false);
    }

    //Удаляем иконки рабочих во втором окне заказа
    private void DeleteWorkersInSecondWindow()
    {
        for (int i = 0; i < order.research.neededWorkers; i++)
        {
            transform.GetChild(1).GetChild(0).GetChild(i).GetComponent<WorkerSlot>().isBusy = false;
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
    public void SetWorkersStateIcon(GameObject stepPanel)
    {
        foreach (GameObject employer in stepPanel.transform.parent.GetComponent<OrderScript>().assignedEmployees)
        {
            // employer.GetComponent<WorkerScript>().ChangeStateImage();
        }

        foreach (GameObject employer in stepPanel.transform.parent.GetComponent<OrderScript>().boughtedEmployees)
        {
            // employer.GetComponent<WorkerScript>().ChangeStateImage();
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
                    manager.workers[i].resposibility = panel.transform.parent.GetComponent<OrderScript>().assignedEmployees[j].transform.parent.GetComponent<WorkerSlot>().slotResponsibility;
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
        return (stepPanel.transform.parent.GetComponent<OrderScript>().assignedEmployees.Count == research.neededWorkers);
    }

    //Проверяет какое оборудование нужно для текущего заказа и возвращает true, если есть свободное
    public bool IsHaveEquipment(Research research)
    {
        bool state = true;

        if (research.needReagentTable)
        {
            if (!ShopEquipmentManager.singleton.availableEquipment.Find(x => x.GetComponent<EquipmentInfo>().equipmentObject.equipmentType == Building.Type.Table))
            {
                state = false;
                return state;
            }
        }

        if (research.needCapsule)
        {
            if (!ShopEquipmentManager.singleton.availableEquipment.Find(x => x.GetComponent<EquipmentInfo>().equipmentObject.equipmentType == Building.Type.Capsule))
            {
                state = false;
                return state;
            }
        }
        return state;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ResearchPanelScript : MonoBehaviour
{
    // public Data data;
    // public GameObject equipmentSuccessIcon;

    // public GameObject setPanel;
    // public GameObject duringPanel;
    // public GameObject endPanel;

    // [Header("Set Panel")]
    // public Image setPanelCustomerTypeImg;
    // public Image setPanelIconImg;
    // public TextMeshProUGUI setPanelTaskText;
    // public TextMeshProUGUI setPanelTotalTimeText;
    // public TextMeshProUGUI setPanelResearchTermText;
    // public TextMeshProUGUI setPanelRewardText;
    // public TextMeshProUGUI additionalText;

    // [Header("During Panel")]
    // public Image duringPanelCustomerTypeImg;
    // public Image duringPanelIconImg;
    // public Image loadBarImage;
    // public TextMeshProUGUI duringPanelTotalTimeText;
    // public TextMeshProUGUI duringPanelResearchTermText;
    // public TextMeshProUGUI duringPanelRewardText;
    // public GameObject pauseButton;
    // public GameObject continueButton;

    // [Header("End Panel")]
    // public GameObject chooseContainer;
    // public GameObject choosePrefab;
    // public Image endPanelCustomerTypeImg;
    // public Image endPanelIconImg;
    // public TextMeshProUGUI endPanelTotalTimeText;
    // public TextMeshProUGUI endPanelResearchTermText;
    // public TextMeshProUGUI endPanelRewardText;

    // [Space]
    // private SlotsInOrder slots;
    // private GameObject canvas;
    // [SerializeField] private Order order;

    // public Order Order
    // {
    //     set
    //     {
    //         //Заполняем панель в соответствии с информацией из заказа
    //         order = value;
    //         transform.parent.GetComponent<OrderScript>().AddLoadBarImageInArray(loadBarImage);

    //         setPanelIconImg.sprite = data.ordersData.orderIconsImages[(int)order.developmentSphere];
    //         setPanelCustomerTypeImg.sprite = data.ordersData.orderCustomerTypeImages[(int)order.customerType];
    //         setPanelTaskText.text = order.orderDescription;
    //         setPanelResearchTermText.text = order.research.leadTime.ToString() + " часов";
    //         setPanelTotalTimeText.text = order.totalTime.ToString() + " часов";
    //         setPanelRewardText.text = order.reward.ToString() + " $";
    //         additionalText.text = order.research.additionalText;

    //         duringPanelIconImg.sprite = data.ordersData.orderIconsImages[(int)order.developmentSphere];
    //         duringPanelCustomerTypeImg.sprite = data.ordersData.orderCustomerTypeImages[(int)order.customerType];
    //         duringPanelResearchTermText.text = order.research.leadTime.ToString() + " часов";
    //         duringPanelTotalTimeText.text = order.totalTime.ToString() + " часов";

    //         duringPanelRewardText.text = order.reward.ToString() + " $";

    //         endPanelIconImg.sprite = data.ordersData.orderIconsImages[(int)order.developmentSphere];
    //         endPanelCustomerTypeImg.sprite = data.ordersData.orderCustomerTypeImages[(int)order.customerType];
    //         endPanelResearchTermText.text = order.research.leadTime.ToString() + " часов";
    //         endPanelTotalTimeText.text = order.totalTime.ToString() + " часов";
    //         endPanelRewardText.text = order.reward.ToString() + " $";
    //     }
    //     get { return order; }
    // }

    // private void Awake()
    // {
    //     canvas = GameObject.FindGameObjectWithTag("Canvas");
    // }

    // private void Start()
    // {
    //     //Спавним слоты в панели заказа для работников и оборудования
    //     slots = GetComponent<SlotsInOrder>();
    //     slots.SpawnEquipmentSlots(order.research);
    //     slots.SpawnWorkersSlots(Order.research.requirementsForEmployees.Count);
    //     order.currentStep = Order.CurrentStep.Research;
    //     transform.GetChild(0).gameObject.SetActive(true);
    // }

    // public void SpawnChoosePanels()
    // {
    //     foreach (OrderChooseStep orderChooseStep in order.research.chooseSteps)
    //     {
    //         GameObject newChoose = Instantiate(choosePrefab, chooseContainer.transform);
    //         newChoose.GetComponent<ChooseNextStep>().OrderChooseStep = orderChooseStep;
    //         newChoose.GetComponent<ChooseNextStep>().order = order;
    //     }
    // }

    // public void StopOrder()
    // {
    //     if (order.stateOfOrder == Order.StateOfOrder.InProcess)
    //     {
    //         ActiveOrdersManager.singleton.PauseOrder(Order, gameObject);
    //     }

    //     DeleteWorkersInSecondWindow();
    //     slots.ShowDismissButtons();
    //     ActiveOrdersManager.singleton.ClearCurrentOrders(gameObject);
    //     SetWorkersStateIcon(Worker.Status.Free);
    //     ResetResponsibility();
    //     MakeEquipmentFree();
    //     transform.parent.GetComponent<OrderScript>().assignedEmployees.Clear();
    //     transform.parent.GetComponent<OrderScript>().boughtedEmployees.Clear();


    //     Destroy(order.orderButtonIcon.gameObject);
    //     Destroy(gameObject.transform.parent.gameObject);
    // }

    // private void MakeEquipmentBusy()
    // {
    //     for (int i = 0; i < Enum.GetNames(typeof(Building.Type)).Length; i++)
    //     {
    //         if (order.research.requirementEquipmentList.Contains((Building.Type)i))
    //         {
    //             GameObject newEquipmentObject = ShopEquipmentManager.singleton.availableEquipment.Find(x => x.GetComponent<EquipmentInfo>().equipmentObject.equipmentType == (Building.Type)i);
    //             ShopEquipmentManager.singleton.availableEquipment.Remove(newEquipmentObject);
    //             newEquipmentObject.GetComponent<EquipmentInfo>().currentOrder = Order;
    //             ShopEquipmentManager.singleton.busyEquipment.Add(newEquipmentObject);
    //             order.research.usedEquipment.Add(newEquipmentObject);
    //         }
    //     }
    // }

    // public void SpawnEquipmentSuccessIcon()
    // {
    //     //Заспавнить над оборудованием заказа по значку
    //     GameObject sucIcon = Instantiate(equipmentSuccessIcon, canvas.transform);

    //     //first you need the RectTransform component of your canvas
    //     RectTransform CanvasRect = canvas.GetComponent<RectTransform>();

    //     Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(order.research.usedEquipment[0].transform.GetChild(1).transform.position);
    //     Vector2 WorldObject_ScreenPosition = new Vector2(
    //     ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
    //     ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

    //     //now you can set the position of the ui element
    //     sucIcon.GetComponent<RectTransform>().anchoredPosition = WorldObject_ScreenPosition;
    //     sucIcon.transform.SetAsFirstSibling();
    //     sucIcon.GetComponent<Button>().onClick.AddListener(order.orderButtonIcon.GetComponent<OrderIcon>().ShowHideSteps);

    // }

    // public void MakeEquipmentFree()
    // {
    //     foreach (GameObject item in Order.research.usedEquipment)
    //     {
    //         item.GetComponent<EquipmentInfo>().currentOrder = null;
    //         ShopEquipmentManager.singleton.availableEquipment.Add(item);
    //         ShopEquipmentManager.singleton.busyEquipment.Remove(item);
    //     }
    //     order.research.usedEquipment.Clear();
    // }

    // //Запускает заказ в исполнение
    // public void StartOrder()
    // {
    //     // Debug.Log(IsHaveEquipment(order.research));
    //     if (IsHaveEquipment(order.research) && WorkersIsSet(gameObject, Order.research))
    //     {
    //         order.currentStep = Order.CurrentStep.Research;
    //         order.orderButtonIcon.GetComponent<OrderIcon>().ChangeCurrentActionText(order.currentStep);
    //         order.currentStepPanel = gameObject;
    //         duringPanel.SetActive(true);
    //         SetCurrentOrder(gameObject);
    //         SetWorkersStateIcon(Worker.Status.Busy);
    //         InstantiateWorkersInSecondWindow();
    //         SetResponsibleWorker();
    //         ActiveOrdersManager.singleton.ExecuteOrder(Order, gameObject);
    //         MakeEquipmentBusy();


    //         slots.HideDismissButtons();
    //         order.stateOfOrder = Order.StateOfOrder.InProcess;
    //         transform.parent.gameObject.SetActive(false);
    //         // OpenWindowsManager.singletone.ShowResearchAcceptMessage();
    //         // OpenWindowsManager.singletone.AddOrRemovePanelFromList(transform.parent.gameObject);
    //         DarkBackground.singletone.UnFadeBackground();
    //         setPanel.SetActive(false);
    //     }
    // }

    // public void ContinueOrder()
    // {
    //     if (IsHaveEquipment(order.research) && WorkersIsSet(gameObject, order.research))
    //     {
    //         SetCurrentOrder(gameObject);
    //         SetWorkersStateIcon(Worker.Status.Busy);
    //         SetResponsibleWorker();
    //         ActiveOrdersManager.singleton.ExecuteOrder(Order, gameObject);
    //         MakeEquipmentBusy();
    //         // SetWorkersStateIcon(Worker.Status.Busy);
    //         slots.HideDismissButtons();
    //         order.stateOfOrder = Order.StateOfOrder.InProcess;
    //         pauseButton.SetActive(true);
    //         continueButton.SetActive(false);
    //     }
    // }

    // //Паузим проект
    // public void PauseOrder()
    // {
    //     Order.stateOfOrder = Order.StateOfOrder.Paused;
    //     ActiveOrdersManager.singleton.PauseOrder(Order, gameObject);

    //     //Указываем новое время
    //     Order.research.leadTime = ((1 - loadBarImage.GetComponent<Image>().fillAmount) * Order.research.leadTime);

    //     DeleteWorkersInSecondWindow();
    //     slots.ShowDismissButtons();
    //     ActiveOrdersManager.singleton.ClearCurrentOrders(gameObject);
    //     SetWorkersStateIcon(Worker.Status.Free);
    //     ResetResponsibility();
    //     MakeEquipmentFree();
    //     transform.parent.GetComponent<OrderScript>().assignedEmployees.Clear();
    //     transform.parent.GetComponent<OrderScript>().boughtedEmployees.Clear();

    //     continueButton.SetActive(true);
    //     pauseButton.SetActive(false);
    // }

    // //Удаляем иконки рабочих во втором окне заказа
    // private void DeleteWorkersInSecondWindow()
    // {
    //     for (int i = 0; i < order.research.requirementsForEmployees.Count; i++)
    //     {
    //         transform.GetChild(1).GetChild(0).GetChild(i).GetComponent<WorkerSlot>().isBusy = false;
    //         Destroy(transform.GetChild(1).GetChild(0).GetChild(i).GetChild(0).gameObject);
    //     }
    // }

    // //Создаём иконки рабочих во втором окне заказа
    // private void InstantiateWorkersInSecondWindow()
    // {
    //     int assingedWorkersCount = transform.parent.GetComponent<OrderScript>().assignedEmployees.Count;

    //     for (int i = 0; i < assingedWorkersCount; i++)
    //     {
    //         GameObject oldWorker = setPanel.transform.GetChild(0).GetChild(i).GetChild(0).gameObject;
    //         GameObject newWorker = Instantiate(oldWorker, transform.GetChild(1).GetChild(0).GetChild(i).transform);
    //         transform.GetChild(1).GetChild(0).GetChild(i).GetComponent<WorkerSlot>().isBusy = true;
    //         newWorker.transform.SetAsFirstSibling();
    //         newWorker.GetComponent<WorkerScript>().Worker.status = Worker.Status.Busy;
    //     }
    // }

    // //Устанавливаем цвет индикаторов для работников 
    // public void SetWorkersStateIcon(Worker.Status status)
    // {
    //     foreach (GameObject employer in transform.parent.GetComponent<OrderScript>().assignedEmployees)
    //     {
    //         WorkerScript employerWorkerScript = employer.GetComponent<WorkerScript>();
    //         employerWorkerScript.ChangeStateImage(status);
    //     }

    //     foreach (GameObject employer in transform.parent.GetComponent<OrderScript>().boughtedEmployees)
    //     {
    //         WorkerScript employerWorkerScript = employer.GetComponent<WorkerScript>();
    //         employerWorkerScript.ChangeStateImage(status);
    //     }
    // }

    // public void SetResponsibleWorker()
    // {
    //     Worker firstSlotWorker = duringPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<WorkerScript>().Worker;
    //     List<GameObject> boughtedWorkersList = transform.parent.GetComponent<OrderScript>().boughtedEmployees;
    //     GameObject workerStatsPanel = GameObject.Find("StatsPanel(Clone)");

    //     for (int i = 0; i < boughtedWorkersList.Count; i++)
    //     {
    //         if (boughtedWorkersList[i].GetComponent<WorkerScript>().Worker.workerIndex == firstSlotWorker.workerIndex)
    //         {
    //             transform.parent.GetComponent<OrderScript>().boughtedEmployees[i].GetComponent<WorkerScript>().Worker.responsibility = Worker.Responsibility.Responsible;
    //             break;
    //         }
    //     }

    //     //Если открыта панель с работником и если открыта панель ответственного работника, спавним там звёздочку
    //     if (workerStatsPanel)
    //     {
    //         WorkerStatsPanel workerStatsPanelScript = workerStatsPanel.GetComponent<WorkerStatsPanel>();
    //         if (workerStatsPanelScript.Worker.workerIndex == firstSlotWorker.workerIndex)
    //         {
    //             workerStatsPanelScript.responsibilityStar.gameObject.SetActive(true);
    //         }
    //     }
    // }

    // public void ResetResponsibility()
    // {
    //     if (transform.parent.GetComponent<OrderScript>().assignedEmployees.Count != 0)
    //     {

    //         transform.parent.GetComponent<OrderScript>().assignedEmployees[0].GetComponent<WorkerScript>().ChangeResponsibility(Worker.Responsibility.Helper);
    //         transform.parent.GetComponent<OrderScript>().boughtedEmployees[0].GetComponent<WorkerScript>().ChangeResponsibility(Worker.Responsibility.Helper);

    //         WorkerScript assignedWorkerScript = transform.parent.GetComponent<OrderScript>().assignedEmployees[0].GetComponent<WorkerScript>();

    //         GameObject workerStatsPanel = GameObject.Find("StatsPanel(Clone)");
    //         if (workerStatsPanel)
    //         {
    //             WorkerStatsPanel workerStatsPanelScript = workerStatsPanel.GetComponent<WorkerStatsPanel>();
    //             if (workerStatsPanelScript.Worker.workerIndex == assignedWorkerScript.Worker.workerIndex)
    //             {
    //                 workerStatsPanelScript.responsibilityStar.gameObject.SetActive(false);
    //             }
    //         }
    //     }
    // }

    // //Для каждого работника указываем его текущий проект
    // public void SetCurrentOrder(GameObject panel)
    // {
    //     WorkersManager manager = GameObject.FindObjectOfType<WorkersManager>();
    //     for (int j = 0; j < panel.transform.parent.GetComponent<OrderScript>().assignedEmployees.Count; j++)
    //     {
    //         for (int i = 0; i < manager.workers.Count; i++)
    //         {
    //             if (manager.workers[i].description == panel.transform.parent.GetComponent<OrderScript>().assignedEmployees[j].GetComponent<WorkerScript>().Worker.description)
    //             {
    //                 //Указываем что ребята в панели и просто купленные теперь заняты
    //                 manager.workers[i].status = Worker.Status.Busy;
    //                 panel.transform.parent.GetComponent<OrderScript>().assignedEmployees[j].GetComponent<WorkerScript>().Worker.status = Worker.Status.Busy;
    //                 manager.workers[i].currentOrder = panel.transform.parent.GetComponent<OrderScript>().Order;
    //             }
    //         }
    //     }
    // }

    // //Кнопка "Отложить"
    // public void HideOrderSteps()
    // {
    //     transform.parent.gameObject.SetActive(false);
    //     DarkBackground.singletone.UnFadeBackground();
    //     // OpenWindowsManager.singletone.AddOrRemovePanelFromList(transform.parent.gameObject);
    // }

    // //Проверяет, совпадает ли количество установленных в панель рабочих с необходимым количеством
    // public bool WorkersIsSet(GameObject stepPanel, Research research)
    // {
    //     return (stepPanel.transform.parent.GetComponent<OrderScript>().assignedEmployees.Count == research.requirementsForEmployees.Count);
    // }

    // //Проверяет какое оборудование нужно для текущего заказа и возвращает true, если есть свободное
    // public bool IsHaveEquipment(Research research)
    // {
    //     for (int i = 0; i < Enum.GetNames(typeof(Building.Type)).Length; i++)
    //     {
    //         if (research.requirementEquipmentList.Contains((Building.Type)i))
    //         {
    //             GameObject newEquipment = ShopEquipmentManager.singleton.availableEquipment.Find(x => x.GetComponent<EquipmentInfo>().equipmentObject.equipmentType == (Building.Type)i);
    //             if (newEquipment == null)
    //             {
    //                 return false;
    //             }
    //         }
    //     }
    //     return true;
    // }
}
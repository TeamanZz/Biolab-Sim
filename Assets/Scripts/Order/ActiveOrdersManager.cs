using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActiveOrdersManager : MonoBehaviour
{
    public Data data;
    public static ActiveOrdersManager singleton { get; private set; }

    public List<Order> launchedOrders = new List<Order>();
    public List<Coroutine> launchedCoroutines = new List<Coroutine>();

    public GameObject orderMessagesPanel;
    public GameObject appliedOrdersPanel;

    private void Awake()
    {
        singleton = this;
    }

    //Запускаем заказ
    public void ExecuteOrder(Order order, GameObject orderPanel)
    {
        launchedCoroutines.Add(StartCoroutine(CompleteTheOrder(order, orderPanel)));
        launchedOrders.Add(order);
        LoadBarChanger.singleton.StartFillingLoadBars(orderPanel.transform.parent.GetComponent<OrderScript>());
    }

    //Ждём X времени и начисляем голду, очищаем заказ у работников и увеличиваем количество завершённых заказов
    public IEnumerator CompleteTheOrder(Order order, GameObject panel)
    {
        OrderStage orderPanel = panel.GetComponent<OrderStage>();

        switch (order.currentStep)
        {
            case Order.CurrentStep.Research:
                yield return new WaitForSeconds(order.research.leadTime);
                break;
            case Order.CurrentStep.Development:
                yield return new WaitForSeconds(order.development.leadTime);
                break;
            case Order.CurrentStep.Testing:
                yield return new WaitForSeconds(order.testing.leadTime);
                break;
        }

        data.currencyData.moneyCount += order.reward;
        order.stateOfOrder = Order.StateOfOrder.Paused;

        orderPanel.SetWorkersStateIcon(Worker.Status.Free);
        orderPanel.ResetResponsibility();

        order.currentStep = Order.CurrentStep.Done;
        order.orderButtonIcon.GetComponent<OrderIcon>().ChangeCurrentActionText(order.currentStep);
        orderPanel.SpawnEquipmentSuccessIcon();
        order.orderStepsPanel.GetComponent<OrderScript>().UpdateInfoAboutWorkers();
        orderPanel.MakeEquipmentFree();
        orderPanel.SpawnChoosePanels();
        orderPanel.SetStepCompleted();
        orderPanel.endPanel.SetActive(true);
        orderPanel.duringPanel.SetActive(false);
    }

    //Паузим заказ
    public void PauseOrder(Order order, GameObject orderPanel)
    {
        //Индекс заказа, по которому останавливаем корутины
        int pauseIndex = launchedOrders.FindIndex(x => x.orderDescription == order.orderDescription);

        //Останавливаем выполнение корутины и удаляем корутины из списка запущенных корутин
        StopCoroutine(launchedCoroutines[pauseIndex]);
        launchedCoroutines.RemoveAt(pauseIndex);
        launchedOrders.RemoveAt(pauseIndex);
        //Останавливаем лоад бары
        LoadBarChanger.singleton.StopFillingLoadBars(orderPanel.transform.parent.GetComponent<OrderScript>());
    }

    //Очищаем у исполнителей currentOrder и увеличиваем количество выполненных ими проектов на 1. Выполняется в конце проекта
    public void Award(GameObject panel)
    {
        for (int j = 0; j < panel.GetComponent<OrderScript>().assignedEmployees.Count; j++)
        {
            for (int i = 0; i < GetComponent<WorkersManager>().workers.Count; i++)
            {
                if (GetComponent<WorkersManager>().workers[i].description == panel.GetComponent<OrderScript>().assignedEmployees[j].GetComponent<WorkerScript>().Worker.description)
                {
                    GetComponent<WorkersManager>().workers[i].completedOrdersCount++;
                    GetComponent<WorkersManager>().workers[i].currentOrder = null;
                }
            }
        }
    }

    //Очищает поле текущего проекта у работников. Нужно для паузы.
    public void ClearCurrentOrders(GameObject orderPanel)
    {
        for (int i = 0; i < GetComponent<WorkersManager>().workers.Count; i++)
        {
            for (int j = 0; j < orderPanel.transform.parent.GetComponent<OrderScript>().boughtedEmployees.Count; j++)
            {
                if (GetComponent<WorkersManager>().workers[i].description == orderPanel.transform.parent.GetComponent<OrderScript>().assignedEmployees[j].GetComponent<WorkerScript>().Worker.description)
                {
                    GetComponent<WorkersManager>().workers[i].currentOrder = null;
                    GetComponent<WorkersManager>().workers[i].status = Worker.Status.Free;
                }
            }
        }
    }

    public void MoveOrdersOnUIDown()
    {
        appliedOrdersPanel.GetComponent<RectTransform>().localPosition += new Vector3(0, -106, 0);
    }

    public void MoveOrdersOnUIUp()
    {
        appliedOrdersPanel.GetComponent<RectTransform>().localPosition += new Vector3(0, 106, 0);
    }
}

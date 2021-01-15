using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class OrdersManager : MonoBehaviour
{
    //Префабы сообщения о новом заказе и панели заказа
    public GameObject orderStepsPanelPrefab;
    public GameObject orderMessageButtonPrefab;
    [SerializeField] public List<Order> generalListOfOrders = new List<Order>();

    //Сюда кладём созданные панель и кнопку
    private GameObject newOrderStepsPanel;
    private GameObject newOrderMessageButton;
    private GameObject messagesPanel;
    private GameObject canvas;

    private void Awake()
    {
        messagesPanel = GameObject.FindGameObjectWithTag("MessagesPanel");
        canvas = GameObject.FindGameObjectWithTag("Canvas");
    }

    private void SpawnOrder(Order order)
    {
        newOrderMessageButton = Instantiate(orderMessageButtonPrefab, messagesPanel.transform);
        newOrderStepsPanel = Instantiate(orderStepsPanelPrefab, canvas.transform);

        newOrderStepsPanel.GetComponent<OrderDescriptionPanel>().Order = order;
        // newOrderStepsPanel.GetComponent<OrderDescriptionPanel>().Order.orderIcon = newOrderMessageButton;

        newOrderStepsPanel.GetComponent<OrderDescriptionPanel>().orderButton = newOrderMessageButton;
        newOrderStepsPanel.SetActive(false);

        newOrderMessageButton.GetComponent<NewOrderIcon>().Order = order;
        newOrderMessageButton.GetComponent<NewOrderIcon>().orderPanel = newOrderStepsPanel;
        //Сдвигаем вниз активные заказы
        ActiveOrdersManager.singleton.MoveOrdersOnUIDown();
    }

    public void CheckOrdersOnSpawn(int hours, int days)
    {
        foreach (Order order in generalListOfOrders)
        {
            if (order.orderDaysSpawnTime == days && order.orderHoursSpawnTime == hours)
            {
                SpawnOrder(order);
            }
        }
    }
}
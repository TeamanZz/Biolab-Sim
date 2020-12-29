using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Скрипт висит на окне, которое открывается после нажатия на новый заказ (Окно "принять" "отклонить")
public class OrderDescriptionPanel : MonoBehaviour
{
    public GameObject orderButton;
    public GameObject orderSteps;
    public GameObject orderIcon;

    [Header("Объекты для заполнения информацией")]
    public Image iconImg;
    public Image customerTypeImg;
    public TextMeshProUGUI orderHeadingText;
    public TextMeshProUGUI customerNameText;
    public TextMeshProUGUI scopeOfDevelopmentText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI developmentRewardText;

    [Space]
    [SerializeField]
    private Order order;
    private GameObject activeOrdersPanel;

    private void Start()
    {
        activeOrdersPanel = GameObject.FindGameObjectWithTag("ActiveOrdersPanel");
    }

    public Order Order
    {
        set
        {
            order = value;
            iconImg.sprite = order.iconImg;
            customerTypeImg.sprite = order.customerTypeImg;
            orderHeadingText.text = order.orderHeading;
            customerNameText.text = order.customer;
            scopeOfDevelopmentText.text = order.developmentSphere.ToString();
            descriptionText.text = order.orderDescription;
            developmentRewardText.text = "$ " + order.reward.ToString();
        }

        get { return order; }
    }

    //При нажатии на кнопку "принять заказ" закрывает текущее окно и создаёт иконку активного задания слева.
    public void AcceptOrder()
    {
        GameObject orderStep = Instantiate(orderSteps);
        orderStep.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        orderStep.GetComponent<OrderScript>().Order = order;


        GameObject newOrderIcon = Instantiate(orderIcon, activeOrdersPanel.transform, false);
        newOrderIcon.GetComponent<OrderIcon>().stepsPanel = orderStep;
        newOrderIcon.GetComponent<OrderIcon>().Order = Order;
        orderStep.GetComponent<OrderScript>().Order.orderButtonIcon = newOrderIcon;

        // OpenWindowsManager.singletone.iconsList.Remove(gameObject);
        orderStep.SetActive(false);
        // OpenWindowsManager.singletone.AddOrRemovePanelFromList(orderStep);

        OpenWindowsManager.singletone.ShowOrderAcceptMessage();
        //Сдвигаем вверх активные заказы
        ActiveOrdersManager.singleton.MoveOrdersOnUIUp();
        Destroy(orderButton);
        Destroy(gameObject);
    }

    //При нажатии на кнопку "Отменить заказ" удаляет текущее окно и уведомление о заказе
    public void CancelOrder()
    {
        OpenWindowsManager.singletone.iconsList.Remove(gameObject);
        Destroy(orderButton);
        Destroy(gameObject);
    }

    public void HidePanelWithCross()
    {
        OpenWindowsManager.singletone.iconsList.Remove(gameObject);
        gameObject.SetActive(false);
    }
}

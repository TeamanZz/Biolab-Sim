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
    public TextMeshProUGUI developmentTime;

    [Space]
    [SerializeField]
    private Order order;
    private GameObject activeOrdersPanel;
    private GameObject manager;
    private Data data;
    private GameObject canvas;

    private void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("Manager");
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        data = manager.GetComponent<ActiveOrdersManager>().data;

    }

    private void Start()
    {
        activeOrdersPanel = GameObject.FindGameObjectWithTag("ActiveOrdersPanel");
    }

    public Order Order
    {
        set
        {
            order = value;
            iconImg.sprite = data.ordersData.orderIconsImages[(int)order.developmentSphere];
            customerTypeImg.sprite = data.ordersData.orderCustomerTypeImages[(int)order.customerType];
            orderHeadingText.text = order.orderHeading;
            customerNameText.text = order.customer;
            scopeOfDevelopmentText.text = order.developmentSphere.ToString();
            descriptionText.text = order.orderDescription;
            developmentRewardText.text = "$ " + order.reward.ToString();
            developmentTime.text = order.totalTime.ToString() + " часов";
        }

        get { return order; }
    }

    //При нажатии на кнопку "принять заказ" закрывает текущее окно и создаёт иконку активного задания слева.
    public void AcceptOrder()
    {
        GameObject orderStep = Instantiate(orderSteps, canvas.transform);
        orderStep.GetComponent<OrderScript>().Order = order;

        GameObject newOrderIcon = Instantiate(orderIcon, activeOrdersPanel.transform, false);
        newOrderIcon.GetComponent<OrderIcon>().stepsPanel = orderStep;
        newOrderIcon.GetComponent<OrderIcon>().Order = Order;
        orderStep.GetComponent<OrderScript>().Order.orderButtonIcon = newOrderIcon;

        // OpenWindowsManager.singletone.iconsList.Remove(gameObject);
        // OpenWindowsManager.singletone.AddOrRemovePanelFromList(orderStep);

        // OpenWindowsManager.singletone.ShowOrderAcceptMessage();
        DarkBackground.singletone.staffPanel.transform.SetAsLastSibling();
        //Сдвигаем вверх активные заказы
        ActiveOrdersManager.singleton.MoveOrdersOnUIUp();
        Destroy(orderButton);
        Destroy(gameObject);
    }

    //При нажатии на кнопку "Отменить заказ" удаляет текущее окно и уведомление о заказе
    public void CancelOrder()
    {
        OpenWindowsManager.singletone.iconsList.Remove(gameObject);
        DarkBackground.singletone.UnFadeBackground();
        Destroy(orderButton);
        Destroy(gameObject);
    }

    public void HidePanelWithCross()
    {
        OpenWindowsManager.singletone.iconsList.Remove(gameObject);
        DarkBackground.singletone.UnFadeBackground();
        gameObject.SetActive(false);
    }
}

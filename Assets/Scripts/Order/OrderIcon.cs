using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderIcon : MonoBehaviour
{
    public Data data;
    public Image loadBar;
    public Image icon;
    public Image customerType;
    [HideInInspector] public GameObject stepsPanel;

    [SerializeField] private Order order;

    public Order Order
    {
        set
        {
            order = value;
            customerType.sprite = data.ordersData.orderCustomerTypeFrames[(int)order.customerType + 1];
            ChooseIcon();
        }

        get { return order; }
    }

    private void Start()
    {
        AddLoadBarImageInArray();
    }

    //Действие сокрытия для кнопки заказа слева вверху
    public void ShowHideSteps()
    {
        stepsPanel.SetActive(!stepsPanel.activeSelf);

        // OpenWindowsManager.singletone.AddOrRemovePanelFromList(stepsPanel);
    }

    //Выбирает значок в зависимости от типа исследования
    private void ChooseIcon()
    {
        icon.sprite = data.ordersData.orderIconsImages[(int)order.developmentSphere];
    }

    //Добавляем лоадбар с иконки в список лоадбаров, для заполнения
    private void AddLoadBarImageInArray()
    {
        stepsPanel.GetComponent<OrderScript>().loadBarImages.Add(loadBar);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderIcon : MonoBehaviour
{
    public Data data;
    public Image loadBarImage;
    public Image icon;
    [HideInInspector] public GameObject stepsPanel;

    [SerializeField] private Order order;

    public Order Order
    {
        set
        {
            order = value;
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
        switch (order.developmentSphere)
        {
            case Order.DevelopmentSphere.Medicine:
                icon.sprite = data.ordersData.orderIconsImages[0];
                break;
            case Order.DevelopmentSphere.Chemistry:
                icon.sprite = data.ordersData.orderIconsImages[1];
                break;
        }
    }

    //Добавляем лоадбар с иконки в список лоадбаров, для заполнения
    private void AddLoadBarImageInArray()
    {
        stepsPanel.GetComponent<OrderScript>().loadBarImages.Add(loadBarImage);
    }
}

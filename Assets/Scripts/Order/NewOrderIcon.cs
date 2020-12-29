using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

//Скрипт висит на активном задании слева
public class NewOrderIcon : MonoBehaviour
{
    public TextMeshProUGUI customerNameText;
    public GameObject orderPanel;

    [SerializeField]
    private Order order;

    //Заполняется поле order.
    public Order Order
    {
        set
        {
            order = value;
            customerNameText.text = "От: " + order.customer;
        }
    }

    //Скрыть/показать панель заказа
    public void ShowHideOrder()
    {
        orderPanel.SetActive(!orderPanel.activeSelf);
        OpenWindowsManager.singletone.AddOrRemovePanelFromList(orderPanel);
    }
}

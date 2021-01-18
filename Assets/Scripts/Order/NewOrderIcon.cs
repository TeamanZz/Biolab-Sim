using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Скрипт висит на активном задании слева
public class NewOrderIcon : MonoBehaviour
{
    public TextMeshProUGUI customerNameText;

    [HideInInspector] public GameObject orderPanel;

    [SerializeField]
    private Order order;

    private Data data;

    //Заполняется поле order.
    public Order Order
    {
        set
        {
            order = value;
            data = GameObject.Find("Manager").GetComponent<ActiveOrdersManager>().data;
            customerNameText.text = order.customer;
            GetComponent<Image>().sprite = data.ordersData.orderCustomerTypeFrames[(int)order.customerType - 1];
        }
    }

    private void Start()
    {
        if (order.timeToDestroy != 0)
        {
            StartCoroutine(IDestroyOrderAfterTime());
        }
    }

    //timeToDestroy выражается в часах.
    private IEnumerator IDestroyOrderAfterTime()
    {
        float seconds = order.timeToDestroy * 6 * TimePanel.singleton.param;
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }

    //Скрыть/показать панель заказа
    public void ShowHideOrder()
    {
        DarkBackground.singletone.FadeBackground(orderPanel);

        orderPanel.SetActive(!orderPanel.activeSelf);
        // OpenWindowsManager.singletone.AddOrRemovePanelFromList(orderPanel);
    }
}

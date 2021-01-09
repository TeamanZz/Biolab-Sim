using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class EquipmentSlotInOrder : MonoBehaviour
{
    public Data data;

    private EquipmentObject slot;
    private BuildingsGrid grid;
    private GameObject stepsPanel;
    public GameObject buyButton;

    [SerializeField]
    public EquipmentObject EquipmentSlot
    {
        set { slot = value; }
        get { return slot; }
    }

    private void Awake()
    {
        //Кешируем объекты
        stepsPanel = transform.parent.parent.parent.parent.gameObject;
        grid = GameObject.FindObjectOfType<BuildingsGrid>();
    }

    private void Start()
    {
    }

    private void FixedUpdate()
    {
        SetButton();
    }

    //Покупаем оборудование из окна заказа
    public void BuyEquipment()
    {
        grid.InstantiateFlyingBuilding(EquipmentSlot.instantiatedObject.GetComponent<Building>(), stepsPanel);
        // OpenWindowsManager.singletone.iconsList.Remove(stepsPanel);
        stepsPanel.SetActive(false);
    }

    private void SetButton()
    {
        if (stepsPanel.GetComponent<OrderScript>().Order.stateOfOrder == Order.StateOfOrder.Paused)
        {
            for (int i = 0; i < Enum.GetNames(typeof(Building.Type)).Length; i++)
            {
                if (EquipmentSlot.instantiatedObject.GetComponent<EquipmentInfo>().equipmentObject.equipmentType == (Building.Type)i)
                {
                    bool flag = true;
                    if (ShopEquipmentManager.singleton.availableEquipment.Find(x => x.GetComponent<EquipmentInfo>().equipmentObject.equipmentType == (Building.Type)i))
                    {
                        flag = false;
                    }
                    //Если есть свободные столы, скрываем кнопку. Если нет - то показываем.
                    buyButton.SetActive(flag);
                }
            }
        }
        else
        {
            buyButton.SetActive(false);
        }
    }
}

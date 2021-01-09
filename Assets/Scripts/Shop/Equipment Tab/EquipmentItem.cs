using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class EquipmentItem : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;
    public Image slotImage;

    [SerializeField]
    private EquipmentObject equipmentObject;
    private BuildingsGrid grid;

    public EquipmentObject EquipmentSlot
    {
        set
        {
            equipmentObject = value;

            //Изменяем реальную стоимость предмета на ту, которую указали в менеджере
            equipmentObject.instantiatedObject.GetComponent<Building>().buildingCost = equipmentObject.itemCost;
            nameText.text = equipmentObject.instantiatedObject.GetComponent<EquipmentInfo>().equipmentObject.itemName;

            costText.text = equipmentObject.itemCost.ToString() + " $";
            slotImage.sprite = equipmentObject.itemImage;
        }
        get { return equipmentObject; }
    }

    //Кешируем grid
    private void Awake()
    {
        grid = GameObject.FindObjectOfType<BuildingsGrid>();
    }

    public void OpenItemDescription()
    {
        Shop.singletone.ToggleDescriptionPanel();
        DescriptionTab.singletone.clickedButton = gameObject;
        DescriptionTab.singletone.Reagent = null;
        DescriptionTab.singletone.EquipmentSlot = EquipmentSlot;
    }

    //Создаём объект, который будем ставить
    public void BuyEquipment()
    {
        grid.InstantiateFlyingBuilding(equipmentObject.instantiatedObject.GetComponent<Building>());
        transform.parent.parent.parent.GetChild(2).gameObject.SetActive(false);
    }
}

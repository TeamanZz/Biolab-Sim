using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DescriptionTab : MonoBehaviour
{
    public static DescriptionTab singletone { get; private set; }

    [HideInInspector] public GameObject clickedButton;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemCost;
    public TextMeshProUGUI itemDescription;
    public Image itemImage;

    private Reagent reagent;
    private EquipmentObject equipmentSlot;

    public Reagent Reagent
    {
        get
        {
            return reagent;
        }
        set
        {
            reagent = value;
            if (reagent != null)
            {
                itemImage.sprite = reagent.image;
                itemName.text = reagent.name;
                itemCost.text = "Цена: " + reagent.cost.ToString() + "$";
                itemDescription.text = reagent.description;
            }
        }
    }

    public EquipmentObject EquipmentSlot
    {
        get
        {
            return equipmentSlot;
        }
        set
        {
            equipmentSlot = value;
            if (equipmentSlot != null)
            {
                itemImage.sprite = equipmentSlot.itemImage;

                itemName.text = equipmentSlot.instantiatedObject.GetComponent<EquipmentInfo>().equipmentObject.itemName;
                itemCost.text = "Цена: " + equipmentSlot.itemCost.ToString() + "$";
                itemDescription.text = equipmentSlot.itemDescription;
            }
        }
    }

    private void Awake()
    {
        singletone = this;
    }

    public void BuyItem()
    {
        //В зависимости от типа кликнутого предмета, запускаем его функцию покупки
        if (clickedButton.GetComponent<EquipmentItem>() != null)
            clickedButton.GetComponent<EquipmentItem>().BuyEquipment();
        else if (clickedButton.GetComponent<ReagentItem>() != null)
            clickedButton.GetComponent<ReagentItem>().BuyReagent();
    }

    //Кнопка закрытия панели с описанием товара
    public void CloseDescriptionPanel()
    {
        GameObject descriptionPanel = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
        descriptionPanel.SetActive(false);
    }
}

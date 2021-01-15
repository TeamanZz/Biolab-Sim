using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class ReagentItem : MonoBehaviour
{
    [SerializeField]
    [HideInInspector] private Reagent reagent;

    public Data data;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;
    public Image slotImage;

    public Reagent Reagent
    {
        set
        {
            reagent = value;
            nameText.text = reagent.name;
            costText.text = reagent.cost.ToString();
            slotImage.sprite = reagent.image;
        }
        get
        {
            return reagent;
        }
    }

    public void OpenItemDescription()
    {
        Shop.singletone.ToggleDescriptionPanel();
        DescriptionTab.singletone.clickedButton = gameObject;
        DescriptionTab.singletone.EquipmentSlot = null;
        DescriptionTab.singletone.Reagent = reagent;
    }

    public void BuyReagent()
    {
        if (isEnoughMoney(reagent.cost))
        {
            switch (reagent.reagentType)
            {
                case Reagent.ReagentType.Na:
                    data.currencyData.reagentsListData[0].count++;

                    break;
                case Reagent.ReagentType.Chlor:
                    data.currencyData.reagentsListData[1].count++;

                    break;
            }
            data.currencyData.moneyCount -= reagent.cost;
        }
    }

    private bool isEnoughMoney(int cost)
    {
        return (cost <= data.currencyData.moneyCount);
    }
}

[Serializable]
public class Reagent
{
    public string name;
    public string description;
    public int cost;
    public Sprite image;
    public ReagentType reagentType;

    public enum ReagentType
    {
        Na,
        Chlor
    }
}

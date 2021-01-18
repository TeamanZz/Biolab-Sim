using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShopEquipmentManager : MonoBehaviour
{
    public static ShopEquipmentManager singleton { get; private set; }
    public Data data;

    public GameObject equipmentSlotPrefab;
    public GameObject equipmentContentPanel;


    public GameObject reagentSlotPrefab;
    public GameObject reagentsContentPanel;

    public GameObject equipmentContainer;

    [SerializeField] public List<GameObject> listOfEquipmentItems = new List<GameObject>();
    [SerializeField] public List<Reagent> listOfReagents = new List<Reagent>();

    public List<GameObject> availableEquipment = new List<GameObject>();
    public List<GameObject> busyEquipment = new List<GameObject>();

    private void Awake()
    {
        singleton = this;
        AddPlacedEquipmentToArray();

    }

    //Кладём в массив доступных предметов те, что находятся на поле при старте игры
    private void AddPlacedEquipmentToArray()
    {
        for (int i = 0; i < equipmentContainer.transform.childCount; i++)
        {
            availableEquipment.Add(equipmentContainer.transform.GetChild(i).gameObject);
        }
    }

    private void Start()
    {
        //Спавним слоты с предметами в магазине
        foreach (GameObject equipmentItem in listOfEquipmentItems)
        {
            EquipmentObject slot = equipmentItem.GetComponent<EquipmentInfo>().equipmentObject;
            GameObject newEquipmentItem = Instantiate(equipmentSlotPrefab, equipmentContentPanel.transform);
            newEquipmentItem.GetComponent<EquipmentItem>().EquipmentSlot = slot;
        }
        //Спавним слоты с реагентами в магазине
        for (int i = 0; i < listOfReagents.Count; i++)
        {
            listOfReagents[i] = data.currencyData.reagentsListData.Find(x => x.reagentType == listOfReagents[i].reagentType);
            GameObject newReagentItem = Instantiate(reagentSlotPrefab, reagentsContentPanel.transform);
            newReagentItem.GetComponent<ReagentItem>().Reagent = listOfReagents[i];
        }
    }
}
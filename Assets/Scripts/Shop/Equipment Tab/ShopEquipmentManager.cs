using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShopEquipmentManager : MonoBehaviour
{
    public static ShopEquipmentManager singleton { get; private set; }

    public GameObject equipmentSlotPrefab;
    public GameObject equipmentContentPanel;

    public GameObject reagentSlotPrefab;
    public GameObject reagentsContentPanel;

    [SerializeField] public List<GameObject> listOfEquipmentItems = new List<GameObject>();
    [SerializeField] public List<Reagent> listOfreagents = new List<Reagent>();

    public List<GameObject> availableEquipment = new List<GameObject>();
    public List<GameObject> busyEquipment = new List<GameObject>();

    private void Awake()
    {
        singleton = this;
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
        foreach (Reagent reagent in listOfreagents)
        {
            GameObject newReagentItem = Instantiate(reagentSlotPrefab, reagentsContentPanel.transform);
            newReagentItem.GetComponent<ReagentItem>().Reagent = reagent;
        }
    }
}

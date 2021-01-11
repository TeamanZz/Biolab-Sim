using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class SlotsInOrder : MonoBehaviour
{
    public GameObject equipmentIconPrefab;
    public GameObject workerIconPrefab;

    private Data data;
    private int workersSlotsCount;

    //КОГДА БУДЕШЬ ДОБАВЛЯТЬ КРОМЕ РЕСЁРЧА, ДЕЛАЙ ПЕРЕГРУЗКУ

    private void Awake()
    {
        data = ActiveOrdersManager.singleton.data;
    }

    //Спавним слоты в окне заказа
    public void SpawnEquipmentSlots(Research research)
    {
        for (int i = 0; i < Enum.GetNames(typeof(Building.Type)).Length; i++)
        {
            if (research.requirementEquipmentList.Contains((Building.Type)i))
            {
                for (int j = 0; j < 2; j++)
                {
                    GameObject newEquipmentSlot = Instantiate(equipmentIconPrefab, transform.GetChild(j).GetChild(1).gameObject.transform);
                    //Передаём значение слота из магазина в слот заказа, для возможности купить объект из окна заказа
                    GameObject equipmentObject = ShopEquipmentManager.singleton.listOfEquipmentItems.Find(x => x.GetComponent<EquipmentInfo>().equipmentObject.equipmentType == (Building.Type)i);
                    newEquipmentSlot.GetComponent<EquipmentSlotInOrder>().EquipmentSlot = equipmentObject.GetComponent<EquipmentInfo>().equipmentObject;
                    newEquipmentSlot.GetComponent<Image>().sprite = equipmentObject.GetComponent<EquipmentInfo>().equipmentObject.itemImage;
                }
            }
        }
    }

    public void SpawnWorkersSlots(int countSlotsOfWorkers)
    {
        Order order = GetComponent<ResearchPanelScript>().Order;

        for (int k = 0; k < 2; k++)
        {
            for (int i = 0; i < countSlotsOfWorkers; i++)
            {
                //2 потому что 2 раза спавним слоты. В Set и during.
                GameObject newWorkerSlot = Instantiate(workerIconPrefab, transform.GetChild(k).GetChild(0).gameObject.transform);
                if (i == 0)
                {
                    newWorkerSlot.GetComponent<WorkerSlot>().slotResponsibility = Worker.Responsibility.Responsible;
                }
                else
                {
                    newWorkerSlot.GetComponent<WorkerSlot>().slotResponsibility = Worker.Responsibility.Helper;
                }

                newWorkerSlot.GetComponent<WorkerSlot>().requirements = order.research.requirementsForEmployees[i];

                workersSlotsCount++;
            }
        }
        workersSlotsCount /= 2;
    }

    public void HideDismissButtons()
    {

        for (int i = 0; i < workersSlotsCount; i++)
        {
            GameObject dismissButton = transform.GetChild(1).GetChild(0).GetChild(i).GetChild(1).gameObject;
            dismissButton.SetActive(false);
        }
    }

    public void ShowDismissButtons()
    {
        for (int i = 0; i < workersSlotsCount; i++)
        {
            GameObject dismissButton = transform.GetChild(1).GetChild(0).GetChild(i).GetChild(1).gameObject;
            dismissButton.SetActive(true);
        }
    }
}

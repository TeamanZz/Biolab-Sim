using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotsInOrder : MonoBehaviour
{

    public GameObject equipmentIconPrefab;
    public GameObject workerIconPrefab;

    private Data data;
    //
    //Добавь список и крестики
    //КОГДА БУДЕШЬ ДОБАВЛЯТЬ КРОМЕ РЕСЁРЧА, ДЕЛАЙ ПЕРЕГРУЗКУ

    private void Awake()
    {
        data = ActiveOrdersManager.singleton.data;
    }
    public void SpawnEquipmentSlots(Research research)
    {
        if (research.needReagentTable)
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject newEquipmentSlot = Instantiate(equipmentIconPrefab, transform.GetChild(i).GetChild(1).gameObject.transform);
                newEquipmentSlot.GetComponent<Image>().sprite = data.equipmentData.equipmentImagesList[0];
                //Передаём значение слота из магазина в слот заказа, для возможности купить объект из окна заказа
                GameObject equipmentObject = ShopEquipmentManager.singleton.listOfEquipmentItems.Find(x => x.GetComponent<EquipmentInfo>().equipmentObject.instantiatedObject.name == "Table");
                newEquipmentSlot.GetComponent<EquipmentSlotInOrder>().EquipmentSlot = equipmentObject.GetComponent<EquipmentInfo>().equipmentObject;
            }
        }

        if (research.needCapsule)
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject newEquipmentSlot = Instantiate(equipmentIconPrefab, transform.GetChild(i).GetChild(1).gameObject.transform);
                newEquipmentSlot.GetComponent<Image>().sprite = data.equipmentData.equipmentImagesList[1];
                //Передаём значение слота из магазина в слот заказа, для возможности купить объект из окна заказа
                // newEquipmentSlot.GetComponent<EquipmentSlotInOrder>().EquipmentSlot = ShopEquipmentManager.singleton.listOfEquipmentItems.Find(x => x.instantiatedObject.name == "Table");
            }
        }
    }

    public void SpawnWorkersSlots(int countSlotsOfWorkers)
    {
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
                    newWorkerSlot.GetComponent<WorkerSlot>().slotResponsibility = Worker.Responsibility.Helper;

            }
        }
    }

    public void HideCrosses()
    {
        for (int i = 0; i < GetComponent<ResearchPanelScript>().Order.research.neededWorkers; i++)
        {
            transform.GetChild(1).GetChild(0).GetChild(i).GetChild(1).gameObject.SetActive(false);
        }
    }

    public void ShowCrosses()
    {
        for (int i = 0; i < transform.GetChild(0).GetChild(0).childCount; i++)
        {
            transform.GetChild(1).GetChild(0).GetChild(i).GetChild(1).gameObject.SetActive(true);
        }
    }
}

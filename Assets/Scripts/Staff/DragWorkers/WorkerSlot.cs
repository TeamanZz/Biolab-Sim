using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class WorkerSlot : MonoBehaviour, IDropHandler
{
    public bool isBusy;
    public TextMeshProUGUI responsibilityText;
    public Worker.Responsibility slotResponsibility;
    public RequirementsForEmployees requirements;

    private GameObject parentStepsPanel;
    private GameObject manager;
    private GameObject slotImage;
    private Data data;

    private void Awake()
    {
        parentStepsPanel = transform.parent.parent.parent.parent.gameObject;
        manager = GameObject.FindGameObjectWithTag("Manager");
        data = manager.GetComponent<ActiveOrdersManager>().data;
    }

    //Спавним в ячейке купленного персонажа
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && isBusy == false)
        {
            if (!CheckOnRepeat(eventData) && CheckWorkerCompetitions(eventData))
            {
                DestroyWorkerIconOnDrag();

                //Создаём работника в слоте
                GameObject newWorker = Instantiate(eventData.pointerDrag, GetComponent<RectTransform>().transform, true);
                newWorker.transform.SetParent(GetComponent<RectTransform>().transform);
                newWorker.transform.SetAsFirstSibling();

                slotImage = newWorker.transform.parent.GetChild(1).gameObject;
                //Нужно чтобы по персонажу в слоте нельзя было кликнуть и вызвать инфу о нём
                newWorker.GetComponent<CanvasGroup>().blocksRaycasts = false;
                newWorker.GetComponent<WorkerScript>().Worker.currentOrder = parentStepsPanel.GetComponent<OrderScript>().Order;
                newWorker.GetComponent<WorkerScript>().Worker.orderStepsPanel = parentStepsPanel;
                newWorker.GetComponent<RectTransform>().sizeDelta = new Vector3(100f, 147f, 0);
                newWorker.GetComponent<RectTransform>().position = slotImage.transform.position;
                eventData.pointerDrag.GetComponent<WorkerScript>().Worker.workerSlotContainer = gameObject;
                slotImage.SetActive(false);
                transform.GetChild(2).gameObject.SetActive(true);
                isBusy = true;

                if (slotResponsibility == Worker.Responsibility.Responsible)
                {
                    newWorker.GetComponent<WorkerScript>().stateFrame.sprite = data.workerData.workerFrames[4];
                }

                //Добавляем работников в списки
                parentStepsPanel.GetComponent<OrderScript>().assignedEmployees.Add(newWorker);
                parentStepsPanel.GetComponent<OrderScript>().boughtedEmployees.Add(eventData.pointerDrag);
                eventData.pointerDrag.GetComponent<WorkerScript>().Worker.orderStepsPanel = parentStepsPanel;
            }
        }
    }

    public bool CheckWorkerCompetitions(PointerEventData eventData)
    {
        if ((int)requirements.minProfession > (int)eventData.pointerDrag.GetComponent<WorkerScript>().Worker.profession)
        {
            return false;
        }

        if (requirements.specialization != eventData.pointerDrag.GetComponent<WorkerScript>().Worker.specialization[0]
        && eventData.pointerDrag.GetComponent<WorkerScript>().Worker.specialization.Count > 1
        && requirements.specialization != eventData.pointerDrag.GetComponent<WorkerScript>().Worker.specialization[1])
        {
            return false;
        }

        return true;
    }

    //Удаляем персонажа с иконки с помощью крестика
    public void DestroyWorkerIcon()
    {
        if (isBusy)
        {
            Destroy(transform.GetChild(0).gameObject);
            slotImage.SetActive(true);
            DeleteEmployersFromLists();

            isBusy = false;
            transform.GetChild(2).gameObject.SetActive(false);
        }
    }

    public void DismissWorker()
    {
        Destroy(transform.GetChild(0).gameObject);
        isBusy = false;
    }

    //Удаляет работника из списков Employers
    private void DeleteEmployersFromLists()
    {
        foreach (GameObject employer in parentStepsPanel.GetComponent<OrderScript>().assignedEmployees.ToArray())
        {
            if (employer != null)
            {
                if (employer.GetComponent<WorkerScript>().Worker.description == transform.GetChild(0).GetComponent<WorkerScript>().Worker.description)
                {
                    parentStepsPanel.GetComponent<OrderScript>().assignedEmployees.Remove(employer);
                }
            }
        }

        foreach (GameObject employer in parentStepsPanel.GetComponent<OrderScript>().boughtedEmployees.ToArray())
        {
            if (employer != null)
            {
                if (employer.GetComponent<WorkerScript>().Worker.description == transform.GetChild(0).GetComponent<WorkerScript>().Worker.description)
                {
                    parentStepsPanel.GetComponent<OrderScript>().boughtedEmployees.Remove(employer);
                }
            }
        }
    }

    //Удаляем персонажа с иконки при наложении одного персонажа на другого при перетаскивании
    public void DestroyWorkerIconOnDrag()
    {
        if (isBusy)
        {
            Destroy(transform.GetChild(0).gameObject);

            DeleteEmployersFromLists();
        }
    }

    //Возвращаем True если в слотах уже есть перс с таким же описанием
    private bool CheckOnRepeat(PointerEventData eventData)
    {
        //идём по слотам
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if (transform.parent.GetChild(i) != gameObject)
            {
                if (transform.parent.GetChild(i).childCount >= 2 && transform.parent.GetChild(i).GetChild(0).GetComponent<WorkerScript>() != null)
                {
                    if (eventData.pointerDrag.GetComponent<WorkerScript>().Worker.description == transform.parent.GetChild(i).GetChild(0).GetComponent<WorkerScript>().Worker.description)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}


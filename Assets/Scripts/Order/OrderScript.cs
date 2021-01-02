﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class OrderScript : MonoBehaviour
{
    public GameObject researchStagePrefab;
    public List<Image> loadBarImages = new List<Image>();
    public List<Coroutine> activeLoadBarCoroutines = new List<Coroutine>();
    public List<GameObject> assignedEmployees = new List<GameObject>();
    public List<GameObject> boughtedEmployees = new List<GameObject>();
    public float orderFillAmount;
    public float remainingOrderTime;
    [SerializeField] public Order order;

    public Order Order
    {
        set { order = value; }
        get { return order; }
    }

    private void Start()
    {
        GameObject newResearchPanel = Instantiate(researchStagePrefab, gameObject.transform);
        newResearchPanel.transform.SetAsFirstSibling();
        newResearchPanel.GetComponent<ResearchPanelScript>().Order = order;
        order.orderStepsPanel = gameObject;
        remainingOrderTime = order.research.leadTime;
    }

    public void AddLoadBarImageInArray(Image loadBarImage)
    {
        loadBarImages.Add(loadBarImage);
    }

    public void CloseOrderSteps()
    {
        gameObject.SetActive(false);
        // OpenWindowsManager.singletone.AddOrRemovePanelFromList(gameObject);
    }

    public void DismissWorker(Worker dismissedWorker)
    {
        int WorkersCount = assignedEmployees.Count;
        //Удаляем объект из массива и с экрана
        for (int i = 0; i < WorkersCount; i++)
        {
            if (dismissedWorker.description == assignedEmployees[i].GetComponent<WorkerScript>().Worker.description)
            {
                Destroy(boughtedEmployees[i]);
                boughtedEmployees.Remove(boughtedEmployees[i]);

                assignedEmployees[i].gameObject.transform.parent.GetComponent<WorkerSlot>().isBusy = false;
                Destroy(assignedEmployees[i]);
                assignedEmployees.Remove(assignedEmployees[i]);
                break;
            }
        }
    }

    public void UpdateInfoAboutWorkers()
    {
        GameObject workerStatsPanelGameObject = GameObject.Find("StatsPanel(Clone)");
        Worker statsPanelWorker = new Worker();
        if (workerStatsPanelGameObject != null)
        {
            statsPanelWorker = workerStatsPanelGameObject.GetComponent<WorkerStatsPanel>().Worker;
        }

        //Обнуляем CurrentOrder у купленных работников и говорим что их выполненные заказы+1
        ActiveOrdersManager.singleton.Award(gameObject);

        //Изменияем рисунки состояния на зелёные(свободные)
        foreach (GameObject worker in boughtedEmployees)
        {
            WorkerScript workerScript = worker.GetComponent<WorkerScript>();
            workerScript.Worker.status = Worker.Status.Free;
            workerScript.Worker.orderStepsPanel = null;

            if (workerStatsPanelGameObject != null && statsPanelWorker.workerIndex == workerScript.Worker.workerIndex)
            {
                GameObject.Find("StatsPanel(Clone)").GetComponent<WorkerStatsPanel>().Worker = workerScript.Worker;
            }

        }
        //Очищаем массивы с работниками
        assignedEmployees.Clear();
        boughtedEmployees.Clear();
    }
}

[Serializable]
public class Order
{
    [HideInInspector] public GameObject orderStepsPanel;
    [HideInInspector] public GameObject orderButtonIcon;
    [HideInInspector] public GameObject currentStepPanel;
    public DevelopmentSphere developmentSphere;
    public CustomerType customerType;
    public StateOfOrder stateOfOrder = StateOfOrder.Paused;
    public CurrentStep currentStep;
    public String customer;
    public String orderHeading;
    public String orderDescription;
    public int reward;
    public bool developmentStage;
    public bool TestingStage;

    public enum DevelopmentSphere
    {
        Bacteriology,
        Genetics,
        Bioengineering,
        MedicalEngineering,
        Neurobiology,
        Virology
    }
    public enum CustomerType
    {
        Clear,
        Anonymous,
        CorporationFirst,
        CorporationSecond,
        CorporationThird,
        Government,
        Private
    }

    public enum StateOfOrder
    {
        Paused,
        InProcess
    }

    public enum CurrentStep
    {
        Done,
        Research,
        Development,
        Testing
    }

    //Этапы разработки заказа
    [SerializeField] public Research research;
    [SerializeField] public Development development;
    [SerializeField] public Testing testing;
}

[Serializable]
public class Research
{
    public string additionalText;
    public string taskText;
    public float leadTime;

    public int neededWorkers;
    public bool needReagentTable;
    public bool needCapsule;

    public List<GameObject> usedEquipment = new List<GameObject>();
}

[Serializable]
public class Development
{
    public String name;
    public bool needReagentTable;
    public bool needCapsule;

}

[Serializable]
public class Testing
{
    public String name;
    public bool needReagentTable;
    public bool needCapsule;

}
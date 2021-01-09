using System.Collections;
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

    [Header("Descriptions")]
    public String customer;
    public String orderHeading;
    public String orderDescription;
    public int reward;

    [Header("Types")]
    public DevelopmentSphere developmentSphere;
    public CustomerType customerType;

    [Header("Timings")]
    public int orderHoursSpawnTime;
    public int orderDaysSpawnTime;
    public int timeToDestroy;

    [Header("States")]
    public StateOfOrder stateOfOrder = StateOfOrder.Paused;
    public CurrentStep currentStep;

    [Header("Required Stages")]
    public bool researchStage;
    public bool developmentStage;
    public bool TestingStage;

    [Header("Stages")]
    [SerializeField] public Research research;
    [SerializeField] public Development development;
    [SerializeField] public Testing testing;

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
}


[Serializable]
public class RequirementsForEmployees
{
    [SerializeField]
    public Worker.Profession minProfession;
    [SerializeField]
    public Worker.Specialization specialization;
}

[Serializable]
public class Results
{
    string mainText;
    string description;
}

[Serializable]
public class Research
{
    [Header("Description")]
    public string additionalText;
    public string taskText;
    public float leadTime;

    [Header("Workers Requierments")]
    public List<RequirementsForEmployees> requirementsForEmployees = new List<RequirementsForEmployees>();

    [Header("Equipment")]
    public List<Building.Type> requirementEquipmentList = new List<Building.Type>();

    [Header("Bought Research")]
    bool canBuyResearch;

    [Header("Acceleration")]
    int accelerationHoursTime;
    int accelerationCost;

    [Header("Mini Games")]
    public Minigames minigame;

    [HideInInspector]
    public List<GameObject> usedEquipment = new List<GameObject>();
}

[Serializable]
public class Development
{
    public String mainText;
    public bool needReagentTable;
    public bool needCapsule;
    public float leadTime;

    public List<Results> results = new List<Results>();

}

[Serializable]
public class Testing
{
    public String name;
    public bool needReagentTable;
    public bool needCapsule;
    public float leadTime;

}

public enum Minigames
{
    None
}
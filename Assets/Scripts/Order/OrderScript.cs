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

    public void ReleaseWorkers()
    {
        //Обнуляем CurrentOrder у купленных работников и говорим что их выполненные заказы+1
        ActiveOrdersManager.singleton.Award(gameObject);

        //Изменияем рисунки состояния на зелёные(свободные)
        foreach (GameObject worker in boughtedEmployees)
        {
            worker.GetComponent<WorkerScript>().Worker.status = Worker.Status.Free;
            // worker.GetComponent<WorkerScript>().ChangeStateImage();
            worker.GetComponent<WorkerScript>().Worker.orderStepsPanel = null;
        }
        //Очищаем массивы с работниками
        assignedEmployees.Clear();
        boughtedEmployees.Clear();
    }
}

[Serializable]
public class Order
{
    public GameObject orderStepsPanel;
    public GameObject orderButtonIcon;
    public Sprite customerTypeImg;
    public Sprite iconImg;
    public String customer;
    public String orderHeading;
    public String orderDescription;
    public DevelopmentSphere developmentSphere;
    public StateOfOrder stateOfOrder = StateOfOrder.Paused;
    public GameObject currentStepPanel;
    public CurrentStep currentStep;
    public int reward;
    public bool developmentStage;
    public bool TestingStage;

    public enum DevelopmentSphere
    {
        Chemistry,
        Medicine
    }

    public enum StateOfOrder
    {
        Paused,
        InProcess
    }

    public enum CurrentStep
    {
        None,
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
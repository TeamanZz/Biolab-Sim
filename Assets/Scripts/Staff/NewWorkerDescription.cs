using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewWorkerDescription : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    [HideInInspector] public GameObject availableWorker;

    [SerializeField] private Worker worker;
    private GameObject manager;

    public Worker Worker
    {
        set
        {
            worker = value;
            descriptionText.text = worker.description;
            image.sprite = worker.photo;
        }
        get
        {
            return worker;
        }
    }

    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Manager");
    }

    //Уничтожает работника из панели покупки работников и создаёт в нижней
    public void BuyNewWorker()
    {
        Destroy(availableWorker);
        manager.GetComponent<WorkersManager>().BuyWorker(worker);
    }

    public void CloseWindow()
    {
        gameObject.SetActive(false);
    }
}

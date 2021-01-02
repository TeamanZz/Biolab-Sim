using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewWorkerDescription : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI nameBoxText;
    public TextMeshProUGUI fullName;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI professionText;
    public TextMeshProUGUI educationText;
    public TextMeshProUGUI specializationText;
    public TextMeshProUGUI salary;
    [HideInInspector] public GameObject availableWorker;

    [SerializeField] private Worker worker;
    private GameObject manager;

    public Worker Worker
    {
        set
        {
            worker = value;
            image.sprite = worker.photo;
            nameBoxText.text = worker.name;
            fullName.text = worker.fullName;
            descriptionText.text = worker.description;
            professionText.text = worker.profession.ToString();
            educationText.text = worker.education.ToString();
            specializationText.text = worker.specialization.ToString();
            salary.text = "$ " + worker.salary.ToString();
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

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

            specializationText.text = worker.specialization[0].ToString();
            if (worker.specialization.Count > 1)
            {
                specializationText.text += " и " + worker.specialization[1].ToString();
            }

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
        if (!GameObject.FindGameObjectWithTag("OrderStepsPanel"))
        {
            DarkBackground.singletone.UnFadeBackground();
        }
        else
        {
            DarkBackground.singletone.darkPanel.transform.SetSiblingIndex(DarkBackground.singletone.darkPanel.transform.GetSiblingIndex() - 2);
        }
    }

    public void CloseWindow()
    {
        gameObject.SetActive(false);
    }
}

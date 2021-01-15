using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChooseNextStep : MonoBehaviour
{
    private OrderChooseStep orderChooseStep;
    public Data data;
    [HideInInspector] public Order order;

    public Image image;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI term;
    public TextMeshProUGUI sphere;
    public GameObject acceptButton;

    //В Awake подписываемся на OnClick Event для кнопки в зависимости от действия/триггера. Пишем все действия в этот скрипт.
    public OrderChooseStep OrderChooseStep
    {
        get { return orderChooseStep; }
        set
        {
            orderChooseStep = value;
            image.sprite = data.ordersData.orderIconsImages[(int)orderChooseStep.developmentSphere];
            nameText.text = orderChooseStep.nameText;
            descriptionText.text = orderChooseStep.descriptionText;
            term.text = orderChooseStep.daysTerm + " Дней и " + orderChooseStep.hoursTerm + " часов";
            sphere.text = orderChooseStep.developmentSphere.ToString();

        }
    }

    private void Awake()
    {
        acceptButton.GetComponent<Button>().onClick.AddListener(DestroySuccessIcon);
    }

    private void DestroySuccessIcon()
    {

    }

    public void GoToNextStep()
    {
        order.orderStepsPanel.transform.GetChild(1).gameObject.SetActive(true);
        LoadBarChanger.singleton.ResetLoadBars(order.currentStepPanel.transform.parent.GetComponent<OrderScript>());
        Destroy(order.currentStepPanel.gameObject);
    }
}

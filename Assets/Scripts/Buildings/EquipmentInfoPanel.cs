using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class EquipmentInfoPanel : MonoBehaviour, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI equipmentState;
    public GameObject stopButton;
    public Image loadBarImage;
    public Order currentOrder;
    public GameObject item;

    private bool mouseIsOver = false;

    private void FixedUpdate()
    {
        nameText.text = item.GetComponent<EquipmentInfo>().equipmentObject.itemName;
        if (currentOrder != null && currentOrder.orderHeading != "")
        {
            loadBarImage.fillAmount = currentOrder.orderStepsPanel.GetComponent<OrderScript>().orderFillAmount;
            equipmentState.text = "Режим: работает";
            stopButton.SetActive(true);
        }
        else
        {
            loadBarImage.fillAmount = 0;
            equipmentState.text = "Режим: простаивает";
            stopButton.SetActive(false);
        }
    }

    public void TogglePanel()
    {
        Destroy(gameObject);
    }

    public void StopProject()
    {
        currentOrder.currentStepPanel.GetComponent<OrderStage>().PauseOrder();
        Destroy(gameObject);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (!mouseIsOver)
        {
            Destroy(gameObject);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseIsOver = true;
        EventSystem.current.SetSelectedGameObject(gameObject);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseIsOver = false;
        EventSystem.current.SetSelectedGameObject(gameObject);

    }
}

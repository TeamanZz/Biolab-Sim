using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CupBoardScript : MonoBehaviour, IPointerDownHandler, IDeselectHandler, ISelectHandler
{
    public GameObject CupBoardPanel;

    public void OnDeselect(BaseEventData eventData)
    {
        if (CupBoardPanel != null)
            CupBoardPanel.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        CupBoardPanel.SetActive(!CupBoardPanel.activeSelf);
        CupBoardPanel.transform.position = Input.mousePosition;
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void OnSelect(BaseEventData eventData)
    {
        CupBoardPanel.SetActive(CupBoardPanel.activeSelf);
    }
}

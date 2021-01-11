using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CupBoardScript : MonoBehaviour, IPointerDownHandler
{
    public GameObject CupBoardPanel;



    public void OnPointerDown(PointerEventData eventData)
    {
        CupBoardPanel.SetActive(!CupBoardPanel.activeSelf);
        // EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void CloseRepository()
    {
        CupBoardPanel.SetActive(false);
    }

    // public void OnSelect(BaseEventData eventData)
    // {
    //     CupBoardPanel.SetActive(CupBoardPanel.activeSelf);
    // }

    // public void OnDeselect(BaseEventData eventData)
    // {
    //     if (CupBoardPanel != null)
    //         CupBoardPanel.SetActive(false);
    // }
}

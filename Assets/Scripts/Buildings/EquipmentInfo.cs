using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class EquipmentInfo : MonoBehaviour, IPointerDownHandler
{
    public GameObject infoPanelPrefab;
    // private GameObject infoPanel;
    private GameObject canvas;
    [HideInInspector] public Order currentOrder;
    public EquipmentObject equipmentObject;


    private void Awake()
    {
        equipmentObject.instantiatedObject = gameObject;
        canvas = GameObject.FindGameObjectWithTag("Canvas");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // if (!infoPanel)
        // {
        //     infoPanel = Instantiate(infoPanelPrefab, canvas.transform);
        //     infoPanel.transform.position = Input.mousePosition;
        //     EventSystem.current.SetSelectedGameObject(infoPanel);
        // }

    }

    private void FixedUpdate()
    {
        // if (infoPanel != null && infoPanel.GetComponent<EquipmentInfoPanel>().currentOrder != currentOrder)
        // {
        //     infoPanel.GetComponent<EquipmentInfoPanel>().currentOrder = currentOrder;
        //     infoPanel.GetComponent<EquipmentInfoPanel>().item = gameObject;

        // }
    }
}

[Serializable]
public class EquipmentObject
{
    public Sprite itemImage;
    public string itemName;
    public string itemDescription;
    public int itemCost;
    public GameObject instantiatedObject;
    public Building.Type equipmentType;
}

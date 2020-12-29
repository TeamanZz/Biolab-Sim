using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject shopPanel;
    public GameObject equipmentTab;
    public GameObject reagentsTab;
    public GameObject descriptionTab;

    private GameObject currentOpenedTab;

    //Скрываем/показываем панель магазина
    public void HideShopPanelWithCross()
    {
        if (descriptionTab.activeSelf == true && shopPanel.activeSelf == true)
        {
            descriptionTab.SetActive(false);
        }
        else
        {
            shopPanel.SetActive(!shopPanel.activeSelf);
            OpenWindowsManager.singletone.AddOrRemovePanelFromList(shopPanel);
            if (currentOpenedTab == null)
                OpenEquipmentTab();
        }
    }

    public void ShowHideShopPanel()
    {
        shopPanel.SetActive(!shopPanel.activeSelf);
        OpenWindowsManager.singletone.AddOrRemovePanelFromList(shopPanel);
        if (currentOpenedTab == null)
            OpenEquipmentTab();
    }

    public void OpenEquipmentTab()
    {
        if (descriptionTab.activeSelf == true)
            descriptionTab.SetActive(false);
        if (currentOpenedTab != null)
            currentOpenedTab.SetActive(false);
        equipmentTab.SetActive(true);
        currentOpenedTab = equipmentTab;
    }

    public void OpenReagentsTab()
    {
        if (descriptionTab.activeSelf == true)
            descriptionTab.SetActive(false);
        currentOpenedTab.SetActive(false);
        reagentsTab.SetActive(true);
        currentOpenedTab = reagentsTab;
    }

    public void OpenDescriptionTab()
    {
        currentOpenedTab.SetActive(false);
        descriptionTab.SetActive(true);
    }
}

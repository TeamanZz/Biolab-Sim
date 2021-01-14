using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddStaffButton : MonoBehaviour
{
    [HideInInspector] public GameObject descriptionPanel;

    public void TogglePanel(GameObject panel)
    {
        // OpenWindowsManager.singletone.AddOrRemovePanelFromList(panel);
        // if (panel.activeSelf)
        // {
        //     panel.transform.SetAsLastSibling();
        // }
        // DarkBackground.singletone.FadeBackground();

        panel.SetActive(!panel.activeSelf);

    }

    public void CloseAvailableWorkerPanel(GameObject panel)
    {
        // DarkBackground.singletone.UnFadeBackground();
        Destroy(descriptionPanel);
        panel.SetActive(false);
    }

}

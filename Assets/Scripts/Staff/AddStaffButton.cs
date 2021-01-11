using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddStaffButton : MonoBehaviour
{
    public void TogglePanel(GameObject panel)
    {
        panel.SetActive(!panel.activeSelf);
        OpenWindowsManager.singletone.AddOrRemovePanelFromList(panel);
        if (panel.activeSelf)
            panel.transform.SetAsLastSibling();
    }
}

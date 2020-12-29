using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OpenWindowsManager : MonoBehaviour
{
    public static OpenWindowsManager singletone { get; private set; }
    public List<GameObject> iconsList = new List<GameObject>();

    public GameObject darkPanel;

    public GameObject messagePanel;
    public TextMeshProUGUI messageText;

    private void Awake()
    {
        singletone = this;
    }

    public IEnumerator ICloseMessage()
    {
        yield return new WaitForSeconds(2);
        messagePanel.SetActive(false);
    }

    public void ShowOrderAcceptMessage()
    {
        messagePanel.SetActive(true);
        messageText.text = "Заказ принят!";
        StartCoroutine(ICloseMessage());
    }

    public void ShowResearchAcceptMessage()
    {
        messagePanel.SetActive(true);
        messageText.text = "Исследование запущено!";
        StartCoroutine(ICloseMessage());
    }

    public void AddOrRemovePanelFromList(GameObject panel)
    {
        if (panel != null)
        {
            if (panel.activeSelf == true)
                OpenWindowsManager.singletone.iconsList.Add(panel);
            else
                OpenWindowsManager.singletone.iconsList.Remove(panel);
        }
    }

    private void Update()
    {
        ActivateDarkPanel();
    }

    private void ActivateDarkPanel()
    {
        if (iconsList.Count > 0)
            darkPanel.SetActive(true);
        else
            darkPanel.SetActive(false);
    }

    public void HidePanelWithCross(GameObject panelToHide)
    {
        OpenWindowsManager.singletone.iconsList.Remove(panelToHide);
        if (gameObject.GetComponent<WorkersManager>().infoAboutNewWorkerPanel != null && gameObject.GetComponent<WorkersManager>().infoAboutNewWorkerPanel.gameObject.activeSelf == true)
            gameObject.GetComponent<WorkersManager>().infoAboutNewWorkerPanel.SetActive(!gameObject.GetComponent<WorkersManager>().infoAboutNewWorkerPanel);
        panelToHide.SetActive(false);
    }
}

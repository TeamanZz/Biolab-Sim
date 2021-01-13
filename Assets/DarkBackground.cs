using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkBackground : MonoBehaviour
{
    public static DarkBackground singletone { get; private set; }

    public GameObject darkPanel;

    public GameObject staffPanel;


    private void Awake()
    {
        singletone = this;
    }

    public void FadeBackground()
    {
        darkPanel.SetActive(true);
        darkPanel.transform.SetSiblingIndex(transform.parent.childCount - 2);
    }

    public void FadeBackground(GameObject panel)
    {
        if (!panel.activeSelf)
        {
            FadeBackground();
        }
        else
        {
            UnFadeBackground();
        }
    }

    public void UnFadeBackground()
    {
        darkPanel.transform.SetSiblingIndex(transform.parent.childCount + 2);
        darkPanel.SetActive(false);
    }
}

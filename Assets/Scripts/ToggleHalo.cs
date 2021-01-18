using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToggleHalo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject haloOutline;
    public GameObject haloTooth;

    public void OnPointerEnter(PointerEventData eventData)
    {
        haloOutline.SetActive(true);
        haloTooth.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        haloOutline.SetActive(false);
        haloTooth.SetActive(false);
    }
}


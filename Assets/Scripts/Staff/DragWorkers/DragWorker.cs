using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragWorker : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    private GameObject copyOfCurrentWorker;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Data data;

    private void Awake()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
        data = GameObject.FindGameObjectWithTag("Manager").GetComponent<WorkersManager>().data;
    }

    //Спавним копию рабочего в мыши, для видимости перетаскивания
    public void OnBeginDrag(PointerEventData eventData)
    {
        WorkerScript workerScript = eventData.pointerDrag.gameObject.GetComponent<WorkerScript>();
        if (workerScript.Worker.status == Worker.Status.Free)
        {
            gameObject.GetComponent<Image>().sprite = data.workerData.workerFrames[0];
            copyOfCurrentWorker = Instantiate(gameObject, canvas.transform, true);
            rectTransform = copyOfCurrentWorker.GetComponent<RectTransform>();
            canvasGroup = copyOfCurrentWorker.GetComponent<CanvasGroup>();
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = .7f;
        }
        else
            eventData.pointerDrag = null;
    }

    //Магия перемещения
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    //Удаляем объект с мыши
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        Destroy(copyOfCurrentWorker);
    }
}

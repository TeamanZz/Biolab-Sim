using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadBarChanger : MonoBehaviour
{
    public static LoadBarChanger singleton { get; private set; }

    private void Awake()
    {
        singleton = this;
    }

    public void StartFillingLoadBars(OrderScript orderScript)
    {
        foreach (Image image in orderScript.loadBarImages)
        {
            orderScript.activeLoadBarCoroutines.Add(StartCoroutine(FillProgressBar(image, orderScript)));
        }
    }

    public void StopFillingLoadBars(OrderScript orderScript)
    {
        switch (orderScript.order.currentStep)
        {
            case Order.CurrentStep.Research:
                foreach (Coroutine coroutine in orderScript.activeLoadBarCoroutines)
                {
                    StopCoroutine(coroutine);
                }
                orderScript.activeLoadBarCoroutines.Clear();
                break;
        }
    }

    IEnumerator FillProgressBar(Image image, OrderScript orderScript)
    {
        switch (orderScript.order.currentStep)
        {
            case Order.CurrentStep.Research:
                while (image.fillAmount != 1)
                {
                    yield return new WaitForSeconds(0.1f);
                    image.fillAmount += (0.1f / orderScript.remainingResearchStageTime);

                    orderScript.orderFillAmount = image.fillAmount;
                }
                break;
        }

    }
}

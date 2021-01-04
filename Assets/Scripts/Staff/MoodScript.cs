using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoodScript : MonoBehaviour
{
    public Image moodImage;

    public void SetMoodImage(WorkerScript workerScript)
    {
        moodImage.sprite = workerScript.Worker.moodImage;
    }

    public void SetMoodImage(WorkerStatsPanel workerStatsPanel)
    {
        moodImage.sprite = workerStatsPanel.Worker.moodImage;
    }

    public void ChangeWorkerMood(Worker worker, Worker.Mood mood)
    {
        worker.mood = mood;
        switch (mood)
        {
            case Worker.Mood.Good:
                worker.moodImage = ActiveOrdersManager.singleton.data.equipmentData.workersMoodImages[0];
                break;
            case Worker.Mood.Frustrated:
                worker.moodImage = ActiveOrdersManager.singleton.data.equipmentData.workersMoodImages[1];
                break;
            case Worker.Mood.Bad:
                worker.moodImage = ActiveOrdersManager.singleton.data.equipmentData.workersMoodImages[2];
                break;
        }
    }
}

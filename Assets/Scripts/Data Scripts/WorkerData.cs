using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WorkerData : ScriptableObject
{
    public List<Sprite> workerFrames = new List<Sprite>();
    public List<Sprite> workerMoods = new List<Sprite>();
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EquipmentData : ScriptableObject
{
    //Изображения оборудования (0 - стол, 1 - капсула)
    public List<Sprite> equipmentImagesList = new List<Sprite>();

    public List<Sprite> workersMoodImages = new List<Sprite>();
}

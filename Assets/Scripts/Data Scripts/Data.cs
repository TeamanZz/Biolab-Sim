using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Data : ScriptableObject
{
    public CurrencyData currencyData;
    public EquipmentData equipmentData;
    public OrdersData ordersData;
    public WorkerData workerData;
}

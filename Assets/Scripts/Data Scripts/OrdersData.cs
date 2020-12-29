using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class OrdersData : ScriptableObject
{
    public List<Order> activeOrders = new List<Order>();
    public List<Sprite> orderIconsImages = new List<Sprite>();
}

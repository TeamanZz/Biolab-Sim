using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class OrdersData : ScriptableObject
{
    public List<Order> activeOrders = new List<Order>();

    public List<Sprite> orderIconsImages = new List<Sprite>();
    public List<Sprite> orderCustomerTypeImages = new List<Sprite>();
    public List<Sprite> orderCustomerTypeFrames = new List<Sprite>();
}

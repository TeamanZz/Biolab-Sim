using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CurrencyData : ScriptableObject
{
    public int moneyCount;

    [SerializeField]
    public List<RepositoryReagent> reagentsListData = new List<RepositoryReagent>();

}

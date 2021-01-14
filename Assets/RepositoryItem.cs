using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RepositoryItem : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI count;

    private RepositoryReagent reagent;

    public RepositoryReagent RepositoryReagent
    {
        set
        {
            reagent = value;
            image.sprite = reagent.image;
            itemName.text = reagent.name;
            count.text = reagent.count.ToString();
        }
        private get { return reagent; }

    }
}

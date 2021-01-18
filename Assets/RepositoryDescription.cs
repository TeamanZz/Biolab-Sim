using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RepositoryDescription : MonoBehaviour
{
    [HideInInspector] public GameObject clickedButton;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemCost;
    public TextMeshProUGUI itemDescription;
    public Image itemImage;

    private RepositoryReagent reagent;

    public void CloseDescriptionPanel()
    {
        gameObject.SetActive(false);
    }

    public RepositoryReagent RepositoryReagent
    {
        set
        {
            reagent = value;
            itemName.text = reagent.name;
            itemCost.text = "$ " + reagent.cost;
            itemDescription.text = reagent.description;
            itemImage.sprite = reagent.image;

        }
        get
        {
            return reagent;
        }
    }

}

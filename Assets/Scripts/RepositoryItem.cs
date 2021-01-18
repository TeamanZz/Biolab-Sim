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
    private GameObject descriptionPanel;

    public RepositoryReagent RepositoryReagent
    {
        set
        {
            reagent = value;
            image.sprite = reagent.image;
            itemName.text = reagent.name;
            count.text = reagent.count.ToString();
        }
        get { return reagent; }

    }

    private void Start()
    {
        descriptionPanel = transform.parent.GetComponentInParent<Repository>().descriptionPanel.gameObject;
    }

    public void ShowInfoAboutReagent()
    {
        if (!descriptionPanel.activeSelf)
        {
            descriptionPanel.GetComponent<RepositoryDescription>().RepositoryReagent = RepositoryReagent;
            descriptionPanel.SetActive(true);

        }
    }
}

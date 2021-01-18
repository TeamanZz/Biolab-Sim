using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repository : MonoBehaviour
{
    public static Repository singleton { private set; get; }

    public GameObject reagentPrefab;
    public GameObject reagentsContainer;
    public GameObject descriptionPanel;
    public Data data;

    [SerializeField]
    private List<RepositoryReagent> repositoryReagents = new List<RepositoryReagent>();

    private void Awake()
    {
        singleton = this;
    }

    private void OnEnable()
    {
        repositoryReagents = data.currencyData.reagentsListData;
        ShowReagents();
    }

    private void ShowReagents()
    {
        for (int i = 0; i < repositoryReagents.Count; i++)
        {
            if (!repositoryReagents[i].isInstantiated && repositoryReagents[i].count != 0)
            {
                GameObject newReagentItem = Instantiate(reagentPrefab, reagentsContainer.transform);
                newReagentItem.GetComponent<RepositoryItem>().RepositoryReagent = repositoryReagents[i];
                data.currencyData.reagentsListData[i].isInstantiated = true;
            }
            //Проверка на количество детей в контейнере. Без проверки out of bounds.
            else if (reagentsContainer.transform.childCount >= i + 1)
            {
                reagentsContainer.transform.GetChild(i).GetComponent<RepositoryItem>().RepositoryReagent = repositoryReagents[i];
            }

        }
    }
}

[System.Serializable]
public class RepositoryReagent : Reagent
{
    public bool isInstantiated;
    public int count;
}
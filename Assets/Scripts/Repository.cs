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
            //Проходимся по списку реагентов. Если не был создан и его количество не 0, создаём
            if (!repositoryReagents[i].isInstantiated && repositoryReagents[i].count != 0)
            {
                GameObject newReagentItem = Instantiate(reagentPrefab, reagentsContainer.transform);
                newReagentItem.GetComponent<RepositoryItem>().RepositoryReagent = repositoryReagents[i];
                data.currencyData.reagentsListData[i].isInstantiated = true;
            }
            //иначе обновляем данные
            else if (reagentsContainer.transform.childCount >= i + 1)
            {
                int itemId = reagentsContainer.transform.GetChild(i).GetComponent<RepositoryItem>().RepositoryReagent.reagentId;
                reagentsContainer.transform.GetChild(i).GetComponent<RepositoryItem>().RepositoryReagent = repositoryReagents.Find(x => x.reagentId == itemId);
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
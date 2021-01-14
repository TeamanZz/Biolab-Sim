using TMPro;
using UnityEngine;
public class ResourcesChanger : MonoBehaviour
{
    public Data data;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI chlorText;
    public TextMeshProUGUI naText;
    public int startMoneyCount;

    private void Start()
    {
        ResetResources();
    }

    private void FixedUpdate()
    {
        //Выводим на экран количество денег
        moneyText.text = data.currencyData.moneyCount.ToString();
    }

    //Проверка, достаточно ли денег для постройки этого здания
    public bool IsEnoughMoney(Building buildingprefab)
    {
        if (buildingprefab.buildingCost <= data.currencyData.moneyCount)
        {
            data.currencyData.moneyCount -= buildingprefab.buildingCost;
            return true;
        }
        return false;
    }

    private void ResetResources()
    {
        data.currencyData.moneyCount = startMoneyCount;
        foreach (RepositoryReagent reagent in data.currencyData.reagentsListData)
        {
            reagent.isInstantiated = false;
            reagent.count = 0;
        }
    }

    public void ExitFromGameButton()
    {
        Application.Quit();
    }
}

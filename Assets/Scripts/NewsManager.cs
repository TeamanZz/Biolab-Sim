using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewsManager : MonoBehaviour
{
    public static NewsManager singleton { get; private set; }
    public GameObject newsPrefab;
    public GameObject newsContainer;
    public GameObject newsButton;
    public List<Sprite> newsAvailableImage = new List<Sprite>();

    [SerializeField] public List<News> newsList = new List<News>();

    [HideInInspector] public GameObject actualNewsObject;

    private void Awake()
    {
        singleton = this;
    }

    private void SpawnNews()
    {
        foreach (News news in newsList)
        {
            actualNewsObject = Instantiate(newsPrefab, newsContainer.transform);
            actualNewsObject.GetComponent<NewsScript>().News = news;
        }
    }

    public void HideAllNewButtons()
    {
        for (int i = 0; i < newsContainer.transform.childCount; i++)
        {
            newsContainer.transform.GetChild(i).GetComponent<NewsScript>().notificationButton.SetActive(false);
            newsButton.GetComponent<Image>().sprite = newsAvailableImage[0];
        }
    }

    private void Start()
    {
        SpawnNews();
        newsButton.GetComponent<Image>().sprite = newsAvailableImage[1];
    }
}

[System.Serializable]
public class News
{
    public string mainName;
    public string content;
    public Sprite image;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewsScript : MonoBehaviour
{
    private News news;

    public Image image;
    public TextMeshProUGUI mainName;
    public TextMeshProUGUI content;
    public GameObject notificationButton;

    public News News
    {
        set
        {
            news = value;
            image.sprite = news.image;
            mainName.text = news.mainName;
            content.text = news.content;
        }
        get { return news; }
    }
}

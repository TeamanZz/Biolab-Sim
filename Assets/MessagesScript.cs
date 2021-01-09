using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessagesScript : MonoBehaviour
{
    public void TogglePanel()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

}
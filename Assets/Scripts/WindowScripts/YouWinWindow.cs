using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class YouWinWindow : MonoBehaviour
{
    public event EventHandler OnClicked;

    private void Awake()
    {

        transform.Find("YouWinBtn").GetComponent<Button_UI>().ClickFunc = () =>
        {
            OnClicked(this, EventArgs.Empty);

        };

    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class MinorAgeWindow : MonoBehaviour
{
    public event EventHandler OnClicked;

    private void Awake()
    {

        transform.Find("MinorAgeBtn").GetComponent<Button_UI>().ClickFunc = () =>
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

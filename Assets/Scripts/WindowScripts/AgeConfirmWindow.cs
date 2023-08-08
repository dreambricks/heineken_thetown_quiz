using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.UI;

public class AgeConfirmWindow : MonoBehaviour
{
    public Button yesButton;
    public Button noButton;

    [SerializeField] private MinorAgeWindow minorAgeWindow;
    [SerializeField] private RegisterWindow registerWindow;

    private void Start()
    {
        yesButton.onClick.AddListener(() => GoRegister());
        noButton.onClick.AddListener(() => GoMinorAge());
    }

    private void GoRegister()
    {
        registerWindow.Show();
        Hide();
    }

    private void GoMinorAge()
    {
        minorAgeWindow.Show();
        Hide();
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

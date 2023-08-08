using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TermsWindow : MonoBehaviour
{

    [SerializeField] private RegisterWindow registerWindow;

    public Button closeBtn;

    void Start()
    {
        closeBtn.onClick.AddListener(() => GoRegister());
    }

    private void GoRegister()
    {
        registerWindow.Show();
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

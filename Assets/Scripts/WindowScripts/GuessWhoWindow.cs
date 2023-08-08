using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuessWhoWindow : MonoBehaviour
{

    public Button startBtn;

    [SerializeField] private RegressiveCountWindow regressiveCountWindow;


    void Start()
    {
        startBtn.onClick.AddListener(() => GoRegressiveCountWindow());
    }

    private void GoRegressiveCountWindow()
    {
        regressiveCountWindow.Show();
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

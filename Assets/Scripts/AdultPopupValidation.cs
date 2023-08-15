using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdultPopupValidation : MonoBehaviour
{
    public Button popupBtn;
    void Start()
    {
        popupBtn.onClick.AddListener(() => closePopUp());
    }

    public void closePopUp()
    {
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

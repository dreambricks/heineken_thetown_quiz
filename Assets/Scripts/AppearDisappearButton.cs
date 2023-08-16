using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

public class AppearDisappearButton : MonoBehaviour
{
    public GameObject objectToShow;
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonPressed);
        objectToShow.SetActive(false);
    }


    private void OnButtonPressed()
    {
        objectToShow.SetActive(true);
    }
}

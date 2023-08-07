using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using TMPro;

public class YouLoseWindow : MonoBehaviour
{
    public event EventHandler OnClicked;

    public AudioSource audioSource;
    public AudioClip clip;

    public TextMeshProUGUI hitsText;
    public TextMeshProUGUI missText;

    [SerializeField] private Points points;
    [SerializeField] private PlayGame playGame;

    private void Awake()
    {

        transform.Find("YouLoseBtn").GetComponent<Button_UI>().ClickFunc = () =>
        {
            OnClicked(this, EventArgs.Empty);

        };

    }
    private void OnEnable()
    {
        audioSource.Stop();
        //StartCoroutine(PlaySound());
        hitsText.text = points.hits + " acertos";
        missText.text = points.miss + " erros";
        StartCoroutine(PlayLoop());
    }

    IEnumerator PlaySound()
    {
        audioSource.PlayOneShot(clip);
        yield return new WaitForSeconds(15);
        audioSource.Stop();
    }

    private void OnDisable()
    {
        points.Hide();
    }

    IEnumerator PlayLoop()
    {
        yield return new WaitForSeconds(20);

        playGame.Show();
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

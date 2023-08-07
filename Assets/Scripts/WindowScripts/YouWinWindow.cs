using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CodeMonkey.Utils;


public class YouWinWindow : MonoBehaviour
{
    public event EventHandler OnClicked;

    public AudioSource audioSource;
    public AudioClip soundWin;

    public TextMeshProUGUI hitsText;
    public TextMeshProUGUI missText;

    [SerializeField] private Points points;
    [SerializeField] private PlayGame playGame;

    private void Awake()
    {

        transform.Find("YouWinBtn").GetComponent<Button_UI>().ClickFunc = () =>
        {
            OnClicked(this, EventArgs.Empty);

        };

    }

    private void OnEnable()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        StartCoroutine(PlaySound());
        hitsText.text = points.hits + " acertos";
        missText.text = points.miss + " erros";
        StartCoroutine(PlayLoop());
    }

    private void OnDisable()
    {
        points.Hide();
    }


    IEnumerator PlaySound()
    {
        audioSource.PlayOneShot(soundWin);
        yield return new WaitForSeconds(15);
        audioSource.Stop();
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

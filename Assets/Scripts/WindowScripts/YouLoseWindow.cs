using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class YouLoseWindow : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip soundLose;

    public TextMeshProUGUI hitsText;
    public TextMeshProUGUI missText;
    public Button homeBtn;

    [SerializeField] private Points points;
    [SerializeField] private MainWindow mainWindow;


    private void Start()
    {
        homeBtn.onClick.AddListener(() => GoMainWindowNow());
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
        StartCoroutine(GoMainWindow());
    }

    private void OnDisable()
    {
        points.Hide();
    }

    private void GoMainWindowNow()
    {
        mainWindow.Show();
        audioSource.Stop();
        Hide();
    }

    IEnumerator PlaySound()
    {
        audioSource.PlayOneShot(soundLose);
        yield return new WaitForSeconds(15);
        audioSource.Stop();
    }

    IEnumerator GoMainWindow()
    {
        yield return new WaitForSeconds(20);

        mainWindow.Show();
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

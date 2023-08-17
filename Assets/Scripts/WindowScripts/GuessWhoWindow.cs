using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GuessWhoWindow : MonoBehaviour
{

    public Button startBtn;

    [SerializeField] private RegressiveCountWindow regressiveCountWindow;
    [SerializeField] private MainWindow mainWindow;

    public float totalTime = 40.0f;
    private float currentTime;


    void Start()
    {
        startBtn.onClick.AddListener(() => GoRegressiveCountWindow());
    }

    private void Update()
    {
        Countdown();
    }

    private void OnEnable()
    {
        currentTime = totalTime;
    }

    private void GoRegressiveCountWindow()
    {
        regressiveCountWindow.Show();
        Hide();
    }

    private void Countdown()
    {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            currentTime = 0;
            mainWindow.Show();
            SendLog();
            Hide();
        }
    }

    private void SendLog()
    {
        string folderPath = Path.Combine(Application.persistentDataPath, "data_logs");

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        DataLog data = new DataLog();

        data.barName = BarConfig.LoadBarName();
        data.status = StatusEnum.CadastroConcluidoNaoJogou.ToString();

        string formattedDateTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");

        data.timePlayed = formattedDateTime;


        string json = JsonUtility.ToJson(data);

        string fileName = string.Format("{0}_{1}_datalog.json", data.barName, data.timePlayed.Replace("-", "").Replace("T", "_").Replace(":", "").Replace("Z", ""));
        Debug.Log(fileName);

        string filePath = Path.Combine(folderPath, fileName);

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.Write(json);
        }

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

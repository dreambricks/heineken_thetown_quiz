using System;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class MinorAgeWindow : MonoBehaviour
{
    [SerializeField] private MainWindow mainWindow;
    public Button button;

    public float totalTime = 40.0f;
    private float currentTime;
  

    private void OnEnable()
    {
        currentTime = totalTime;
    }

    private void Start()
    {
        button.onClick.AddListener(() => GoMainWindow());
    }

    private void Update()
    {
        Countdown();
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

    private void GoMainWindow()
    {
        mainWindow.Show();
        SendLog();
        Hide();
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
        data.status = StatusEnum.MenorDeIdade.ToString();

        string formattedDateTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");

        data.timePlayed = formattedDateTime;


        string json = JsonUtility.ToJson(data);

        string fileName = string.Format("{0}_{1}_datalog.json", data.barName, data.timePlayed.Replace("-","").Replace("T","_").Replace(":","").Replace("Z",""));
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

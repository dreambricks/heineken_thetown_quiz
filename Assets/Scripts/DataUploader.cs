using System;
using System.Collections;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

public class DataUploader : MonoBehaviour
{
    private string outputFolder = Path.Combine(Application.streamingAssetsPath, "user_data");
    private string backupFolder = Path.Combine(Application.streamingAssetsPath, "user_data_backup");

    public string uploadURL;
    private string barName;
    public int checkIntervalSeconds;

    // Start is called before the first frame update
    void Start()
    {
        CheckIfDirectoryExists(outputFolder);
        CheckIfDirectoryExists(backupFolder);
        StartCoroutine(Worker());
    }

    IEnumerator Worker()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkIntervalSeconds);

            // check if internet is available
            if (!CheckForInternetConnection())
            {
                Debug.Log("no internet available");
                //lastCheck = DateTime.Now;
                continue;
            }

            // check if there are files to process
            // get a list of files in the output folder
            string[] fileEntries = Directory.GetFiles(outputFolder);
            if (fileEntries.Length == 0)
            {
                Debug.Log("no files to process");
                continue;
            }

            foreach (string filepath in fileEntries)
            {
                string filename = Path.GetFileName(filepath);
                Debug.Log(string.Format("processing file '{0}'", filename));
                yield return StartCoroutine(SendData(filename));
            }
        }
    }

    void BackupFile(string filename)
    {
        string from = Path.Combine(outputFolder, filename);
        string to = Path.Combine(backupFolder, filename);

        File.Move(from, to);
    }

    IEnumerator SendData(string filename)
    {

        barName = BarConfig.LoadBarName();

        // Crie um objeto WWWForm para armazenar o arquivo
        WWWForm form = new WWWForm();

        string fullPath = Path.Combine(outputFolder, filename);

        // Carregue o arquivo binario
        byte[] fileData = System.IO.File.ReadAllBytes(fullPath);
        form.AddBinaryData("file", fileData, filename);
        form.AddField("nomeBar", barName);

        // Crie uma requisicao UnityWebRequest para enviar o arquivo
        using (UnityWebRequest www = UnityWebRequest.Post(uploadURL, form))
        {
            yield return www.SendWebRequest(); // Envie a requisicao

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(string.Format("Arquivo '{0}' enviado com sucesso!", filename));
                BackupFile(filename);
            }
            else
            {
                Debug.Log(string.Format("Erro ao enviar o arquivo '{0}': {1}", filename, www.error));
            }
        }
    }

    public static bool CheckForInternetConnection(int timeoutMs = 2000)
    {
        try
        {
            string url =  "http://www.gstatic.com/generate_204";

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.KeepAlive = false;
            request.Timeout = timeoutMs;
            using (var response = (HttpWebResponse)request.GetResponse())
                return true;
        }
        catch
        {
            return false;
        }
    }

    public static void CheckIfDirectoryExists(string path)
    {
        bool exists = System.IO.Directory.Exists(path);

        if (!exists)
        {
            System.IO.Directory.CreateDirectory(path);
        }
    }
}

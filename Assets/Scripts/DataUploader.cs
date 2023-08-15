using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class DataUploader : MonoBehaviour
{
    public string outputFolder;
    public string backupFolder;
    public string uploadURL;
    public string barName;
    public int checkIntervalMs;
    private DateTime lastCheck;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsTimeToCheck())
        {
            return;
        }
    }

    bool IsTimeToCheck()
    {
        return ((TimeSpan)(DateTime.Now - lastCheck)).TotalMilliseconds >= checkIntervalMs;
    }

    void BackupFile(string filename)
    {
        string from = Path.Combine(outputFolder, filename);
        string to = Path.Combine(backupFolder, filename);

        File.Move(from, to);
    }

    IEnumerator SendData(string filename)
    {
        // Crie um objeto WWWForm para armazenar o arquivo
        WWWForm form = new WWWForm();

        // Carregue o arquivo binario
        byte[] fileData = System.IO.File.ReadAllBytes(Path.Combine(outputFolder, filename));
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
}

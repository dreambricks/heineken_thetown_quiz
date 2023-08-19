using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class FileDownloader : MonoBehaviour
{
    private string fileURL = "https://dreambricks.com.br/files/hnk/public_key.xml"; // Substitua pelo URL do arquivo desejado

    private void Start()
    {
        StartCoroutine(DownloadAndSaveFile());
    }

    private IEnumerator DownloadAndSaveFile()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(fileURL))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error while downloading: " + webRequest.error);
            }
            else
            {
                // Salvar o arquivo na pasta persistentDataPath
                string filePath = Path.Combine(Application.persistentDataPath, "keys");

                DataUploaderUtils.CheckIfDirectoryExists(filePath);

                string fileName = Path.GetFileName(fileURL);

                string fileoutput = Path.Combine(filePath, fileName);

                File.WriteAllBytes(fileoutput, webRequest.downloadHandler.data);

                Debug.Log("File downloaded and saved at: " + filePath);
            }
        }
    }
}

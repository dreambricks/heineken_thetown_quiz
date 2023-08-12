using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class TestDecrypt : MonoBehaviour
{

    public Text text;
    public TextAsset xmlAsset;

    public string filePath;

    private string xmlString;
    private string fileContent;

    // Start is called before the first frame update
    void Start()
    {

        if (xmlAsset != null)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlAsset.text);

            xmlString = xmlDoc.OuterXml;

            Debug.Log("XML as string:\n" + xmlString);
        }
        else
        {
            Debug.LogError("XML asset is missing!");
        }

  

        if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
        {
            fileContent = ReadFileToString(filePath);
            Debug.Log("File content as string:\n" + fileContent);
        }
        else
        {
            Debug.LogError("File path is invalid or file does not exist!");
        }

        text.text = RSAUtil.Decrypt(xmlString, fileContent);

    }

    private string ReadFileToString(string path)
    {
        byte[] fileBytes = File.ReadAllBytes(path);
        return Encoding.UTF8.GetString(fileBytes); // Use a codificação apropriada aqui
    }


}

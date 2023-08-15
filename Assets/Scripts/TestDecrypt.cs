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

    public string filePath;

    private string xmlString;
    private string fileContent;


    void Start()
    {

        XmlDocument xmlDoc = new XmlDocument();

        string xmlPath = Application.streamingAssetsPath + "/keys/private_key.xml";

        //xmlDoc.LoadXml(xmlAsset.text);
        xmlDoc.Load(xmlPath);

        xmlString = xmlDoc.OuterXml;



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
        return Encoding.UTF8.GetString(fileBytes);
    }


}

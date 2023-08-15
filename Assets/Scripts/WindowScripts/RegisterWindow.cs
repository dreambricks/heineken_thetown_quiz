using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RegisterWindow : MonoBehaviour
{

    public Button termsBtn;
    public Button policyBtn;
    public Button advanceBtn;

    public InputField nome;
    public InputField sobrenome;
    public InputField cpf;
    public InputField email;
    public InputField dataAniversario;
    public CheckButtonBehavior terms;
    public CheckButtonBehavior policy;

    [SerializeField] private TermsWindow termsWindow;
    [SerializeField] private PrivacyPolicyWindow privacyPolicyWindow;
    [SerializeField] private GuessWhoWindow guessWhoWindow;
    [SerializeField] private AlertPopup alertPopup;

    private string xmlString;
    private string folderOutput = Application.streamingAssetsPath;
    private string dataToEncrypt;
    private string fileName;
    private string stringEncrypted;


    private void Start()
    {
        termsBtn.onClick.AddListener(() => GoTerms());
        policyBtn.onClick.AddListener(() => GoPolicy());
        advanceBtn.onClick.AddListener(() => GoGuessWhoWindow());
    }

    private void OnEnable()
    {
        nome.text = "";
        sobrenome.text = "";
        cpf.text = "";
        email.text = "";
        dataAniversario.text = "";
    }

    private void GoTerms()
    {
        termsWindow.Show();
        Hide();
    }

    private void GoPolicy()
    {
        privacyPolicyWindow.Show();
        Hide();
    }

    private void GoGuessWhoWindow()
    {

        if (nome.text != "" && sobrenome.text != "" && cpf.text != "" && email.text != "" && dataAniversario.text != "" && terms.isChecked && policy.isChecked)
        {
            guessWhoWindow.Show();
            StartCoroutine(EncryptData());
            Hide();
        }
        else
        {
            alertPopup.Show();
        }
    }


    IEnumerator EncryptData()
    {
  
        XmlDocument xmlDoc = new XmlDocument();

        string xmlPath = Path.Combine(Application.streamingAssetsPath,"keys/public_key.xml");

        xmlDoc.Load(xmlPath);

        xmlString = xmlDoc.OuterXml;

        Debug.Log("XML as string:\n" + xmlString);
    

        dataToEncrypt = nome.text + "," + sobrenome.text + "," + cpf.text + "," + dataAniversario.text + "," + email.text + "\n";

        DateTime today = DateTime.Now;
        
        string formattedDate = today.ToString("yyyyMMdd HH:mm:ss");

        string formattedDateTime = FormatDateTimeString(formattedDate);


        stringEncrypted = RSAUtil.Encrypt(xmlString, dataToEncrypt);
        fileName = "player_data/" + "moesbar" + "_" + formattedDateTime + ".enc";

        string fullPath = Path.Combine(folderOutput, fileName);
        Debug.Log(fullPath);

        using (StreamWriter writer = new StreamWriter(fullPath))
        {
            writer.Write(stringEncrypted);
        }

        // Crie um objeto WWWForm para armazenar o arquivo
        WWWForm form = new WWWForm();

        // Carregue o arquivo bin�rio
        byte[] fileData = System.IO.File.ReadAllBytes(fullPath);
        form.AddBinaryData("file", fileData, fileName);
        form.AddField("nomeBar", "moes");

        // Crie uma requisi��o UnityWebRequest para enviar o arquivo
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:8080/api/players/upload", form))
        {
            yield return www.SendWebRequest(); // Envie a requisi��o

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Arquivo enviado com sucesso!");
            }
            else
            {
                Debug.Log("Erro ao enviar o arquivo: " + www.error);
            }
        }

    }

    private string FormatDateTimeString(string input)
    {
        DateTime parsedDateTime;
        if (DateTime.TryParseExact(input, "yyyyMMdd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out parsedDateTime))
        {
            string formatted = parsedDateTime.ToString("yyyyMMdd_HHmmss");
            return formatted;
        }
        else
        {
            Debug.LogError("Failed to parse input date and time.");
            return string.Empty;
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

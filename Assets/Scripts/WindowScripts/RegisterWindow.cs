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

    public TextAsset xmlAsset;
    private string xmlString;
    public string folderOutput;
    private string dataToEncrypt;
    private string fileName;
    private string stringEncrypted;

    public string barName;

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
            EncryptData();
            Hide();
        }
        else
        {
            alertPopup.Show();
        }
    }

    void EncryptData()
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

        dataToEncrypt = nome.text + "," + sobrenome.text + "," + cpf.text + "," + dataAniversario.text + "," + email.text + "\n";
        Debug.Log(dataToEncrypt);

        DateTime today = DateTime.Now;
        
        string formattedDate = today.ToString("yyyyMMdd HH:mm:ss");
        Debug.Log(formattedDate);
        string formattedDateTime = FormatDateTimeString(formattedDate);
        Debug.Log(formattedDateTime);

        stringEncrypted = RSAUtil.Encrypt(xmlString, dataToEncrypt);
        fileName = string.Format("{0}_{1}.enc", barName, formattedDateTime);

        string fullPath = Path.Combine(folderOutput, fileName);

        using (StreamWriter writer = new StreamWriter(fullPath))
        {
            writer.Write(stringEncrypted);
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

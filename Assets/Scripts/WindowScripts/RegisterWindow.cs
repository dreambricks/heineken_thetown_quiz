using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
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
    [SerializeField] private CpfPopupValidation cpfPopupValidation;
    [SerializeField] private AdultPopupValidation adultPopupValidation;
    [SerializeField] private EmailPopupValidation emailPopupValidation;

    private string xmlString;
    private string folderOutput = Application.streamingAssetsPath;
    private string dataToEncrypt;
    private string fileName;
    private string stringEncrypted;
    public string barName;
    private bool isCpfValid;
    private bool isAdult;
    private bool IsEmail;

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
       
    }

    private void GoPolicy()
    {
        privacyPolicyWindow.Show();
        
    }

    private void GoGuessWhoWindow()
    {

        isCpfValid = ValidateCPF(cpf.text);
        isAdult = IsAdult(dataAniversario.text);
        IsEmail = IsValidEmail(email.text);

        if (nome.text != "" && sobrenome.text != "" && cpf.text != "" && email.text != "" && dataAniversario.text != "" && terms.isChecked && policy.isChecked)
        {
 
            if (isCpfValid)
            {
                if (isAdult)
                {
                   if (IsEmail)
                    {
                        guessWhoWindow.Show();
                        EncryptData();
                        Hide();
                    }
                    else
                    {
                        emailPopupValidation.Show();
                    }
                }
                else
                {
                    adultPopupValidation.Show();
                }
            }
            else
            {
                cpfPopupValidation.Show();
            }

        }
        else
        {
            alertPopup.Show();
        }
    }


    void EncryptData()
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
        fileName = string.Format("player_data/{0}_{1}.enc", barName, formattedDateTime);

        string fullPath = Path.Combine(folderOutput, fileName);
        Debug.Log(fullPath);

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

    public bool ValidateCPF(string cpf)
    {
        // Remova caracteres n�o num�ricos do CPF
        cpf = new string(cpf.Where(char.IsDigit).ToArray());

        // Verifique se o CPF tem 11 d�gitos
        if (cpf.Length != 11)
        {
            return false;
        }

        // Verifique se todos os d�gitos s�o iguais (CPF inv�lido)
        if (new string(cpf[0], 11) == cpf)
        {
            return false;
        }

        // Calcule os d�gitos verificadores
        int[] multiplicadores1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicadores2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        string tempCpf = cpf.Substring(0, 9);
        int soma = 0;

        for (int i = 0; i < 9; i++)
        {
            soma += int.Parse(tempCpf[i].ToString()) * multiplicadores1[i];
        }

        int resto = soma % 11;
        int digito1 = resto < 2 ? 0 : 11 - resto;

        tempCpf += digito1;

        soma = 0;
        for (int i = 0; i < 10; i++)
        {
            soma += int.Parse(tempCpf[i].ToString()) * multiplicadores2[i];
        }

        resto = soma % 11;
        int digito2 = resto < 2 ? 0 : 11 - resto;

        tempCpf += digito2;

        return cpf == tempCpf;
    }

    public static bool IsAdult(string birthdate)
    {
        DateTime birthdateDateTime;
        if (!DateTime.TryParseExact(birthdate, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out birthdateDateTime))
        {
            Debug.Log("Formato de data inv�lido!");
            return false;
        }

        DateTime currentDate = DateTime.Now;
        int age = currentDate.Year - birthdateDateTime.Year;

        if (birthdateDateTime > currentDate.AddYears(-age))
        {
            age--;
        }

        return age >= 18;
    }

    public static bool IsValidEmail(string email)
    {
        
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        bool isMatch = Regex.IsMatch(email, pattern);

        return isMatch;
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

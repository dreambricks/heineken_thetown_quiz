using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
            Hide();
        }
        else
        {
            alertPopup.Show();
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

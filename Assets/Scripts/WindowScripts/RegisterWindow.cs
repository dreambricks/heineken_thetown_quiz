using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterWindow : MonoBehaviour
{

    public Button termsBtn;
    public Button policyBtn;
    public Button advanceBtn;

    [SerializeField] private TermsWindow termsWindow;
    [SerializeField] private PrivacyPolicyWindow privacyPolicyWindow;
    [SerializeField] private GuessWhoWindow guessWhoWindow;

    private void Start()
    {
        termsBtn.onClick.AddListener(() => GoTerms());
        policyBtn.onClick.AddListener(() => GoPolicy());
        advanceBtn.onClick.AddListener(() => GoGuessWhoWindow());
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
        guessWhoWindow.Show();
        Hide();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private MainWindow mainWindow;
    [SerializeField] private AgeConfirmWindow ageConfirmWindow;
    [SerializeField] private MinorAgeWindow minorAgeWindow;
    [SerializeField] private RegisterWindow registerWindow;
    [SerializeField] private TermsWindow termsWindow;
    [SerializeField] private PrivacyPolicyWindow privacyPolicyWindow;
    [SerializeField] private GuessWhoWindow guessWhoWindow;
    [SerializeField] private RegressiveCountWindow regressiveCountWindow;
    [SerializeField] private PlayGame playGame;
    [SerializeField] private Points points;
    [SerializeField] private YouWinWindow youWinWindow;
    [SerializeField] private YouLoseWindow youLoseWindow;

    private void Start()
    {
        mainWindow.Show();
        ageConfirmWindow.Hide();
        minorAgeWindow.Hide();
        registerWindow.Hide();
        termsWindow.Hide();
        privacyPolicyWindow.Hide();
        guessWhoWindow.Hide();
        regressiveCountWindow.Hide();
        playGame.Hide();
        points.Hide();
        youWinWindow.Hide();
        youLoseWindow.Hide();

        mainWindow.OnClicked += MainWindow_OnClicked;
        minorAgeWindow.OnClicked += MinorAgeWindow_OnClicked;
    }

    private void MainWindow_OnClicked(object sender, System.EventArgs e)
    {

        mainWindow.Hide();
        ageConfirmWindow.Show();
    }

    private void MinorAgeWindow_OnClicked(object sender, System.EventArgs e)
    {
        ageConfirmWindow.Show();
        minorAgeWindow.Hide();
    }
}

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    public float totalTime = 60.0f;  // Tempo total da contagem regressiva em segundos
    public Text countdownText;
    private float currentTime;

    [SerializeField] private PlayGame playGame;
    [SerializeField] private RegressiveCountWindow regressiveCountWindow;

    private void OnEnable()
    {
        currentTime = totalTime;
    }

    private void Update()
    {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            currentTime = 0;
            playGame.Show();
            regressiveCountWindow.Hide();
        }

        UpdateCountdownText();
    }

    private void UpdateCountdownText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        countdownText.text = seconds.ToString();
    }
}

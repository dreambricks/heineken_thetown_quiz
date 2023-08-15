using UnityEngine;
using UnityEngine.UI;

public class ProgressBarController : MonoBehaviour
{
    public Slider progressBar;
    public float totalTime = 15f;

    private float currentTime = 0f;

    void Update()
    {
        if (currentTime <= totalTime)
        {
            currentTime += Time.deltaTime;
            float progress = currentTime / totalTime;
            progressBar.value = progress;
        }
        else
        {
            currentTime = 0f;
        }
    }
}

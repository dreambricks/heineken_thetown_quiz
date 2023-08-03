using UnityEngine;

public class PlayAudioOnStart : MonoBehaviour
{
    public AudioSource audioSource;  // Assign the Audio Source in the Inspector

    private void Start()
    {
        if (audioSource && audioSource.clip)
        {
            audioSource.Play();
        }
    }
}
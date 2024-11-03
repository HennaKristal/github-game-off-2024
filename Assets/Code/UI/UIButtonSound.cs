using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButtonSound : MonoBehaviour
{
    [SerializeField] private AudioClip buttonHoverAudio;
    [SerializeField] private AudioClip buttonClickAudio;
    [SerializeField] private AudioSource audioSource;
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    public void PlayButtonHoverSound()
    {
        if (button == null || !button.interactable)
        {
            return;
        }

        audioSource.clip = buttonHoverAudio;
        audioSource.Play();
    }

    public void PlayButtonClickSound()
    {
        if (button == null || !button.interactable)
        {
            return;
        }

        audioSource.clip = buttonClickAudio;
        audioSource.Play();
    }
}

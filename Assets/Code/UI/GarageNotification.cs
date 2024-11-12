using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GarageNotification : MonoBehaviour
{
    [SerializeField] private GameObject notificationPanel;
    [SerializeField] private Garage garage;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI senderText;
    [SerializeField] private TextMeshProUGUI messageText;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip pressedSound;

    private InputController inputController;
    private Tuple<string, string, string> notification;
    private bool noMoreMessages = false;

    private void Start()
    {
        inputController = GameManager.Instance.GetInputController();
        DisplayNextNotification();
    }

    private void Update()
    {
        if (noMoreMessages)
        {
            return;
        }

        if (inputController.dodgePressed)
        {
            PlayPressedSound();
            DisplayNextNotification();
        }
    }

    private void DisplayNextNotification()
    {
        notification = GameManager.Instance.GetNotification();

        if (notification != null)
        {
            garage.disableNavigation = true;
            notificationPanel.SetActive(true);
            titleText.text = notification.Item1;
            senderText.text = "From: " + notification.Item2;
            messageText.text = notification.Item3;
        }
        else
        {

            notificationPanel.SetActive(false);
            Invoke(nameof(returnNavigationControl), 1f);
            noMoreMessages = true;
        }
    }

    private void returnNavigationControl()
    {
        garage.disableNavigation = false;
        this.enabled = false;
    }

    private void PlayPressedSound()
    {
        audioSource.PlayOneShot(pressedSound);
    }
}

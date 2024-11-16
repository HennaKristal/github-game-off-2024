using System;
using TMPro;
using UnityEngine;

public class GarageNotification : MonoBehaviour
{
    [SerializeField] private GameObject notificationPanel;
    [SerializeField] private Garage garage;
    [SerializeField] private PartSelection partSelection;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI senderText;
    [SerializeField] private TextMeshProUGUI messageText;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip pressedSound;

    private InputController inputController;
    private Tuple<string, string, string> notification;
    private bool noMoreMessages = false;
    private bool partSelectionNavigationDisabled = false;
    private bool garageNavigationDisabled = false;

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
            DisplayNotification();
        }
    }

    public void DisplayNextNotification()
    {
        partSelectionNavigationDisabled = partSelection.disableNavigation;
        garageNavigationDisabled = garage.disableNavigation;
        garage.disableNavigation = true;
        partSelection.disableNavigation = true;
        Invoke(nameof(DisplayNotification), 0f);
    }


    private void DisplayNotification()
    {
        notification = GameManager.Instance.GetNotification();

        if (notification != null)
        {
            noMoreMessages = false;
            notificationPanel.SetActive(true);
            titleText.text = notification.Item1;
            senderText.text = "From: " + notification.Item2;
            messageText.text = notification.Item3;
        }
        else
        {
            noMoreMessages = true;
            notificationPanel.SetActive(false);
            Invoke(nameof(returnNavigationControl), 0.5f);
        }
    }


    private void returnNavigationControl()
    {
        garage.disableNavigation = garageNavigationDisabled;
        partSelection.disableNavigation = partSelectionNavigationDisabled;
    }

    private void PlayPressedSound()
    {
        audioSource.PlayOneShot(pressedSound);
    }
}

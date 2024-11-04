using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GarageNotification : MonoBehaviour
{
    public GameObject notificationPanel;
    public Text titleText;
    public Text messageText;
    public Button nextButton;

    private Queue<Notification> notifications = new Queue<Notification>();
    private bool isDisplaying = false;

    // Inner class to store individual notifications
    private class Notification
    {
        public string Title { get; private set; }
        public string Message { get; private set; }

        public Notification(string title, string message)
        {
            Title = title;
            Message = message;
        }
    }

    private void Start()
    {
        // Hide the notification panel initially
        notificationPanel.SetActive(false);
        nextButton.onClick.AddListener(DisplayNextNotification);
    }

    // Method to add a new notification
    public void AddNew(string title, string message)
    {
        notifications.Enqueue(new Notification(title, message));

        // Show notification panel if it's not already displaying notifications
        if (!isDisplaying)
        {
            DisplayNextNotification();
        }
    }

    // Display the next notification in the queue
    private void DisplayNextNotification()
    {
        if (notifications.Count > 0)
        {
            // Get the next notification and display it
            Notification currentNotification = notifications.Dequeue();
            titleText.text = currentNotification.Title;
            messageText.text = currentNotification.Message;

            notificationPanel.SetActive(true);
            isDisplaying = true;
        }
        else
        {
            // No more notifications to display, hide the panel
            notificationPanel.SetActive(false);
            isDisplaying = false;
        }
    }
}

using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class MainMenuNavigation : MonoBehaviour
{
    [System.Serializable]
    public struct Credit
    {
        public Image image;
        public string url;
    }

    private enum MainMenuColumn { Menu, Credits };

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI[] mainButtons;
    [SerializeField] private Credit[] creditButtons;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip selectionSound;
    [SerializeField] private AudioClip pressedSound;

    private InputController inputController;
    private int currentIndex = 0;
    private MainMenuColumn currentColumn = MainMenuColumn.Menu;
    private float inputCooldown = 0.25f;
    private float nextInputTime = 0f;
    private float movementDeadZone = 0.4f;
    private bool isContinueEnabled = false;

    private void Start()
    {
        inputController = GameManager.Instance.GetInputController();

        isContinueEnabled = PlayerPrefs.HasKey("SafeFileFound");

        if (!isContinueEnabled)
        {
            mainButtons[0].color = new Color32(125, 125, 125, 255);
            currentIndex = 1;
        }

        UpdateButtonHighlight();
    }

    private void Update()
    {
        HandleNavigation();

        if (inputController.dodgePressed)
        {
            SelectCurrentSlot();
        }
    }

    private void HandleNavigation()
    {
        // Reset delay if input is released
        if (Mathf.Abs(inputController.Move.x) < movementDeadZone && Mathf.Abs(inputController.Move.y) < movementDeadZone)
        {
            nextInputTime = Time.time;
        }

        // Delay for inputs
        if (Time.time < nextInputTime)
        {
            return;
        }

        int previousIndex = currentIndex;
        MainMenuColumn previousColumn = currentColumn;

        if (inputController.Move.y > movementDeadZone) // Up
        {
            if (currentColumn == MainMenuColumn.Menu)
            {
                currentIndex = Mathf.Max(isContinueEnabled ? 0 : 1, currentIndex - 1);
            }
            else if (currentColumn == MainMenuColumn.Credits)
            {
                currentIndex = Mathf.Max(0, currentIndex - 1);
            }
        }
        else if (inputController.Move.y < -movementDeadZone) // Down
        {
            if (currentColumn == MainMenuColumn.Menu && currentIndex < mainButtons.Length - 1)
            {
                currentIndex++;
            }
            else if (currentColumn == MainMenuColumn.Credits && currentIndex < creditButtons.Length - 1)
            {
                currentIndex++;
            }
        }
        else if (inputController.Move.x < -movementDeadZone && currentColumn == MainMenuColumn.Menu) // Left
        {
            currentColumn = MainMenuColumn.Credits;
            currentIndex = 0;
        }
        else if (inputController.Move.x > movementDeadZone && currentColumn == MainMenuColumn.Credits) // Right
        {
            currentColumn = MainMenuColumn.Menu;
            currentIndex = isContinueEnabled ? 0 : 1;
        }

        // Update visuals if something changed
        if (currentIndex != previousIndex || currentColumn != previousColumn)
        {
            nextInputTime = Time.time + inputCooldown;
            UpdateButtonHighlight();
            PlaySelectionSound();
        }
    }

    private void UpdateButtonHighlight()
    {
        // Reset all highlights
        foreach (var button in creditButtons) button.image.color = new Color32(255, 200, 0, 255);
        foreach (var button in mainButtons) button.color = Color.white;
        if (!isContinueEnabled) { mainButtons[0].color = new Color32(125, 125, 125, 255); }

        // Highlight current selection
        if (currentColumn == MainMenuColumn.Menu)
        {
            mainButtons[currentIndex].color = new Color32(255, 200, 0, 255);
        }
        else
        {
            creditButtons[currentIndex].image.color = new Color32(255, 150, 0, 255);
        }
    }

    private void PlaySelectionSound()
    {
        audioSource.PlayOneShot(selectionSound);
    }

    private void PlayPressedSound()
    {
        audioSource.PlayOneShot(pressedSound);
    }

    public void SelectCurrentSlot() => StartCoroutine(SelectCurrentSlotCoroutine());
    private IEnumerator SelectCurrentSlotCoroutine()
    {
        PlayPressedSound();
        yield return new WaitForSeconds(pressedSound.length);

        if (currentColumn == MainMenuColumn.Menu)
        {
            switch (currentIndex)
            {
                case 0: if (isContinueEnabled) ContinueGame(); break;
                case 1: StartNewGame(); break;
                case 2: ExitGame(); break;
            }
        }
        else
        {
            GameManager.Instance.OpenLink(creditButtons[currentIndex].url);
        }
    }

    private void ContinueGame()
    {
        if (!isContinueEnabled) { return; }

        GameManager.Instance.LoadData();
        GameManager.Instance.LoadSceneByName("Garage");
        this.enabled = false;
    }

    private void StartNewGame()
    {
        GameManager.Instance.ResetSaveData();
        GameManager.Instance.LoadSceneByName("Tutorial");
        this.enabled = false;
    }

    private void ExitGame()
    {
        GameManager.Instance.ExitApplication();
        this.enabled = false;
    }

    public void HoverOverCredits(int index)
    {
        currentColumn = MainMenuColumn.Credits;
        currentIndex = index;
        UpdateButtonHighlight();
        PlaySelectionSound();
    }

    public void HoverOverMenu(int index)
    {
        if (isContinueEnabled && index == 0)
        {
            return;
        }

        currentColumn = MainMenuColumn.Menu;
        currentIndex = index;
        UpdateButtonHighlight();
        PlaySelectionSound();
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Garage : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputController inputController;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PartSelection partSelection;

    [Header("UI Elements")]
    [SerializeField] private Image[] topRowSlots;
    [SerializeField] private Image[] middleRowSlots;
    [SerializeField] private Button pilotSkillsButton;
    [SerializeField] private Button nextMissionButton;
    [SerializeField] private Button exitGameButton;

    [Header("Slot Images")]
    [SerializeField] private Sprite normalSlotImage;
    [SerializeField] private Sprite activeSlotImage;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip selectionSound;
    [SerializeField] private AudioClip pressedSound;

    [HideInInspector] public bool ispartSelectionWindowOpened = false;
    [HideInInspector] public bool isPlaneMisconfigured = false;
    private Image currentActiveSlotImage;
    private int currentIndex = 0;
    private int currentRow = 0;
    private float inputCooldown = 0.25f;
    private float nextInputTime = 0f;
    private float movementDeadZone = 0.4f;

    private void Start()
    {
        UpdateActiveSlot();
    }

    private void Update()
    {
        if (ispartSelectionWindowOpened)
        {
            return;
        }

        HandleNavigation();

        if (inputController.dodgePressed)
        {
            Invoke(nameof(SelectCurrentSlot), 0f);
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
        int previousRow = currentRow;

        // Horizontal movement (right)
        if (inputController.Move.x > movementDeadZone)
        {
            if (currentRow == 0 && currentIndex < topRowSlots.Length - 1)
            {
                currentIndex++;
            }
            else if (currentRow == 1 && currentIndex < middleRowSlots.Length - 1)
            {
                currentIndex++;
            }
        }
        // Horizontal movement (left)
        else if (inputController.Move.x < -movementDeadZone)
        {
            if (currentRow < 2 && currentIndex > 0)
            {
                currentIndex--;
            }
        }
        // Vertical movement (up)
        else if (inputController.Move.y > movementDeadZone)
        {
            if (currentRow == 4)
            {
                currentRow = 3;
                currentIndex = 0;
            }
            else if (currentRow == 3)
            {
                currentRow = 2;
                currentIndex = 0;
            }
            else if (currentRow == 2)
            {
                currentRow = 1;
                currentIndex = 2;
            }
            else if (currentRow == 1)
            {
                currentRow = 0;
                currentIndex = Mathf.Clamp(currentIndex, 0, topRowSlots.Length - 1);
            }
        }
        // Vertical movement (down)
        else if (inputController.Move.y < -movementDeadZone)
        {
            if (currentRow == 0)
            {
                currentRow = 1;
                currentIndex = Mathf.Clamp(currentIndex, 0, middleRowSlots.Length - 1);
            }
            else if (currentRow == 1)
            {
                currentRow = 2;
                currentIndex = 0;
            }
            else if (currentRow == 2)
            {
                currentRow = 3;
                currentIndex = 0;
            }
            else if (currentRow == 3)
            {
                currentRow = 4;
                currentIndex = 0;
            }
        }

        // Update slots if something changed
        if (currentIndex != previousIndex || currentRow != previousRow)
        {
            nextInputTime = Time.time + inputCooldown;
            UpdateActiveSlot();
            PlaySelectionSound();
        }
    }

    private void UpdateActiveSlot()
    {
        // Reset previous buttons' appearance
        pilotSkillsButton.GetComponent<TextMeshProUGUI>().color = Color.white;
        nextMissionButton.GetComponent<TextMeshProUGUI>().color = Color.white;
        exitGameButton.GetComponent<TextMeshProUGUI>().color = Color.white;

        // Reset previous slot's appearance
        if (currentActiveSlotImage != null)
        {
            currentActiveSlotImage.sprite = normalSlotImage;
        }

        // Set new active slot/button based on row and index
        if (currentRow == 0)
        {
            currentActiveSlotImage = topRowSlots[currentIndex];
            currentActiveSlotImage.sprite = activeSlotImage;
        }
        else if (currentRow == 1)
        {
            currentActiveSlotImage = middleRowSlots[currentIndex];
            currentActiveSlotImage.sprite = activeSlotImage;
        }
        else if (currentRow == 2)
        {
            currentActiveSlotImage = null;
            pilotSkillsButton.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 0f);
        }
        else if (currentRow == 3)
        {
            currentActiveSlotImage = null;
            nextMissionButton.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 0f);
        }
        else if (currentRow == 4)
        {
            currentActiveSlotImage = null;
            exitGameButton.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 0f);
        }
    }

    private void SelectCurrentSlot()
    {
        PlayPressedSound();

        if (currentRow == 0 || currentRow == 1)
        {
            OpenPartSelectionUI();
        }
        else if (currentRow == 2)
        {
            OpenPilotSkills();
        }
        else if (currentRow == 3)
        {
            StartNextMission();
        }
        else if (currentRow == 4)
        {
            QuitGame();
        }
    }

    private void OpenPartSelectionUI()
    {
        if (currentRow == 0)
        {
            switch (currentIndex)
            {
                case 0: partSelection.DisplayPlaneParts(); break;
                case 1: partSelection.DisplayEngineParts(); break;
                case 2: partSelection.DisplayGeneratorParts(); break;
                case 3: partSelection.DisplayCoolerParts(); break;
                case 4: partSelection.DisplayTokens(); break;
                case 5: partSelection.DisplayBadges(); break;
            }
        }
        else if (currentRow == 1)
        {
            switch (currentIndex)
            {
                case 0: partSelection.DisplayLeftOuterWeaponParts(); break;
                case 1: partSelection.DisplayLeftInnerWeaponParts(); break;
                case 2: partSelection.DisplayMainWeaponParts(); break;
                case 3: partSelection.DisplayRightInnerWeaponParts(); break;
                case 4: partSelection.DisplayRightOuterWeaponParts(); break;
            }
        }
    }

    private void StartNextMission()
    {
        if (!isPlaneMisconfigured)
        {
            gameManager.LoadSceneByName("cp0_mission1");
        }
        else
        {
            // TODO: show error message
        }
    }

    private void OpenPilotSkills()
    {
        Debug.Log("Opening pilot skills...");
    }

    private void QuitGame()
    {
        gameManager.LoadSceneByName("Main Menu");
    }

    private void PlaySelectionSound()
    {
        audioSource.PlayOneShot(selectionSound);
    }

    private void PlayPressedSound()
    {
        audioSource.PlayOneShot(pressedSound);
    }
}

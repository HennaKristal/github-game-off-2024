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

    private Image currentActiveSlotImage;
    private int currentIndex = 0;
    private int currentRow = 0;
    private float inputCooldown = 0.25f;
    private float nextInputTime = 0f;
    private float movementDeadZone = 0.4f;
    public bool ispartSelectionWindowOpened = false;

    private void Start()
    {
        UpdateActiveSlot();
    }

    private void Update()
    {
        if (!ispartSelectionWindowOpened)
        {
            HandleNavigation();

            if (inputController.dodgePressed)
            {
                SelectCurrentSlot();
            }
        }
    }

    private void HandleNavigation()
    {
        if (Mathf.Abs(inputController.Move.x) < movementDeadZone && Mathf.Abs(inputController.Move.y) < movementDeadZone)
        {
            nextInputTime = Time.time;
        }

        if (Time.time < nextInputTime)
        {
            return;
        }

        int previousIndex = currentIndex;
        int previousRow = currentRow;

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
        else if (inputController.Move.x < -movementDeadZone)
        {
            if (currentRow < 2 && currentIndex > 0)
            {
                currentIndex--;
            }
        }
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

        if (currentIndex != previousIndex || currentRow != previousRow)
        {
            nextInputTime = Time.time + inputCooldown;
            UpdateActiveSlot();
        }
    }

    private void UpdateActiveSlot()
    {
        // Reset previous slot/button appearance
        if (currentActiveSlotImage != null)
        {
            currentActiveSlotImage.sprite = normalSlotImage;
        }

        ResetButtonColors();

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

    private void ResetButtonColors()
    {
        pilotSkillsButton.GetComponent<TextMeshProUGUI>().color = Color.white;
        nextMissionButton.GetComponent<TextMeshProUGUI>().color = Color.white;
        exitGameButton.GetComponent<TextMeshProUGUI>().color = Color.white;
    }

    private void SelectCurrentSlot()
    {
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
            }
        }
        else if (currentRow == 1)
        {
            switch (currentIndex)
            {
                case 0: partSelection.DisplayPlaneParts(); break;
                case 1: partSelection.DisplayPlaneParts(); break;
                case 2: partSelection.DisplayPlaneParts(); break;
                case 3: partSelection.DisplayPlaneParts(); break;
                case 4: partSelection.DisplayPlaneParts(); break;
            }
        }
    }

    private void StartNextMission()
    {
        gameManager.LoadSceneByName("Game");
    }

    private void OpenPilotSkills()
    {
        Debug.Log("Opening pilot skills...");
    }

    private void QuitGame()
    {
        gameManager.LoadSceneByName("Main Menu");
    }
}

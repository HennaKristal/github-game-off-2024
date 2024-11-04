using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GarageUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputController inputController;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Garage garage;

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
    private float inputCooldown = 0.2f;
    private float nextHorizontalInputTime = 0f;
    private float nextVerticalInputTime = 0f;
    private bool ispartSelectionWindowOpened = false;

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
        int previousIndex = currentIndex;
        int previousRow = currentRow;

        // Horizontal Navigation Logic
        if (inputController.Move.x > 0.1f && Time.time >= nextHorizontalInputTime)
        {
            if (currentRow == 0 && currentIndex < topRowSlots.Length - 1)
            {
                currentIndex++;
            }
            else if (currentRow == 1 && currentIndex < middleRowSlots.Length - 1)
            {
                currentIndex++;
            }

            nextHorizontalInputTime = Time.time + inputCooldown;
        }
        else if (inputController.Move.x < -0.1f && Time.time >= nextHorizontalInputTime)
        {
            if (currentRow < 2 && currentIndex > 0)
            {
                currentIndex--;
            }

            nextHorizontalInputTime = Time.time + inputCooldown;
        }

        // Vertical Navigation Logic
        if (inputController.Move.y < -0.1f && Time.time >= nextVerticalInputTime)
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

            nextVerticalInputTime = Time.time + inputCooldown;
        }
        else if (inputController.Move.y > 0.1f && Time.time >= nextVerticalInputTime)
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

            nextVerticalInputTime = Time.time + inputCooldown;
        }

        if (currentIndex != previousIndex || currentRow != previousRow)
        {
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
                case 0: garage.DisplayPlaneParts(); break;
                case 1: garage.DisplayEngineParts(); break;
                case 2: garage.DisplayGeneratorParts(); break;
                case 3: garage.DisplayCoolerParts(); break;
            }
        }
        else if (currentRow == 1)
        {
            switch (currentIndex)
            {
                case 0: garage.DisplayPlaneParts(); break;
                case 1: garage.DisplayPlaneParts(); break;
                case 2: garage.DisplayPlaneParts(); break;
                case 3: garage.DisplayPlaneParts(); break;
                case 4: garage.DisplayPlaneParts(); break;
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

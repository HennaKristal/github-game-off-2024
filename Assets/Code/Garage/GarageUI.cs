using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XInput;
using UnityEngine.UI;

public class GarageUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputController inputController;
    [SerializeField] private GameManager gameManager;

    [Header("Windows")]
    [SerializeField] private GameObject planeCoreSelectionWindow;
    [SerializeField] private GameObject engineSelectionWindow;
    [SerializeField] private GameObject generatorSelectionWindow;
    [SerializeField] private GameObject coolerSelectionWindow;
    [SerializeField] private GameObject mainGunSelectionWindow;
    [SerializeField] private GameObject leftInnerGunSelectionWindow;
    [SerializeField] private GameObject rightInnerGunSelectionWindow;
    [SerializeField] private GameObject leftOuterGunSelectionWindow;
    [SerializeField] private GameObject rightOuterGunSelectionWindow;

    [Header("UI Elements")]
    [SerializeField] private Image[] topRowSlots;
    [SerializeField] private Image[] middleRowSlots;
    [SerializeField] private Button pilotSkillsButton;
    [SerializeField] private Button nextMissionButton;
    [SerializeField] private Button exitGameButton;

    [Header("Slot Images")]
    [SerializeField] private Sprite normalSlotImage;
    [SerializeField] private Sprite activeSlotImage;

    private int currentIndex = 0;
    private int currentRow = 0;
    private Image activeImage;
    private float inputCooldown = 0.2f;
    private float nextHorizontalInputTime = 0f;
    private float nextVerticalInputTime = 0f;

    private void Start()
    {
        UpdateActiveSlot();
    }

    private void Update()
    {
        HandleNavigation();

        if (inputController.dodgePressed)
        {
            SelectCurrentSlot();
        }

        if (inputController.healPressed)
        {
            ClosePartSelectionUI();
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
                currentIndex++;
            else if (currentRow == 1 && currentIndex < middleRowSlots.Length - 1)
                currentIndex++;

            nextHorizontalInputTime = Time.time + inputCooldown;
        }
        else if (inputController.Move.x < -0.1f && Time.time >= nextHorizontalInputTime)
        {
            if (currentIndex > 0)
                currentIndex--;

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
        if (activeImage != null)
            activeImage.sprite = normalSlotImage;

        ResetButtonColors();

        // Set new active slot/button based on row and index
        if (currentRow == 0)
        {
            activeImage = topRowSlots[currentIndex];
            activeImage.sprite = activeSlotImage;
        }
        else if (currentRow == 1)
        {
            activeImage = middleRowSlots[currentIndex];
            activeImage.sprite = activeSlotImage;
        }
        else if (currentRow == 2)
        {
            activeImage = null;
            ResetButtonColors();
            pilotSkillsButton.GetComponent<TextMeshProUGUI>().color = Color.red;
        }
        else if (currentRow == 3)
        {
            activeImage = null;
            ResetButtonColors();
            nextMissionButton.GetComponent<TextMeshProUGUI>().color = Color.red;
        }
        else if (currentRow == 4)
        {
            activeImage = null;
            ResetButtonColors();
            exitGameButton.GetComponent<TextMeshProUGUI>().color = Color.red;
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
        if (currentRow == 0)
        {
            string slotName = topRowSlots[currentIndex].name;
            OpenPartSelectionUI(slotName);
        }
        else if (currentRow == 1)
        {
            string slotName = middleRowSlots[currentIndex].name;
            OpenPartSelectionUI(slotName);
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

    private void OpenPartSelectionUI(string windowToOpen)
    {
        windowToOpen = windowToOpen.ToLower();
        ClosePartSelectionUI();

        switch (windowToOpen)
        {
            case "plane core slot": planeCoreSelectionWindow.SetActive(true); break;
            case "engine slot": engineSelectionWindow.SetActive(true); break;
            case "generator slot": generatorSelectionWindow.SetActive(true); break;
            case "cooler slot": coolerSelectionWindow.SetActive(true); break;
            case "main gun slot": mainGunSelectionWindow.SetActive(true); break;
            case "left inner side gun slot": leftInnerGunSelectionWindow.SetActive(true); break;
            case "right inner side gun slot": rightInnerGunSelectionWindow.SetActive(true); break;
            case "left outer side gun slot": leftOuterGunSelectionWindow.SetActive(true); break;
            case "right outer side gun slot": rightOuterGunSelectionWindow.SetActive(true); break;
            default: Debug.LogWarning("Unknown part selection window: " + windowToOpen); break;
        }
    }

    private void ClosePartSelectionUI()
    {
        planeCoreSelectionWindow.SetActive(false);
        engineSelectionWindow.SetActive(false);
        generatorSelectionWindow.SetActive(false);
        coolerSelectionWindow.SetActive(false);
        mainGunSelectionWindow.SetActive(false);
        leftInnerGunSelectionWindow.SetActive(false);
        rightInnerGunSelectionWindow.SetActive(false);
        leftOuterGunSelectionWindow.SetActive(false);
        rightOuterGunSelectionWindow.SetActive(false);
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

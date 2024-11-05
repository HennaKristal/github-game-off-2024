using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartSelection : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Garage garage;
    [SerializeField] private InputController inputController;

    [Header("Parts")]
    [SerializeField] private List<PlaneStats> planeParts;
    [SerializeField] private List<EngineStats> engineParts;
    [SerializeField] private List<GeneratorStats> generatorParts;
    [SerializeField] private List<CoolerStats> coolerParts;
    [SerializeField] private List<WeaponStats> weapons;

    [Header("UI Elements")]
    [SerializeField] private GameObject partSelectionWindow;
    [SerializeField] private GameObject partUIPrefab;
    [SerializeField] private TextMeshProUGUI partSelectionTitle;
    [SerializeField] private GameObject partParentContainer;
    [SerializeField] private Button exitPartSelectionButton;

    [Header("Slot Images")]
    [SerializeField] private Sprite normalSlotImage;
    [SerializeField] private Sprite activeSlotImage;

    private int currentPartIndex = 0;
    private List<GameObject> partUIs = new List<GameObject>();
    private Image currentActiveSlotImage;
    private float inputCooldown = 0.25f;
    private float nextInputTime = 0f;
    private float movementDeadZone = 0.4f;
    private int index = -1;

    private void Update()
    {
        if (garage.ispartSelectionWindowOpened)
        {
            HandlePartNavigation();

            if (inputController.dodgePressed)
            {
                if (currentPartIndex == -1)
                {
                    ClosePartSelectionWindow();
                }
                else
                {
                    // SelectCurrentSlot();
                }
            }

            if (inputController.healPressed)
            {
                ClosePartSelectionWindow();
            }
        }
    }

    private void ClosePartSelectionWindow()
    {
        partSelectionWindow.SetActive(false);

        // Clear any existing UI elements in the container to avoid duplicates
        foreach (Transform child in partParentContainer.transform)
        {
            Destroy(child.gameObject);
        }

        partUIs.Clear();
        index = -1;

        garage.ispartSelectionWindowOpened = false;
    }

    private void HandlePartNavigation()
    {
        if (Mathf.Abs(inputController.Move.x) < movementDeadZone && Mathf.Abs(inputController.Move.y) < movementDeadZone)
        {
            nextInputTime = Time.time;
        }

        if (Time.time < nextInputTime)
        {
            return;
        }

        int previousPartIndex = currentPartIndex;

        if (currentPartIndex == -1)
        {
            if (inputController.Move.y > movementDeadZone)
            {
                currentPartIndex = partUIs.Count - 1;
            }
        }
        else if (inputController.Move.x > movementDeadZone)
        {
            if (currentPartIndex + 1 > partUIs.Count - 1)
            {
                currentPartIndex = -1;
            }
            else
            {
                currentPartIndex = Mathf.Min(currentPartIndex + 1, partUIs.Count - 1);
            }
        }
        else if (inputController.Move.x < -movementDeadZone)
        {
            currentPartIndex = Mathf.Max(currentPartIndex - 1, 0);
        }
        else if (inputController.Move.y > movementDeadZone && currentPartIndex >= 5)
        {
            currentPartIndex -= 5;
        }
        else if (inputController.Move.y < -movementDeadZone)
        {
            if (currentPartIndex + 5 < partUIs.Count)
            {
                currentPartIndex += 5;
            }
            else if (Mathf.Floor(currentPartIndex / 5) != Mathf.Floor(partUIs.Count / 5))
            {
                currentPartIndex = partUIs.Count - 1;
            }
            else
            {
                currentPartIndex = -1;
            }
        }

        if (currentPartIndex != previousPartIndex)
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

        if (currentPartIndex == -1)
        {
            exitPartSelectionButton.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 0f);
        }
        else
        {
            currentActiveSlotImage = partUIs[currentPartIndex].GetComponent<Image>();
            currentActiveSlotImage.sprite = activeSlotImage;
            exitPartSelectionButton.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f);

        }
    }


    public void DisplayPlaneParts()
    {
        partSelectionWindow.SetActive(true);
        garage.ispartSelectionWindowOpened = true;
        partSelectionTitle.text = "Plane Cores";

        foreach (PlaneStats planePart in planeParts)
        {
            if (!planePart.isPurchasable && !planePart.isOwned)
            {
                continue;
            }

            index++;

            GameObject partUI = Instantiate(partUIPrefab, partParentContainer.transform);
            partUIs.Add(partUI);
            TextMeshProUGUI titleText = partUI.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI priceText = partUI.transform.Find("Price").GetComponent<TextMeshProUGUI>();

            titleText.text = planePart.name;

            if (planePart.isEquipped)
            {
                currentPartIndex = index;
                currentActiveSlotImage = partUI.GetComponent<Image>();
                currentActiveSlotImage.sprite = activeSlotImage;
                priceText.text = "Equipped";
                priceText.color = Color.green;
            }
            else if (planePart.isPurchasable)
            {
                priceText.text = $"${planePart.purchasePrice}";
                priceText.color = Color.white;
            }
            else
            {
                priceText.text = "";
            }
        }
    }

    public void DisplayEngineParts()
    {
        partSelectionWindow.SetActive(true);
        garage.ispartSelectionWindowOpened = true;
        partSelectionTitle.text = "Engines";

        foreach (EngineStats enginePart in engineParts)
        {
            if (!enginePart.isPurchasable && !enginePart.isOwned)
            {
                continue;
            }

            index++;

            GameObject partUI = Instantiate(partUIPrefab, partParentContainer.transform);
            partUIs.Add(partUI);
            TextMeshProUGUI titleText = partUI.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI priceText = partUI.transform.Find("Price").GetComponent<TextMeshProUGUI>();

            titleText.text = enginePart.name;

            if (enginePart.isEquipped)
            {
                currentPartIndex = index;
                currentActiveSlotImage = partUI.GetComponent<Image>();
                currentActiveSlotImage.sprite = activeSlotImage;
                priceText.text = "Equipped";
                priceText.color = Color.green;
            }
            else if (enginePart.isPurchasable)
            {
                priceText.text = $"${enginePart.purchasePrice}";
                priceText.color = Color.white;
            }
            else
            {
                priceText.text = "";
            }
        }
    }

    public void DisplayGeneratorParts()
    {
        partSelectionWindow.SetActive(true);
        garage.ispartSelectionWindowOpened = true;
        partSelectionTitle.text = "Generators";

        foreach (GeneratorStats generatorPart in generatorParts)
        {
            if (!generatorPart.isPurchasable && !generatorPart.isOwned)
            {
                continue;
            }

            index++;

            GameObject partUI = Instantiate(partUIPrefab, partParentContainer.transform);
            partUIs.Add(partUI);
            TextMeshProUGUI titleText = partUI.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI priceText = partUI.transform.Find("Price").GetComponent<TextMeshProUGUI>();

            titleText.text = generatorPart.name;

            if (generatorPart.isEquipped)
            {
                currentPartIndex = index;
                currentActiveSlotImage = partUI.GetComponent<Image>();
                currentActiveSlotImage.sprite = activeSlotImage;
                priceText.text = "Equipped";
                priceText.color = Color.green;
            }
            else if (generatorPart.isPurchasable)
            {
                priceText.text = $"${generatorPart.purchasePrice}";
                priceText.color = Color.white;
            }
            else
            {
                priceText.text = "";
            }
        }
    }

    public void DisplayCoolerParts()
    {
        partSelectionWindow.SetActive(true);
        garage.ispartSelectionWindowOpened = true;
        partSelectionTitle.text = "Coolers";

        foreach (CoolerStats coolerPart in coolerParts)
        {
            if (!coolerPart.isPurchasable && !coolerPart.isOwned)
            {
                continue;
            }

            index++;

            GameObject partUI = Instantiate(partUIPrefab, partParentContainer.transform);
            partUIs.Add(partUI);
            TextMeshProUGUI titleText = partUI.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI priceText = partUI.transform.Find("Price").GetComponent<TextMeshProUGUI>();

            titleText.text = coolerPart.name;

            if (coolerPart.isEquipped)
            {
                currentPartIndex = index;
                currentActiveSlotImage = partUI.GetComponent<Image>();
                currentActiveSlotImage.sprite = activeSlotImage;
                priceText.text = "Equipped";
                priceText.color = Color.green;
            }
            else if (coolerPart.isPurchasable)
            {
                priceText.text = $"${coolerPart.purchasePrice}";
                priceText.color = Color.white;
            }
            else
            {
                priceText.text = "";
            }
        }
    }



    private void Start()
    {
        // TODO: on start load prefab or serialized data about every single scriptable object part and load their values
        // Also set this object to do not destory on load
    }

    public void AddItem(string category, string name)
    {
        // TODO find the correct part's scrptable object and set it's stats correctly
        //part.isOwned = true;
        //part.isPurchasable = true;
    }

    private void EquipPart(string category, string name)
    {
        // Get currently equipped item and unequip it or unequip just every item in this category
        // currentpart.isequipped = false;

        // TODO: equip the new item
        // part.isEquipped = true;
    }
}

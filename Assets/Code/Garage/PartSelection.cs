using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum PartCategory
{
    PlaneCores,
    Engines,
    Generators,
    Coolers,
    MainWeapons,
    LeftInnerWeapons,
    LeftOuterWeapons,
    RightInnerWeapons,
    RightOuterWeapons
}

public class PartSelection : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Garage garage;
    [SerializeField] private InputController inputController;
    [SerializeField] private PlayerStats playerStats;

    [Header("Parts")]
    [SerializeField] private List<PlaneStats> allPlaneParts;
    [SerializeField] private List<EngineStats> allEngineParts;
    [SerializeField] private List<GeneratorStats> allGeneratorParts;
    [SerializeField] private List<CoolerStats> allCoolerParts;
    [SerializeField] private List<WeaponStats> allMainWeaponsParts;
    [SerializeField] private List<WeaponStats> allLeftInnerWeaponParts;
    [SerializeField] private List<WeaponStats> allLeftOuterWeaponParts;
    [SerializeField] private List<WeaponStats> allRightInnerWeaponParts;
    [SerializeField] private List<WeaponStats> allRightOuterWeaponParts;
    private List<PlaneStats> planeParts = new List<PlaneStats>();
    private List<EngineStats> engineParts = new List<EngineStats>();
    private List<GeneratorStats> generatorParts = new List<GeneratorStats>();
    private List<CoolerStats> coolerParts = new List<CoolerStats>();
    private List<WeaponStats> mainWeaponsParts = new List<WeaponStats>();
    private List<WeaponStats> leftInnerWeaponParts = new List<WeaponStats>();
    private List<WeaponStats> leftOuterWeaponParts = new List<WeaponStats>();
    private List<WeaponStats> rightInnerWeaponParts = new List<WeaponStats>();
    private List<WeaponStats> rightOuterWeaponParts = new List<WeaponStats>();

    [Header("UI Elements")]
    [SerializeField] private GameObject partSelectionWindow;
    [SerializeField] private GameObject partUIPrefab;
    [SerializeField] private TextMeshProUGUI partSelectionTitle;
    [SerializeField] private GameObject partParentContainer;
    [SerializeField] private Button exitPartSelectionButton;
    [SerializeField] private TextMeshProUGUI moneyText;

    [Header("Slot Images")]
    [SerializeField] private Sprite normalSlotImage;
    [SerializeField] private Sprite activeSlotImage;

    private int equippedIndex = 0;
    private int currentPartIndex = 0;
    private List<GameObject> partUIs = new List<GameObject>();
    private Image currentActiveSlotImage;
    private float inputCooldown = 0.25f;
    private float nextInputTime = 0f;
    private float movementDeadZone = 0.4f;
    private int index = -1;
    private PartCategory partCategory;

    private void Start()
    {
        moneyText.text = "Money: " + playerStats.money + "$";
    }

    private void Update()
    {
        if (!garage.ispartSelectionWindowOpened)
        {
            return;
        }

        HandlePartNavigation();

        if (inputController.dodgePressed)
        {
            if (currentPartIndex == -1)
            {
                ClosePartSelectionWindow();
            }
            else if (currentPartIndex != equippedIndex)
            {
                EquipPart();
            }
        }

        if (inputController.healPressed)
        {
            ClosePartSelectionWindow();
        }

    }

    private void ClosePartSelectionWindow()
    {
        partSelectionWindow.SetActive(false);
        garage.ispartSelectionWindowOpened = false;
        index = -1;

        exitPartSelectionButton.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f);

        foreach (Transform child in partParentContainer.transform)
        {
            Destroy(child.gameObject);
        }

        partUIs.Clear();
        planeParts.Clear();
        engineParts.Clear();
        generatorParts.Clear();
        coolerParts.Clear();
        mainWeaponsParts.Clear();
        leftInnerWeaponParts.Clear();
        leftOuterWeaponParts.Clear();
        rightInnerWeaponParts.Clear();
        rightOuterWeaponParts.Clear();
    }

    private void OpenPartSelectionWindow(string title, PartCategory category)
    {
        partSelectionWindow.SetActive(true);
        garage.ispartSelectionWindowOpened = true;
        partSelectionTitle.text = title;
        partCategory = category;
    }

    private void HandlePartNavigation()
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

        int previousPartIndex = currentPartIndex;

        // Movement from return button
        if (currentPartIndex == -1)
        {
            if (inputController.Move.y > movementDeadZone)
            {
                currentPartIndex = partUIs.Count - 1;
            }
        }
        // Horizontal movement (right)
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
        // Horizontal movement (left)
        else if (inputController.Move.x < -movementDeadZone)
        {
            currentPartIndex = Mathf.Max(currentPartIndex - 1, 0);
        }
        // Vertical movement (up)
        else if (inputController.Move.y > movementDeadZone && currentPartIndex >= 5)
        {
            currentPartIndex -= 5;
        }
        // Vertical movement (down)
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

        // Update slots if something changed
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
        OpenPartSelectionWindow("Plane Cores", PartCategory.PlaneCores);

        foreach (PlaneStats planePart in allPlaneParts)
        {
            if (!planePart.isPurchasable && !planePart.isOwned)
            {
                continue;
            }

            GameObject partUI = Instantiate(partUIPrefab, partParentContainer.transform);
            TextMeshProUGUI titleText = partUI.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI priceText = partUI.transform.Find("Price").GetComponent<TextMeshProUGUI>();

            partUIs.Add(partUI);
            planeParts.Add(planePart);
            index++;

            titleText.text = planePart.partName;

            if (planePart.isEquipped)
            {
                currentPartIndex = index;
                equippedIndex = index;
                currentActiveSlotImage = partUI.GetComponent<Image>();
                currentActiveSlotImage.sprite = activeSlotImage;
                priceText.text = "Equipped";
                priceText.color = Color.green;
            }
            else if (planePart.isPurchasable)
            {
                priceText.text = $"{planePart.purchasePrice}$";
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
        OpenPartSelectionWindow("Engines", PartCategory.Engines);

        foreach (EngineStats enginePart in allEngineParts)
        {
            if (!enginePart.isPurchasable && !enginePart.isOwned)
            {
                continue;
            }

            GameObject partUI = Instantiate(partUIPrefab, partParentContainer.transform);
            TextMeshProUGUI titleText = partUI.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI priceText = partUI.transform.Find("Price").GetComponent<TextMeshProUGUI>();

            partUIs.Add(partUI);
            engineParts.Add(enginePart);
            index++;

            titleText.text = enginePart.partName;

            if (enginePart.isEquipped)
            {
                currentPartIndex = index;
                equippedIndex = index;
                currentActiveSlotImage = partUI.GetComponent<Image>();
                currentActiveSlotImage.sprite = activeSlotImage;
                priceText.text = "Equipped";
                priceText.color = Color.green;
            }
            else if (enginePart.isPurchasable)
            {
                priceText.text = $"{enginePart.purchasePrice}$";
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
        OpenPartSelectionWindow("Generators", PartCategory.Generators);

        foreach (GeneratorStats generatorPart in allGeneratorParts)
        {
            if (!generatorPart.isPurchasable && !generatorPart.isOwned)
            {
                continue;
            }

            GameObject partUI = Instantiate(partUIPrefab, partParentContainer.transform);
            TextMeshProUGUI titleText = partUI.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI priceText = partUI.transform.Find("Price").GetComponent<TextMeshProUGUI>();

            partUIs.Add(partUI);
            generatorParts.Add(generatorPart);
            index++;

            titleText.text = generatorPart.partName;

            if (generatorPart.isEquipped)
            {
                currentPartIndex = index;
                equippedIndex = index;
                currentActiveSlotImage = partUI.GetComponent<Image>();
                currentActiveSlotImage.sprite = activeSlotImage;
                priceText.text = "Equipped";
                priceText.color = Color.green;
            }
            else if (generatorPart.isPurchasable)
            {
                priceText.text = $"{generatorPart.purchasePrice}$";
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
        OpenPartSelectionWindow("Coolers", PartCategory.Coolers);

        foreach (CoolerStats coolerPart in allCoolerParts)
        {
            if (!coolerPart.isPurchasable && !coolerPart.isOwned)
            {
                continue;
            }

            GameObject partUI = Instantiate(partUIPrefab, partParentContainer.transform);
            TextMeshProUGUI titleText = partUI.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI priceText = partUI.transform.Find("Price").GetComponent<TextMeshProUGUI>();

            partUIs.Add(partUI);
            coolerParts.Add(coolerPart);
            index++;

            titleText.text = coolerPart.partName;

            if (coolerPart.isEquipped)
            {
                currentPartIndex = index;
                equippedIndex = index;
                currentActiveSlotImage = partUI.GetComponent<Image>();
                currentActiveSlotImage.sprite = activeSlotImage;
                priceText.text = "Equipped";
                priceText.color = Color.green;
            }
            else if (coolerPart.isPurchasable)
            {
                priceText.text = $"{coolerPart.purchasePrice}$";
                priceText.color = Color.white;
            }
            else
            {
                priceText.text = "";
            }
        }
    }

    public void DisplayMainWeaponParts()
    {
        OpenPartSelectionWindow("Main Weapons", PartCategory.MainWeapons);

        foreach (WeaponStats mainWeaponsPart in allMainWeaponsParts)
        {
            if (!mainWeaponsPart.isPurchasable && !mainWeaponsPart.isOwned)
            {
                continue;
            }

            GameObject partUI = Instantiate(partUIPrefab, partParentContainer.transform);
            TextMeshProUGUI titleText = partUI.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI priceText = partUI.transform.Find("Price").GetComponent<TextMeshProUGUI>();

            partUIs.Add(partUI);
            mainWeaponsParts.Add(mainWeaponsPart);
            index++;

            titleText.text = mainWeaponsPart.partName;

            if (mainWeaponsPart.isEquipped)
            {
                currentPartIndex = index;
                equippedIndex = index;
                currentActiveSlotImage = partUI.GetComponent<Image>();
                currentActiveSlotImage.sprite = activeSlotImage;
                priceText.text = "Equipped";
                priceText.color = Color.green;
            }
            else if (mainWeaponsPart.isPurchasable)
            {
                priceText.text = $"{mainWeaponsPart.purchasePrice}$";
                priceText.color = Color.white;
            }
            else
            {
                priceText.text = "";
            }
        }
    }

    public void DisplayLeftInnerWeaponParts()
    {
        OpenPartSelectionWindow("Left Inner Weapons", PartCategory.LeftInnerWeapons);

        foreach (WeaponStats leftInnerWeaponPart in allLeftInnerWeaponParts)
        {
            if (!leftInnerWeaponPart.isPurchasable && !leftInnerWeaponPart.isOwned)
            {
                continue;
            }

            GameObject partUI = Instantiate(partUIPrefab, partParentContainer.transform);
            TextMeshProUGUI titleText = partUI.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI priceText = partUI.transform.Find("Price").GetComponent<TextMeshProUGUI>();

            partUIs.Add(partUI);
            leftInnerWeaponParts.Add(leftInnerWeaponPart);
            index++;

            titleText.text = leftInnerWeaponPart.partName;

            if (leftInnerWeaponPart.isEquipped)
            {
                currentPartIndex = index;
                equippedIndex = index;
                currentActiveSlotImage = partUI.GetComponent<Image>();
                currentActiveSlotImage.sprite = activeSlotImage;
                priceText.text = "Equipped";
                priceText.color = Color.green;
            }
            else if (leftInnerWeaponPart.isPurchasable)
            {
                priceText.text = $"{leftInnerWeaponPart.purchasePrice}$";
                priceText.color = Color.white;
            }
            else
            {
                priceText.text = "";
            }
        }
    }

    public void DisplayLeftOuterWeaponParts()
    {
        OpenPartSelectionWindow("Left Outer Weapons", PartCategory.LeftOuterWeapons);

        foreach (WeaponStats leftOuterWeaponPart in allLeftOuterWeaponParts)
        {
            if (!leftOuterWeaponPart.isPurchasable && !leftOuterWeaponPart.isOwned)
            {
                continue;
            }

            GameObject partUI = Instantiate(partUIPrefab, partParentContainer.transform);
            TextMeshProUGUI titleText = partUI.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI priceText = partUI.transform.Find("Price").GetComponent<TextMeshProUGUI>();

            partUIs.Add(partUI);
            leftOuterWeaponParts.Add(leftOuterWeaponPart);
            index++;

            titleText.text = leftOuterWeaponPart.partName;

            if (leftOuterWeaponPart.isEquipped)
            {
                currentPartIndex = index;
                equippedIndex = index;
                currentActiveSlotImage = partUI.GetComponent<Image>();
                currentActiveSlotImage.sprite = activeSlotImage;
                priceText.text = "Equipped";
                priceText.color = Color.green;
            }
            else if (leftOuterWeaponPart.isPurchasable)
            {
                priceText.text = $"{leftOuterWeaponPart.purchasePrice}$";
                priceText.color = Color.white;
            }
            else
            {
                priceText.text = "";
            }
        }
    }

    public void DisplayRightInnerWeaponParts()
    {
        OpenPartSelectionWindow("Right Inner Weapons", PartCategory.RightInnerWeapons);

        foreach (WeaponStats rightInnerWeaponPart in allRightInnerWeaponParts)
        {
            if (!rightInnerWeaponPart.isPurchasable && !rightInnerWeaponPart.isOwned)
            {
                continue;
            }

            GameObject partUI = Instantiate(partUIPrefab, partParentContainer.transform);
            TextMeshProUGUI titleText = partUI.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI priceText = partUI.transform.Find("Price").GetComponent<TextMeshProUGUI>();

            partUIs.Add(partUI);
            rightInnerWeaponParts.Add(rightInnerWeaponPart);
            index++;

            titleText.text = rightInnerWeaponPart.partName;

            if (rightInnerWeaponPart.isEquipped)
            {
                currentPartIndex = index;
                equippedIndex = index;
                currentActiveSlotImage = partUI.GetComponent<Image>();
                currentActiveSlotImage.sprite = activeSlotImage;
                priceText.text = "Equipped";
                priceText.color = Color.green;
            }
            else if (rightInnerWeaponPart.isPurchasable)
            {
                priceText.text = $"{rightInnerWeaponPart.purchasePrice}$";
                priceText.color = Color.white;
            }
            else
            {
                priceText.text = "";
            }
        }
    }

    public void DisplayRightOuterWeaponParts()
    {
        OpenPartSelectionWindow("Right Outer Weapons", PartCategory.RightOuterWeapons);

        foreach (WeaponStats rightOuterWeaponPart in allRightOuterWeaponParts)
        {
            if (!rightOuterWeaponPart.isPurchasable && !rightOuterWeaponPart.isOwned)
            {
                continue;
            }

            GameObject partUI = Instantiate(partUIPrefab, partParentContainer.transform);
            TextMeshProUGUI titleText = partUI.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI priceText = partUI.transform.Find("Price").GetComponent<TextMeshProUGUI>();

            partUIs.Add(partUI);
            rightOuterWeaponParts.Add(rightOuterWeaponPart);
            index++;

            titleText.text = rightOuterWeaponPart.partName;

            if (rightOuterWeaponPart.isEquipped)
            {
                currentPartIndex = index;
                equippedIndex = index;
                currentActiveSlotImage = partUI.GetComponent<Image>();
                currentActiveSlotImage.sprite = activeSlotImage;
                priceText.text = "Equipped";
                priceText.color = Color.green;
            }
            else if (rightOuterWeaponPart.isPurchasable)
            {
                priceText.text = $"{rightOuterWeaponPart.purchasePrice}$";
                priceText.color = Color.white;
            }
            else
            {
                priceText.text = "";
            }
        }
    }

    public void UnlockItem(PartCategory category, string name, bool grantItem)
    {
        if (category == PartCategory.PlaneCores)
        {
            foreach (PlaneStats part in allPlaneParts)
            {
                if (part.partName == name)
                {
                    if (grantItem) { part.isOwned = true; }
                    part.isPurchasable = true;
                    return;
                }
            }
        }
        else if (category == PartCategory.Engines)
        {
            foreach (EngineStats part in allEngineParts)
            {
                if (part.partName == name)
                {
                    if (grantItem) { part.isOwned = true; }
                    part.isPurchasable = true;
                    return;
                }
            }
        }
        else if (category == PartCategory.Generators)
        {
            foreach (GeneratorStats part in allGeneratorParts)
            {
                if (part.partName == name)
                {
                    if (part) { part.isOwned = true; }
                    part.isPurchasable = true;
                    return;
                }
            }
        }
        else if (category == PartCategory.Coolers)
        {
            foreach (CoolerStats part in allCoolerParts)
            {
                if (part.partName == name)
                {
                    if (grantItem) { part.isOwned = true; }
                    part.isPurchasable = true;
                    return;
                }
            }
        }
        else if (category == PartCategory.MainWeapons)
        {
            foreach (WeaponStats part in allMainWeaponsParts)
            {
                if (part.partName == name)
                {
                    if (grantItem) { part.isOwned = true; }
                    part.isPurchasable = true;
                    return;
                }
            }
        }
        else if (category == PartCategory.LeftInnerWeapons)
        {
            foreach (WeaponStats part in allLeftInnerWeaponParts)
            {
                if (part.partName == name)
                {
                    if (grantItem) { part.isOwned = true; }
                    part.isPurchasable = true;
                    return;
                }
            }
        }
        else if (category == PartCategory.LeftOuterWeapons)
        {
            foreach (WeaponStats part in allLeftOuterWeaponParts)
            {
                if (part.partName == name)
                {
                    if (grantItem) { part.isOwned = true; }
                    part.isPurchasable = true;
                    return;
                }
            }
        }
        else if (category == PartCategory.RightInnerWeapons)
        {
            foreach (WeaponStats part in allRightInnerWeaponParts)
            {
                if (part.partName == name)
                {
                    if (grantItem) { part.isOwned = true; }
                    part.isPurchasable = true;
                    return;
                }
            }
        }
        else if (category == PartCategory.RightOuterWeapons)
        {
            foreach (WeaponStats part in allRightOuterWeaponParts)
            {
                if (part.partName == name)
                {
                    if (grantItem) { part.isOwned = true; }
                    part.isPurchasable = true;
                    return;
                }
            }
        }
    }

    private void EquipPart()
    {
        string slotText = partUIs[currentPartIndex].transform.Find("Price").GetComponent<TextMeshProUGUI>().text;

        // Purchase a new part
        if (slotText != string.Empty && slotText != "Equipped")
        {
            if (int.TryParse(Regex.Replace(slotText, @"[^\d]", ""), out int parsedPrice))
            {
                if (playerStats.money >= parsedPrice)
                {
                    playerStats.money -= parsedPrice;
                    moneyText.text = "Money: " + playerStats.money + "$";
                }
                else
                {
                    // TODO: user feedback from insufficient money
                    return;
                }
            }
            else
            {
                // TODO: user feedback from error
                return;
            }
        }

        // Unequip previous part from UI
        partUIs[equippedIndex].transform.Find("Price").GetComponent<TextMeshProUGUI>().text = "";

        // Update equipped data and index
        if (partCategory == PartCategory.PlaneCores)
        {
            planeParts[equippedIndex].isEquipped = false;
            equippedIndex = currentPartIndex;
            planeParts[equippedIndex].isEquipped = true;
            planeParts[equippedIndex].isOwned = true;
            planeParts[equippedIndex].isPurchasable = false;
        }
        else if (partCategory == PartCategory.Engines)
        {
            engineParts[equippedIndex].isEquipped = false;
            equippedIndex = currentPartIndex;
            engineParts[equippedIndex].isEquipped = true;
            engineParts[equippedIndex].isOwned = true;
            engineParts[equippedIndex].isPurchasable = false;
        }
        else if (partCategory == PartCategory.Generators)
        {
            generatorParts[equippedIndex].isEquipped = false;
            equippedIndex = currentPartIndex;
            generatorParts[equippedIndex].isEquipped = true;
            generatorParts[equippedIndex].isOwned = true;
            generatorParts[equippedIndex].isPurchasable = false;
        }
        else if (partCategory == PartCategory.Coolers)
        {
            coolerParts[equippedIndex].isEquipped = false;
            equippedIndex = currentPartIndex;
            coolerParts[equippedIndex].isEquipped = true;
            coolerParts[equippedIndex].isOwned = true;
            coolerParts[equippedIndex].isPurchasable = false;
        }
        else if (partCategory == PartCategory.MainWeapons)
        {
            mainWeaponsParts[equippedIndex].isEquipped = false;
            equippedIndex = currentPartIndex;
            mainWeaponsParts[equippedIndex].isEquipped = true;
            mainWeaponsParts[equippedIndex].isOwned = true;
            mainWeaponsParts[equippedIndex].isPurchasable = false;
        }
        else if (partCategory == PartCategory.LeftInnerWeapons)
        {
            leftInnerWeaponParts[equippedIndex].isEquipped = false;
            equippedIndex = currentPartIndex;
            leftInnerWeaponParts[equippedIndex].isEquipped = true;
            leftInnerWeaponParts[equippedIndex].isOwned = true;
            leftInnerWeaponParts[equippedIndex].isPurchasable = false;
        }
        else if (partCategory == PartCategory.LeftOuterWeapons)
        {
            leftOuterWeaponParts[equippedIndex].isEquipped = false;
            equippedIndex = currentPartIndex;
            leftOuterWeaponParts[equippedIndex].isEquipped = true;
            leftOuterWeaponParts[equippedIndex].isOwned = true;
            leftOuterWeaponParts[equippedIndex].isPurchasable = false;
        }
        else if (partCategory == PartCategory.RightInnerWeapons)
        {
            rightInnerWeaponParts[equippedIndex].isEquipped = false;
            equippedIndex = currentPartIndex;
            rightInnerWeaponParts[equippedIndex].isEquipped = true;
            rightInnerWeaponParts[equippedIndex].isOwned = true;
            rightInnerWeaponParts[equippedIndex].isPurchasable = false;
        }
        else if (partCategory == PartCategory.RightOuterWeapons)
        {
            rightOuterWeaponParts[equippedIndex].isEquipped = false;
            equippedIndex = currentPartIndex;
            rightOuterWeaponParts[equippedIndex].isEquipped = true;
            rightOuterWeaponParts[equippedIndex].isOwned = true;
            rightOuterWeaponParts[equippedIndex].isPurchasable = false;
        }

        // Equip the new part from UI
        partUIs[equippedIndex].transform.Find("Price").GetComponent<TextMeshProUGUI>().text = "Equipped";
        partUIs[equippedIndex].transform.Find("Price").GetComponent<TextMeshProUGUI>().color = Color.green;
    }
}

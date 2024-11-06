using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEditor.Experimental.GraphView;
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
    private List<WeaponStats> mainWeaponParts = new List<WeaponStats>();
    private List<WeaponStats> leftInnerWeaponParts = new List<WeaponStats>();
    private List<WeaponStats> leftOuterWeaponParts = new List<WeaponStats>();
    private List<WeaponStats> rightInnerWeaponParts = new List<WeaponStats>();
    private List<WeaponStats> rightOuterWeaponParts = new List<WeaponStats>();

    [Header("Part Selection UI Elements")]
    [SerializeField] private GameObject partSelectionWindow;
    [SerializeField] private GameObject statsWindow;
    [SerializeField] private GameObject partDetailsWindow;
    [SerializeField] private TextMeshProUGUI partDetailName;
    [SerializeField] private TextMeshProUGUI partDetailManufacturer;
    [SerializeField] private TextMeshProUGUI partDetailInfo;
    [SerializeField] private GameObject partUIPrefab;
    [SerializeField] private TextMeshProUGUI partSelectionTitle;
    [SerializeField] private GameObject partParentContainer;
    [SerializeField] private Button exitPartSelectionButton;

    [Header("Overview UI Elements")]
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private Image planePreviewImage;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private TextMeshProUGUI energyRechargeText;
    [SerializeField] private TextMeshProUGUI physicalDefenceText;
    [SerializeField] private TextMeshProUGUI energyDefenceText;
    [SerializeField] private TextMeshProUGUI horizontalSpeedText;
    [SerializeField] private TextMeshProUGUI verticalSpeedText;
    [SerializeField] private TextMeshProUGUI heatToleranceText;
    [SerializeField] private TextMeshProUGUI idleHeatText;
    [SerializeField] private TextMeshProUGUI coolingText;
    [SerializeField] private TextMeshProUGUI overHeatCoolingText;
    [SerializeField] private TextMeshProUGUI carryWeightText;
    [SerializeField] private TextMeshProUGUI liftWeightText;
    [SerializeField] private TextMeshProUGUI energyOutputText;

    [Header("Overview UI slot image Elements")]
    [SerializeField] private Image equippedPlaneCoreImage;
    [SerializeField] private Image equippedEngineImage;
    [SerializeField] private Image equippedGeneratorImage;
    [SerializeField] private Image equippedCoolerImage;
    [SerializeField] private Image equippedMainGunImage;
    [SerializeField] private Image equippedLeftInnerWeaponImage;
    [SerializeField] private Image equippedLeftOuterWeaponImage;
    [SerializeField] private Image equippedRightInnerWeaponImage;
    [SerializeField] private Image equippedRightOuterWeaponImage;

    [Header("Overview UI name Elements")]
    [SerializeField] private TextMeshProUGUI equippedPlaneCoreNameText;
    [SerializeField] private TextMeshProUGUI equippedEngineNameText;
    [SerializeField] private TextMeshProUGUI equippedGeneratorNameText;
    [SerializeField] private TextMeshProUGUI equippedCoolerNameText;
    [SerializeField] private TextMeshProUGUI equippedMainGunNameText;
    [SerializeField] private TextMeshProUGUI equippedLeftInnerWeaponNameText;
    [SerializeField] private TextMeshProUGUI equippedLeftOuterWeaponNameText;
    [SerializeField] private TextMeshProUGUI equippedRightInnerWeaponNameText;
    [SerializeField] private TextMeshProUGUI equippedRightOuterWeaponNameText;

    [Header("Slot Images")]
    [SerializeField] private Sprite normalSlotImage;
    [SerializeField] private Sprite activeSlotImage;

    private int equippedIndex = -1;
    private int currentPartIndex = -1;
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
        UpdateGarageOverview();
    }

    private void UpdateGarageOverview()
    {
        float _energyConsumption = 0f;
        float _energyOutput = 0f;
        float _repairCost = 0f;
        float _currentLiftWeight = 0f;
        float _maxLiftWeight = 0f;
        float _maxCarryWeight = 0f;
        float _currentCarryWeight = 0f;

        foreach (PlaneStats part in allPlaneParts)
        {
            if (part.isEquipped)
            {
                equippedPlaneCoreNameText.text = part.partName;
                equippedPlaneCoreImage.sprite = part.icon;
                planePreviewImage.sprite = part.sprite;

                playerStats.maxHealth = part.maxHealth;
                playerStats.physicalDefence = part.physicalDefence;
                playerStats.energyDefence = part.energyDefence;
                playerStats.maxHeatTolerance = part.maxHeatTolerance;
                playerStats.idleHeat = part.idleHeat;

                healthText.text = part.maxHealth.ToString();
                physicalDefenceText.text = part.physicalDefence.ToString() + "(TODO: calculate %)";
                energyDefenceText.text = part.energyDefence.ToString() + "(TODO: calculate %)";
                heatToleranceText.text = part.maxHeatTolerance.ToString() + "ºC";
                idleHeatText.text = part.idleHeat.ToString() + "ºC";

                _maxCarryWeight = part.maxCarryWeight;
                _energyConsumption += part.energyConsumption;
                _currentLiftWeight += part.weight;
                _repairCost += part.repairCost;
            }
        }

        foreach (EngineStats part in allEngineParts)
        {
            if (part.isEquipped)
            {
                equippedEngineNameText.text = part.partName;
                equippedEngineImage.sprite = part.icon;

                playerStats.horizontalSpeed = part.horizontalSpeed;
                playerStats.horizontalIdleSpeed = part.horizontalIdleSpeed;
                playerStats.horizontalReverseSpeed = part.horizontalReverseSpeed;
                playerStats.verticalSpeed = part.verticalSpeed;

                horizontalSpeedText.text = part.horizontalSpeed.ToString() + " km/h";
                verticalSpeedText.text = part.verticalSpeed.ToString() + " km/h";

                _maxLiftWeight = part.maxLiftWeight;
                _energyConsumption += part.energyConsumption;
                _currentLiftWeight += part.weight;
                _repairCost += part.repairCost;
            }
        }

        foreach (GeneratorStats part in allGeneratorParts)
        {
            if (part.isEquipped)
            {
                equippedGeneratorNameText.text = part.partName;
                equippedGeneratorImage.sprite = part.icon;

                playerStats.maxEnergy = part.maxEnergy;
                playerStats.energyRecharge = part.energyRecharge;

                energyText.text = part.maxEnergy.ToString();
                energyRechargeText.text = part.energyRecharge.ToString();

                _energyOutput = part.energyOutput;
                _energyConsumption += part.energyConsumption;
                _currentLiftWeight += part.weight;
                _repairCost += part.repairCost;
            }
        }

        foreach (CoolerStats part in allCoolerParts)
        {
            if (part.isEquipped)
            {
                equippedCoolerNameText.text = part.partName;
                equippedCoolerImage.sprite = part.icon;

                playerStats.coolingEfficiency = part.coolingEfficiency;
                playerStats.overHeatcoolingEfficiency = part.overHeatcoolingEfficiency;

                coolingText.text = part.coolingEfficiency.ToString() + " ºC/s";
                overHeatCoolingText.text = part.overHeatcoolingEfficiency.ToString() + " ºC/s";

                _energyConsumption += part.energyConsumption;
                _currentLiftWeight += part.weight;
                _repairCost += part.repairCost;
            }
        }

        foreach (WeaponStats part in allMainWeaponsParts)
        {
            if (part.isEquipped)
            {
                equippedMainGunNameText.text = part.partName;
                equippedMainGunImage.sprite = part.icon;

                _energyConsumption += part.energyConsumption;
                _currentCarryWeight += part.weight;
                _currentLiftWeight += part.weight;
            }
        }

        foreach (WeaponStats part in allLeftInnerWeaponParts)
        {
            if (part.isEquipped)
            {
                equippedLeftInnerWeaponNameText.text = part.partName;
                equippedLeftInnerWeaponImage.sprite = part.icon;

                _energyConsumption += part.energyConsumption;
                _currentCarryWeight += part.weight;
                _currentLiftWeight += part.weight;
            }
        }

        foreach (WeaponStats part in allLeftOuterWeaponParts)
        {
            if (part.isEquipped)
            {
                equippedLeftOuterWeaponNameText.text = part.partName;
                equippedLeftOuterWeaponImage.sprite = part.icon;

                _energyConsumption += part.energyConsumption;
                _currentCarryWeight += part.weight;
                _currentLiftWeight += part.weight;
            }
        }

        foreach (WeaponStats part in allRightInnerWeaponParts)
        {
            if (part.isEquipped)
            {
                equippedRightInnerWeaponNameText.text = part.partName;
                equippedRightInnerWeaponImage.sprite = part.icon;

                _energyConsumption += part.energyConsumption;
                _currentCarryWeight += part.weight;
                _currentLiftWeight += part.weight;
            }
        }

        foreach (WeaponStats part in allRightOuterWeaponParts)
        {
            if (part.isEquipped)
            {
                equippedRightOuterWeaponNameText.text = part.partName;
                equippedRightOuterWeaponImage.sprite = part.icon;

                _energyConsumption += part.energyConsumption;
                _currentCarryWeight += part.weight;
                _currentLiftWeight += part.weight;
            }
        }

        garage.isPlaneMisconfigured = false;

        playerStats.currentCarryWeight = _currentCarryWeight;
        playerStats.maxCarryWeight = _maxCarryWeight;
        playerStats.currentLiftWeight = _currentLiftWeight;
        playerStats.maxLiftWeight = _maxLiftWeight;
        playerStats.energyConsumption = _energyConsumption;
        playerStats.energyOutput = _energyOutput;
        playerStats.repairCost = _repairCost;

        // Weapon Weight / Max Weapon Weight
        if (_currentCarryWeight > _maxCarryWeight)
        {
            garage.isPlaneMisconfigured = true;
            carryWeightText.text = _currentCarryWeight.ToString() + "/" + _maxCarryWeight.ToString() + "";
        }
        else
        {
            carryWeightText.text = _currentCarryWeight.ToString() + "/" + _maxCarryWeight.ToString() + "";
        }

        // Total weight / max total weight
        if (_currentLiftWeight > _maxLiftWeight)
        {
            garage.isPlaneMisconfigured = true;
            liftWeightText.text = _currentLiftWeight.ToString() + "/" + _maxLiftWeight.ToString() + "(TODO: % reduction in speed)";
        }
        else if (_currentLiftWeight <= _maxLiftWeight * 0.75f)
        {
            liftWeightText.text = _currentLiftWeight.ToString() + "/" + _maxLiftWeight.ToString() + "(TODO: % reduction in speed)";
        }
        else
        {
            liftWeightText.text = _currentLiftWeight.ToString() + "/" + _maxLiftWeight.ToString();
        }

        // Energy Consuption / Energy Output
        if (_energyConsumption > _energyOutput)
        {
            energyOutputText.text = _energyConsumption.ToString() + "/" + _energyOutput.ToString() + "(100% reduction in energy)";
        }
        else if (_energyConsumption <= _energyOutput * 0.75f)
        {
            energyOutputText.text = _energyConsumption.ToString() + "/" + _energyOutput.ToString() + "(TODO: % reduction in energy)";
        }
        else
        {
            energyOutputText.text = _energyConsumption.ToString() + "/" + _energyOutput.ToString();
        }
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
        UpdateGarageOverview();

        partSelectionWindow.SetActive(false);
        partDetailsWindow.SetActive(false);
        statsWindow.SetActive(true);
        garage.ispartSelectionWindowOpened = false;
        index = -1;
        equippedIndex = -1;
        currentPartIndex = -1;

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
        mainWeaponParts.Clear();
        leftInnerWeaponParts.Clear();
        leftOuterWeaponParts.Clear();
        rightInnerWeaponParts.Clear();
        rightOuterWeaponParts.Clear();
    }

    private void OpenPartSelectionWindow(string title, PartCategory category)
    {
        partSelectionWindow.SetActive(true);
        partDetailsWindow.SetActive(true);
        statsWindow.SetActive(false);
        garage.ispartSelectionWindowOpened = true;
        partSelectionTitle.text = title;
        partCategory = category;
        UpdateActiveSlot();
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
            currentPartIndex = Mathf.Min(currentPartIndex + 1, partUIs.Count - 1);
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
        else if (equippedIndex == -1)
        {
            partDetailName.text = "";
            partDetailManufacturer.text = "";
            partDetailInfo.text = "";
        }
        else
        {
            currentActiveSlotImage = partUIs[currentPartIndex].GetComponent<Image>();
            currentActiveSlotImage.sprite = activeSlotImage;
            exitPartSelectionButton.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f);

            if (partCategory == PartCategory.PlaneCores)
            {
                partDetailName.text = planeParts[currentPartIndex].partName;
                partDetailManufacturer.text = planeParts[currentPartIndex].manufacturer;
                partDetailInfo.text = "<color=#FFC800><size=25>Stats</size></color>"
                    + "\n\n<b>Health:</b> " + FormatStatDisplay(planeParts[equippedIndex].maxHealth, planeParts[currentPartIndex].maxHealth, "positive")
                    + "\n\n<b>Physical Defence:</b> " + FormatStatDisplay(planeParts[equippedIndex].physicalDefence, planeParts[currentPartIndex].physicalDefence, "positive")
                    + "\n\n<b>Energy Defence:</b> " + FormatStatDisplay(planeParts[equippedIndex].energyDefence, planeParts[currentPartIndex].energyDefence, "positive")
                    + "\n\n<b>Heat Tolerance:</b> " + FormatStatDisplay(planeParts[equippedIndex].maxHeatTolerance, planeParts[currentPartIndex].maxHeatTolerance, "positive")
                    + "\n\n<b>Idle Heat:</b> " + FormatStatDisplay(planeParts[equippedIndex].idleHeat, planeParts[currentPartIndex].idleHeat, "negative")
                    + "\n\n<b>Carry Weight:</b> " + FormatStatDisplay(planeParts[equippedIndex].maxCarryWeight, planeParts[currentPartIndex].maxCarryWeight, "positive")
                    + "\n\n<b>Weight:</b> " + FormatStatDisplay(planeParts[equippedIndex].weight, planeParts[currentPartIndex].weight, "negative")
                    + "\n\n<b>Energy Consumption:</b> " + FormatStatDisplay(planeParts[equippedIndex].energyConsumption, planeParts[currentPartIndex].energyConsumption, "negative")
                    + "\n\n<b>Repair Cost:</b> " + FormatStatDisplay(planeParts[equippedIndex].repairCost, planeParts[currentPartIndex].repairCost, "negative")
                    + "\n\n\n" + planeParts[currentPartIndex].description;
            }
            else if (partCategory == PartCategory.Engines)
            {
                partDetailName.text = engineParts[currentPartIndex].partName;
                partDetailManufacturer.text = engineParts[currentPartIndex].manufacturer;
                partDetailInfo.text = "<color=#FFC800><size=25>Stats</size></color>"
                    + "\n\n<b>Horizontal Speed:</b> " + FormatStatDisplay(engineParts[equippedIndex].horizontalSpeed, engineParts[currentPartIndex].horizontalSpeed, "positive")
                    + "\n\n<b>Vertical Speed:</b> " + FormatStatDisplay(engineParts[equippedIndex].verticalSpeed, engineParts[currentPartIndex].verticalSpeed, "positive")
                    + "\n\n<b>Lift Weight:</b> " + FormatStatDisplay(engineParts[equippedIndex].maxLiftWeight, engineParts[currentPartIndex].maxLiftWeight, "positive")
                    + "\n\n<b>Weight:</b> " + FormatStatDisplay(engineParts[equippedIndex].weight, engineParts[currentPartIndex].weight, "negative")
                    + "\n\n<b>Energy Consumption:</b> " + FormatStatDisplay(engineParts[equippedIndex].energyConsumption, engineParts[currentPartIndex].energyConsumption, "negative")
                    + "\n\n<b>Repair Cost:</b> " + FormatStatDisplay(engineParts[equippedIndex].repairCost, engineParts[currentPartIndex].repairCost, "negative")
                    + "\n\n\n" + engineParts[currentPartIndex].description;
            }
            else if (partCategory == PartCategory.Generators)
            {
                partDetailName.text = generatorParts[currentPartIndex].partName;
                partDetailManufacturer.text = generatorParts[currentPartIndex].manufacturer;
                partDetailInfo.text = "<color=#FFC800><size=25>Stats</size></color>"
                    + "\n\n<b>Energy:</b> " + FormatStatDisplay(generatorParts[equippedIndex].maxEnergy, generatorParts[currentPartIndex].maxEnergy, "positive")
                    + "\n\n<b>Energy Recharge:</b> " + FormatStatDisplay(generatorParts[equippedIndex].energyRecharge, generatorParts[currentPartIndex].energyRecharge, "positive")
                    + "\n\n<b>Energy Output:</b> " + FormatStatDisplay(generatorParts[equippedIndex].energyOutput, generatorParts[currentPartIndex].energyOutput, "positive")
                    + "\n\n<b>Weight:</b> " + FormatStatDisplay(generatorParts[equippedIndex].weight, generatorParts[currentPartIndex].weight, "negative")
                    + "\n\n<b>Energy Consumption:</b> " + FormatStatDisplay(generatorParts[equippedIndex].energyConsumption, generatorParts[currentPartIndex].energyConsumption, "negative")
                    + "\n\n<b>Repair Cost:</b> " + FormatStatDisplay(generatorParts[equippedIndex].repairCost, generatorParts[currentPartIndex].repairCost, "negative")
                    + "\n\n\n" + generatorParts[currentPartIndex].description;
            }
            else if (partCategory == PartCategory.Coolers)
            {
                partDetailName.text = coolerParts[currentPartIndex].partName;
                partDetailManufacturer.text = coolerParts[currentPartIndex].manufacturer;
                partDetailInfo.text = "<color=#FFC800><size=25>Stats</size></color>"
                    + "\n\n<b>Cooling:</b> " + FormatStatDisplay(coolerParts[equippedIndex].coolingEfficiency, coolerParts[currentPartIndex].coolingEfficiency, "positive")
                    + "\n\n<b>Over Heat Cooling:</b> " + FormatStatDisplay(coolerParts[equippedIndex].overHeatcoolingEfficiency, coolerParts[currentPartIndex].overHeatcoolingEfficiency, "positive")
                    + "\n\n<b>Weight:</b> " + FormatStatDisplay(coolerParts[equippedIndex].weight, coolerParts[currentPartIndex].weight, "negative")
                    + "\n\n<b>Energy Consumption:</b> " + FormatStatDisplay(coolerParts[equippedIndex].energyConsumption, coolerParts[currentPartIndex].energyConsumption, "negative")
                    + "\n\n<b>Repair Cost:</b> " + FormatStatDisplay(coolerParts[equippedIndex].repairCost, coolerParts[currentPartIndex].repairCost, "negative")
                    + "\n\n\n" + coolerParts[currentPartIndex].description;
            }
            else if (partCategory == PartCategory.MainWeapons)
            {
                partDetailName.text = mainWeaponParts[currentPartIndex].partName;
                partDetailManufacturer.text = mainWeaponParts[currentPartIndex].manufacturer;
                partDetailInfo.text = "<color=#FFC800><size=25>Stats</size></color>"
                    + "\n\n<b>Min Damage:</b> " + FormatStatDisplay(mainWeaponParts[equippedIndex].minDamage, mainWeaponParts[currentPartIndex].minDamage, "positive")
                    + "\n<b>Max Damage:</b> " + FormatStatDisplay(mainWeaponParts[equippedIndex].maxDamage, mainWeaponParts[currentPartIndex].maxDamage, "positive")
                    + "\n<b>Critical Chance:</b> " + FormatStatDisplay(mainWeaponParts[equippedIndex].criticalChance, mainWeaponParts[currentPartIndex].criticalChance, "positive")
                    + "\n<b>Critical Damage:</b> " + FormatStatDisplay(mainWeaponParts[equippedIndex].criticalDamageMultiplier, mainWeaponParts[currentPartIndex].criticalDamageMultiplier, "positive")
                    + "\n<b>Fire Rate:</b> " + FormatStatDisplay(mainWeaponParts[equippedIndex].fireRate, mainWeaponParts[currentPartIndex].fireRate, "positive")
                    + "\n<b>Reload Speed:</b> " + FormatStatDisplay(mainWeaponParts[equippedIndex].reloadSpeed, mainWeaponParts[currentPartIndex].reloadSpeed, "positive")
                    + "\n<b>Is Auto:</b> " + FormatStatDisplayBool(mainWeaponParts[equippedIndex].isAuto)
                    + "\n<b>Magazine Size:</b> " + FormatStatDisplay(mainWeaponParts[equippedIndex].magazineSize, mainWeaponParts[currentPartIndex].magazineSize, "positive")
                    + "\n<b>Total Rounds:</b> " + FormatStatDisplay(mainWeaponParts[equippedIndex].totalRounds, mainWeaponParts[currentPartIndex].totalRounds, "positive")
                    + "\n<b>Heating:</b> " + FormatStatDisplay(mainWeaponParts[equippedIndex].heating, mainWeaponParts[currentPartIndex].heating, "negative")
                    + "\n<b>Weight:</b> " + FormatStatDisplay(mainWeaponParts[equippedIndex].weight, mainWeaponParts[currentPartIndex].weight, "negative")
                    + "\n<b>Energy Consumption:</b> " + FormatStatDisplay(mainWeaponParts[equippedIndex].energyConsumption, mainWeaponParts[currentPartIndex].energyConsumption, "negative")
                    + "\n<b>Energy Cost:</b> " + FormatStatDisplay(mainWeaponParts[equippedIndex].enegryCost, mainWeaponParts[currentPartIndex].enegryCost, "negative")
                    + "\n<b>Ammo Cost:</b> " + FormatStatDisplay(mainWeaponParts[equippedIndex].ammunitionCost, mainWeaponParts[currentPartIndex].ammunitionCost, "negative")
                    + "\n\n\n" + mainWeaponParts[currentPartIndex].description;
            }
            else if (partCategory == PartCategory.LeftInnerWeapons)
            {
                partDetailName.text = leftInnerWeaponParts[currentPartIndex].partName;
                partDetailManufacturer.text = leftInnerWeaponParts[currentPartIndex].manufacturer;
                partDetailInfo.text = "<color=#FFC800><size=25>Stats</size></color>"
                    + "\n\n<b>Min Damage:</b> " + FormatStatDisplay(leftInnerWeaponParts[equippedIndex].minDamage, leftInnerWeaponParts[currentPartIndex].minDamage, "positive")
                    + "\n<b>Max Damage:</b> " + FormatStatDisplay(leftInnerWeaponParts[equippedIndex].maxDamage, leftInnerWeaponParts[currentPartIndex].maxDamage, "positive")
                    + "\n<b>Critical Chance:</b> " + FormatStatDisplay(leftInnerWeaponParts[equippedIndex].criticalChance, leftInnerWeaponParts[currentPartIndex].criticalChance, "positive")
                    + "\n<b>Critical Damage:</b> " + FormatStatDisplay(leftInnerWeaponParts[equippedIndex].criticalDamageMultiplier, leftInnerWeaponParts[currentPartIndex].criticalDamageMultiplier, "positive")
                    + "\n<b>Fire Rate:</b> " + FormatStatDisplay(leftInnerWeaponParts[equippedIndex].fireRate, leftInnerWeaponParts[currentPartIndex].fireRate, "positive")
                    + "\n<b>Reload Speed:</b> " + FormatStatDisplay(leftInnerWeaponParts[equippedIndex].reloadSpeed, leftInnerWeaponParts[currentPartIndex].reloadSpeed, "positive")
                    + "\n<b>Is Auto:</b> " + FormatStatDisplayBool(leftInnerWeaponParts[equippedIndex].isAuto)
                    + "\n<b>Magazine Size:</b> " + FormatStatDisplay(leftInnerWeaponParts[equippedIndex].magazineSize, leftInnerWeaponParts[currentPartIndex].magazineSize, "positive")
                    + "\n<b>Total Rounds:</b> " + FormatStatDisplay(leftInnerWeaponParts[equippedIndex].totalRounds, leftInnerWeaponParts[currentPartIndex].totalRounds, "positive")
                    + "\n<b>Heating:</b> " + FormatStatDisplay(leftInnerWeaponParts[equippedIndex].heating, leftInnerWeaponParts[currentPartIndex].heating, "negative")
                    + "\n<b>Weight:</b> " + FormatStatDisplay(leftInnerWeaponParts[equippedIndex].weight, leftInnerWeaponParts[currentPartIndex].weight, "negative")
                    + "\n<b>Energy Consumption:</b> " + FormatStatDisplay(leftInnerWeaponParts[equippedIndex].energyConsumption, leftInnerWeaponParts[currentPartIndex].energyConsumption, "negative")
                    + "\n<b>Energy Cost:</b> " + FormatStatDisplay(leftInnerWeaponParts[equippedIndex].enegryCost, leftInnerWeaponParts[currentPartIndex].enegryCost, "negative")
                    + "\n<b>Ammo Cost:</b> " + FormatStatDisplay(leftInnerWeaponParts[equippedIndex].ammunitionCost, leftInnerWeaponParts[currentPartIndex].ammunitionCost, "negative")
                    + "\n\n\n" + leftInnerWeaponParts[currentPartIndex].description;
            }
            else if (partCategory == PartCategory.LeftOuterWeapons)
            {
                partDetailName.text = leftOuterWeaponParts[currentPartIndex].partName;
                partDetailManufacturer.text = leftOuterWeaponParts[currentPartIndex].manufacturer;
                partDetailInfo.text = "<color=#FFC800><size=25>Stats</size></color>"
                    + "\n\n<b>Min Damage:</b> " + FormatStatDisplay(leftOuterWeaponParts[equippedIndex].minDamage, leftOuterWeaponParts[currentPartIndex].minDamage, "positive")
                    + "\n<b>Max Damage:</b> " + FormatStatDisplay(leftOuterWeaponParts[equippedIndex].maxDamage, leftOuterWeaponParts[currentPartIndex].maxDamage, "positive")
                    + "\n<b>Critical Chance:</b> " + FormatStatDisplay(leftOuterWeaponParts[equippedIndex].criticalChance, leftOuterWeaponParts[currentPartIndex].criticalChance, "positive")
                    + "\n<b>Critical Damage:</b> " + FormatStatDisplay(leftOuterWeaponParts[equippedIndex].criticalDamageMultiplier, leftOuterWeaponParts[currentPartIndex].criticalDamageMultiplier, "positive")
                    + "\n<b>Fire Rate:</b> " + FormatStatDisplay(leftOuterWeaponParts[equippedIndex].fireRate, leftOuterWeaponParts[currentPartIndex].fireRate, "positive")
                    + "\n<b>Reload Speed:</b> " + FormatStatDisplay(leftOuterWeaponParts[equippedIndex].reloadSpeed, leftOuterWeaponParts[currentPartIndex].reloadSpeed, "positive")
                    + "\n<b>Is Auto:</b> " + FormatStatDisplayBool(leftOuterWeaponParts[equippedIndex].isAuto)
                    + "\n<b>Magazine Size:</b> " + FormatStatDisplay(leftOuterWeaponParts[equippedIndex].magazineSize, leftOuterWeaponParts[currentPartIndex].magazineSize, "positive")
                    + "\n<b>Total Rounds:</b> " + FormatStatDisplay(leftOuterWeaponParts[equippedIndex].totalRounds, leftOuterWeaponParts[currentPartIndex].totalRounds, "positive")
                    + "\n<b>Heating:</b> " + FormatStatDisplay(leftOuterWeaponParts[equippedIndex].heating, leftOuterWeaponParts[currentPartIndex].heating, "negative")
                    + "\n<b>Weight:</b> " + FormatStatDisplay(leftOuterWeaponParts[equippedIndex].weight, leftOuterWeaponParts[currentPartIndex].weight, "negative")
                    + "\n<b>Energy Consumption:</b> " + FormatStatDisplay(leftOuterWeaponParts[equippedIndex].energyConsumption, leftOuterWeaponParts[currentPartIndex].energyConsumption, "negative")
                    + "\n<b>Energy Cost:</b> " + FormatStatDisplay(leftOuterWeaponParts[equippedIndex].enegryCost, leftOuterWeaponParts[currentPartIndex].enegryCost, "negative")
                    + "\n<b>Ammo Cost:</b> " + FormatStatDisplay(leftOuterWeaponParts[equippedIndex].ammunitionCost, leftOuterWeaponParts[currentPartIndex].ammunitionCost, "negative")
                    + "\n\n\n" + leftOuterWeaponParts[currentPartIndex].description;
            }
            else if (partCategory == PartCategory.RightInnerWeapons)
            {
                partDetailName.text = rightInnerWeaponParts[currentPartIndex].partName;
                partDetailManufacturer.text = rightInnerWeaponParts[currentPartIndex].manufacturer;
                partDetailInfo.text = "<color=#FFC800><size=25>Stats</size></color>"
                    + "\n\n<b>Min Damage:</b> " + FormatStatDisplay(rightInnerWeaponParts[equippedIndex].minDamage, rightInnerWeaponParts[currentPartIndex].minDamage, "positive")
                    + "\n<b>Max Damage:</b> " + FormatStatDisplay(rightInnerWeaponParts[equippedIndex].maxDamage, rightInnerWeaponParts[currentPartIndex].maxDamage, "positive")
                    + "\n<b>Critical Chance:</b> " + FormatStatDisplay(rightInnerWeaponParts[currentPartIndex].criticalChance, rightInnerWeaponParts[currentPartIndex].criticalChance, "positive")
                    + "\n<b>Critical Damage:</b> " + FormatStatDisplay(rightInnerWeaponParts[equippedIndex].criticalDamageMultiplier, rightInnerWeaponParts[currentPartIndex].criticalDamageMultiplier, "positive")
                    + "\n<b>Fire Rate:</b> " + FormatStatDisplay(rightInnerWeaponParts[equippedIndex].fireRate, rightInnerWeaponParts[currentPartIndex].fireRate, "positive")
                    + "\n<b>Reload Speed:</b> " + FormatStatDisplay(rightInnerWeaponParts[equippedIndex].reloadSpeed, rightInnerWeaponParts[currentPartIndex].reloadSpeed, "positive")
                    + "\n<b>Is Auto:</b> " + FormatStatDisplayBool(rightInnerWeaponParts[equippedIndex].isAuto)
                    + "\n<b>Magazine Size:</b> " + FormatStatDisplay(rightInnerWeaponParts[equippedIndex].magazineSize, rightInnerWeaponParts[currentPartIndex].magazineSize, "positive")
                    + "\n<b>Total Rounds:</b> " + FormatStatDisplay(rightInnerWeaponParts[equippedIndex].totalRounds, rightInnerWeaponParts[currentPartIndex].totalRounds, "positive")
                    + "\n<b>Heating:</b> " + FormatStatDisplay(rightInnerWeaponParts[equippedIndex].heating, rightInnerWeaponParts[currentPartIndex].heating, "negative")
                    + "\n<b>Weight:</b> " + FormatStatDisplay(rightInnerWeaponParts[equippedIndex].weight, rightInnerWeaponParts[currentPartIndex].weight, "negative")
                    + "\n<b>Energy Consumption:</b> " + FormatStatDisplay(rightInnerWeaponParts[equippedIndex].energyConsumption, rightInnerWeaponParts[currentPartIndex].energyConsumption, "negative")
                    + "\n<b>Energy Cost:</b> " + FormatStatDisplay(rightInnerWeaponParts[equippedIndex].enegryCost, rightInnerWeaponParts[currentPartIndex].enegryCost, "negative")
                    + "\n<b>Ammo Cost:</b> " + FormatStatDisplay(rightInnerWeaponParts[equippedIndex].ammunitionCost, rightInnerWeaponParts[currentPartIndex].ammunitionCost, "negative")
                    + "\n\n\n" + rightInnerWeaponParts[currentPartIndex].description;
            }
            else if (partCategory == PartCategory.RightOuterWeapons)
            {
                partDetailName.text = rightOuterWeaponParts[currentPartIndex].partName;
                partDetailManufacturer.text = rightOuterWeaponParts[currentPartIndex].manufacturer;
                partDetailInfo.text = "<color=#FFC800><size=25>Stats</size></color>"
                    + "\n\n<b>Min Damage:</b> " + FormatStatDisplay(rightOuterWeaponParts[equippedIndex].minDamage, rightOuterWeaponParts[currentPartIndex].minDamage, "positive")
                    + "\n<b>Max Damage:</b> " + FormatStatDisplay(rightOuterWeaponParts[equippedIndex].maxDamage, rightOuterWeaponParts[currentPartIndex].maxDamage, "positive")
                    + "\n<b>Critical Chance:</b> " + FormatStatDisplay(rightOuterWeaponParts[equippedIndex].criticalChance, rightOuterWeaponParts[currentPartIndex].criticalChance, "positive")
                    + "\n<b>Critical Damage:</b> " + FormatStatDisplay(rightOuterWeaponParts[equippedIndex].criticalDamageMultiplier, rightOuterWeaponParts[currentPartIndex].criticalDamageMultiplier, "positive")
                    + "\n<b>Fire Rate:</b> " + FormatStatDisplay(rightOuterWeaponParts[equippedIndex].fireRate, rightOuterWeaponParts[currentPartIndex].fireRate, "positive")
                    + "\n<b>Reload Speed:</b> " + FormatStatDisplay(rightOuterWeaponParts[equippedIndex].reloadSpeed, rightOuterWeaponParts[currentPartIndex].reloadSpeed, "positive")
                    + "\n<b>Is Auto:</b> " + FormatStatDisplayBool(rightOuterWeaponParts[equippedIndex].isAuto)
                    + "\n<b>Magazine Size:</b> " + FormatStatDisplay(rightOuterWeaponParts[equippedIndex].magazineSize, rightOuterWeaponParts[currentPartIndex].magazineSize, "positive")
                    + "\n<b>Total Rounds:</b> " + FormatStatDisplay(rightOuterWeaponParts[equippedIndex].totalRounds, rightOuterWeaponParts[currentPartIndex].totalRounds, "positive")
                    + "\n<b>Heating:</b> " + FormatStatDisplay(rightOuterWeaponParts[equippedIndex].heating, rightOuterWeaponParts[currentPartIndex].heating, "negative")
                    + "\n<b>Weight:</b> " + FormatStatDisplay(rightOuterWeaponParts[equippedIndex].weight, rightOuterWeaponParts[currentPartIndex].weight, "negative")
                    + "\n<b>Energy Consumption:</b> " + FormatStatDisplay(rightOuterWeaponParts[equippedIndex].energyConsumption, rightOuterWeaponParts[currentPartIndex].energyConsumption, "negative")
                    + "\n<b>Energy Cost:</b> " + FormatStatDisplay(rightOuterWeaponParts[equippedIndex].enegryCost, rightOuterWeaponParts[currentPartIndex].enegryCost, "negative")
                    + "\n<b>Ammo Cost:</b> " + FormatStatDisplay(rightOuterWeaponParts[equippedIndex].ammunitionCost, rightOuterWeaponParts[currentPartIndex].ammunitionCost, "negative")
                    + "\n\n\n" + rightOuterWeaponParts[currentPartIndex].description;
            }
        }
    }

    private string FormatStatDisplayBool(bool boolean)
    {
        if (boolean)
        {
            return "Yes";
        }

        return "No";
    }

    private string FormatStatDisplay(float original, float current, string method)
    {
        if (equippedIndex == currentPartIndex)
        {
            return $"<color=#C8C8C8>{original}</color>";
        }
        else
        {
            return $"<color=#C8C8C8>{original} > </color>{ComparePartStats(original, current, method)}";
        }
    }

    private string ComparePartStats(float original, float current, string method)
    {
        if (current == original)
        {
            return $"<color=#C8C8C8>{current}</color>"; // Default color
        }

        if (method == "positive")
        {
            if (current > original)
            {
                return $"<color=#00FF00>{current}</color>"; // Green color
            }
            else
            {
                return $"<color=#FF0000>{current}</color>"; // Red color
            }
        }
        else
        {
            if (current < original)
            {
                return $"<color=#00FF00>{current}</color>"; // Green color
            }
            else
            {
                return $"<color=#FF0000>{current}</color>"; // Red color
            }
        }
    }

    public void DisplayPlaneParts()
    {
        bool hasAnythingEquipped = false;

        foreach (PlaneStats planePart in allPlaneParts)
        {
            if (!planePart.isPurchasable && !planePart.isOwned)
            {
                continue;
            }

            GameObject partUI = Instantiate(partUIPrefab, partParentContainer.transform);
            Image icon = partUI.transform.Find("Icon").GetComponent<Image>();
            TextMeshProUGUI titleText = partUI.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI priceText = partUI.transform.Find("Price").GetComponent<TextMeshProUGUI>();

            partUIs.Add(partUI);
            planeParts.Add(planePart);
            index++;

            icon.sprite = planePart.icon;
            titleText.text = planePart.partName;

            if (equippedIndex == -1 && planePart.isEquipped)
            {
                hasAnythingEquipped = true;
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

        if (!hasAnythingEquipped)
        {
            currentPartIndex = 0;
        }

        OpenPartSelectionWindow("Plane Cores", PartCategory.PlaneCores);
    }

    public void DisplayEngineParts()
    {
        bool hasAnythingEquipped = false;

        foreach (EngineStats enginePart in allEngineParts)
        {
            if (!enginePart.isPurchasable && !enginePart.isOwned)
            {
                continue;
            }

            GameObject partUI = Instantiate(partUIPrefab, partParentContainer.transform);
            Image icon = partUI.transform.Find("Icon").GetComponent<Image>();
            TextMeshProUGUI titleText = partUI.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI priceText = partUI.transform.Find("Price").GetComponent<TextMeshProUGUI>();

            partUIs.Add(partUI);
            engineParts.Add(enginePart);
            index++;

            titleText.text = enginePart.partName;
            icon.sprite = enginePart.icon;

            if (equippedIndex == -1 && enginePart.isEquipped)
            {
                hasAnythingEquipped = true;
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

        if (!hasAnythingEquipped)
        {
            currentPartIndex = 0;
        }

        OpenPartSelectionWindow("Engines", PartCategory.Engines);
    }

    public void DisplayGeneratorParts()
    {
        bool hasAnythingEquipped = false;

        foreach (GeneratorStats generatorPart in allGeneratorParts)
        {
            if (!generatorPart.isPurchasable && !generatorPart.isOwned)
            {
                continue;
            }

            GameObject partUI = Instantiate(partUIPrefab, partParentContainer.transform);
            Image icon = partUI.transform.Find("Icon").GetComponent<Image>();
            TextMeshProUGUI titleText = partUI.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI priceText = partUI.transform.Find("Price").GetComponent<TextMeshProUGUI>();

            partUIs.Add(partUI);
            generatorParts.Add(generatorPart);
            index++;

            icon.sprite = generatorPart.icon;
            titleText.text = generatorPart.partName;

            if (equippedIndex == -1 && generatorPart.isEquipped)
            {
                hasAnythingEquipped = true;
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

        if (!hasAnythingEquipped)
        {
            currentPartIndex = 0;
        }

        OpenPartSelectionWindow("Generators", PartCategory.Generators);
    }

    public void DisplayCoolerParts()
    {
        bool hasAnythingEquipped = false;

        foreach (CoolerStats coolerPart in allCoolerParts)
        {
            if (!coolerPart.isPurchasable && !coolerPart.isOwned)
            {
                continue;
            }

            GameObject partUI = Instantiate(partUIPrefab, partParentContainer.transform);
            Image icon = partUI.transform.Find("Icon").GetComponent<Image>();
            TextMeshProUGUI titleText = partUI.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI priceText = partUI.transform.Find("Price").GetComponent<TextMeshProUGUI>();

            partUIs.Add(partUI);
            coolerParts.Add(coolerPart);
            index++;

            icon.sprite = coolerPart.icon;
            titleText.text = coolerPart.partName;

            if (equippedIndex == -1 && coolerPart.isEquipped)
            {
                hasAnythingEquipped = true;
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

        if (!hasAnythingEquipped)
        {
            currentPartIndex = 0;
        }

        OpenPartSelectionWindow("Coolers", PartCategory.Coolers);
    }

    public void DisplayMainWeaponParts()
    {
        bool hasAnythingEquipped = false;

        foreach (WeaponStats mainWeaponsPart in allMainWeaponsParts)
        {
            if (!mainWeaponsPart.isPurchasable && !mainWeaponsPart.isOwned)
            {
                continue;
            }

            GameObject partUI = Instantiate(partUIPrefab, partParentContainer.transform);
            Image icon = partUI.transform.Find("Icon").GetComponent<Image>();
            TextMeshProUGUI titleText = partUI.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI priceText = partUI.transform.Find("Price").GetComponent<TextMeshProUGUI>();

            partUIs.Add(partUI);
            mainWeaponParts.Add(mainWeaponsPart);
            index++;

            icon.sprite = mainWeaponsPart.icon;
            titleText.text = mainWeaponsPart.partName;

            if (equippedIndex == -1 && mainWeaponsPart.isEquipped)
            {
                hasAnythingEquipped = true;
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

        if (!hasAnythingEquipped)
        {
            currentPartIndex = 0;
        }

        OpenPartSelectionWindow("Main Weapons", PartCategory.MainWeapons);
    }

    public void DisplayLeftInnerWeaponParts()
    {
        bool hasAnythingEquipped = false;

        foreach (WeaponStats leftInnerWeaponPart in allLeftInnerWeaponParts)
        {
            if (!leftInnerWeaponPart.isPurchasable && !leftInnerWeaponPart.isOwned)
            {
                continue;
            }

            GameObject partUI = Instantiate(partUIPrefab, partParentContainer.transform);
            Image icon = partUI.transform.Find("Icon").GetComponent<Image>();
            TextMeshProUGUI titleText = partUI.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI priceText = partUI.transform.Find("Price").GetComponent<TextMeshProUGUI>();

            partUIs.Add(partUI);
            leftInnerWeaponParts.Add(leftInnerWeaponPart);
            index++;

            icon.sprite = leftInnerWeaponPart.icon;
            titleText.text = leftInnerWeaponPart.partName;

            if (equippedIndex == -1 && leftInnerWeaponPart.isEquipped)
            {
                hasAnythingEquipped = true;
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

        if (!hasAnythingEquipped)
        {
            currentPartIndex = 0;
        }

        OpenPartSelectionWindow("Left Inner Weapons", PartCategory.LeftInnerWeapons);
    }

    public void DisplayLeftOuterWeaponParts()
    {
        bool hasAnythingEquipped = false;

        foreach (WeaponStats leftOuterWeaponPart in allLeftOuterWeaponParts)
        {
            if (!leftOuterWeaponPart.isPurchasable && !leftOuterWeaponPart.isOwned)
            {
                continue;
            }

            GameObject partUI = Instantiate(partUIPrefab, partParentContainer.transform);
            Image icon = partUI.transform.Find("Icon").GetComponent<Image>();
            TextMeshProUGUI titleText = partUI.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI priceText = partUI.transform.Find("Price").GetComponent<TextMeshProUGUI>();

            partUIs.Add(partUI);
            leftOuterWeaponParts.Add(leftOuterWeaponPart);
            index++;

            icon.sprite = leftOuterWeaponPart.icon;
            titleText.text = leftOuterWeaponPart.partName;

            if (equippedIndex == -1 && leftOuterWeaponPart.isEquipped)
            {
                hasAnythingEquipped = true;
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

        if (!hasAnythingEquipped)
        {
            currentPartIndex = 0;
        }

        OpenPartSelectionWindow("Left Outer Weapons", PartCategory.LeftOuterWeapons);
    }

    public void DisplayRightInnerWeaponParts()
    {
        bool hasAnythingEquipped = false;

        foreach (WeaponStats rightInnerWeaponPart in allRightInnerWeaponParts)
        {
            if (!rightInnerWeaponPart.isPurchasable && !rightInnerWeaponPart.isOwned)
            {
                continue;
            }

            GameObject partUI = Instantiate(partUIPrefab, partParentContainer.transform);
            Image icon = partUI.transform.Find("Icon").GetComponent<Image>();
            TextMeshProUGUI titleText = partUI.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI priceText = partUI.transform.Find("Price").GetComponent<TextMeshProUGUI>();

            partUIs.Add(partUI);
            rightInnerWeaponParts.Add(rightInnerWeaponPart);
            index++;

            icon.sprite = rightInnerWeaponPart.icon;
            titleText.text = rightInnerWeaponPart.partName;

            if (equippedIndex == -1 && rightInnerWeaponPart.isEquipped)
            {
                hasAnythingEquipped = true;
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

        if (!hasAnythingEquipped)
        {
            currentPartIndex = 0;
        }

        OpenPartSelectionWindow("Right Inner Weapons", PartCategory.RightInnerWeapons);
    }

    public void DisplayRightOuterWeaponParts()
    {
        bool hasAnythingEquipped = false;

        foreach (WeaponStats rightOuterWeaponPart in allRightOuterWeaponParts)
        {
            if (!rightOuterWeaponPart.isPurchasable && !rightOuterWeaponPart.isOwned)
            {
                continue;
            }

            GameObject partUI = Instantiate(partUIPrefab, partParentContainer.transform);
            Image icon = partUI.transform.Find("Icon").GetComponent<Image>();
            TextMeshProUGUI titleText = partUI.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI priceText = partUI.transform.Find("Price").GetComponent<TextMeshProUGUI>();

            partUIs.Add(partUI);
            rightOuterWeaponParts.Add(rightOuterWeaponPart);
            index++;

            icon.sprite = rightOuterWeaponPart.icon;
            titleText.text = rightOuterWeaponPart.partName;

            if (equippedIndex == -1 && rightOuterWeaponPart.isEquipped)
            {
                hasAnythingEquipped = true;
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

        if (!hasAnythingEquipped)
        {
            currentPartIndex = 0;
        }

        OpenPartSelectionWindow("Right Outer Weapons", PartCategory.RightOuterWeapons);
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
        if (equippedIndex != -1)
        {
            partUIs[equippedIndex].transform.Find("Price").GetComponent<TextMeshProUGUI>().text = "";
        }

        // Update equipped data and index
        if (partCategory == PartCategory.PlaneCores)
        {
            if (equippedIndex != -1)
            {
                planeParts[equippedIndex].isEquipped = false;
            }

            equippedIndex = currentPartIndex;
            planeParts[equippedIndex].isEquipped = true;
            planeParts[equippedIndex].isOwned = true;
            planeParts[equippedIndex].isPurchasable = false;
        }
        else if (partCategory == PartCategory.Engines)
        {
            if (equippedIndex != -1)
            {
                engineParts[equippedIndex].isEquipped = false;
            }

            equippedIndex = currentPartIndex;
            engineParts[equippedIndex].isEquipped = true;
            engineParts[equippedIndex].isOwned = true;
            engineParts[equippedIndex].isPurchasable = false;
        }
        else if (partCategory == PartCategory.Generators)
        {
            if (equippedIndex != -1)
            {
                generatorParts[equippedIndex].isEquipped = false;
            }

            equippedIndex = currentPartIndex;
            generatorParts[equippedIndex].isEquipped = true;
            generatorParts[equippedIndex].isOwned = true;
            generatorParts[equippedIndex].isPurchasable = false;
        }
        else if (partCategory == PartCategory.Coolers)
        {
            if (equippedIndex != -1)
            {
                coolerParts[equippedIndex].isEquipped = false;
            }

            equippedIndex = currentPartIndex;
            coolerParts[equippedIndex].isEquipped = true;
            coolerParts[equippedIndex].isOwned = true;
            coolerParts[equippedIndex].isPurchasable = false;
        }
        else if (partCategory == PartCategory.MainWeapons)
        {
            if (equippedIndex != -1)
            {
                mainWeaponParts[equippedIndex].isEquipped = false;
            }

            equippedIndex = currentPartIndex;
            mainWeaponParts[equippedIndex].isEquipped = true;
            mainWeaponParts[equippedIndex].isOwned = true;
            mainWeaponParts[equippedIndex].isPurchasable = false;
        }
        else if (partCategory == PartCategory.LeftInnerWeapons)
        {
            if (equippedIndex != -1)
            {
                leftInnerWeaponParts[equippedIndex].isEquipped = false;
            }

            equippedIndex = currentPartIndex;
            leftInnerWeaponParts[equippedIndex].isEquipped = true;
            leftInnerWeaponParts[equippedIndex].isOwned = true;
            leftInnerWeaponParts[equippedIndex].isPurchasable = false;
        }
        else if (partCategory == PartCategory.LeftOuterWeapons)
        {
            if (equippedIndex != -1)
            {
                leftOuterWeaponParts[equippedIndex].isEquipped = false;
            }

            equippedIndex = currentPartIndex;
            leftOuterWeaponParts[equippedIndex].isEquipped = true;
            leftOuterWeaponParts[equippedIndex].isOwned = true;
            leftOuterWeaponParts[equippedIndex].isPurchasable = false;
        }
        else if (partCategory == PartCategory.RightInnerWeapons)
        {
            if (equippedIndex != -1)
            {
                rightInnerWeaponParts[equippedIndex].isEquipped = false;
            }

            equippedIndex = currentPartIndex;
            rightInnerWeaponParts[equippedIndex].isEquipped = true;
            rightInnerWeaponParts[equippedIndex].isOwned = true;
            rightInnerWeaponParts[equippedIndex].isPurchasable = false;
        }
        else if (partCategory == PartCategory.RightOuterWeapons)
        {
            if (equippedIndex != -1)
            {
                rightOuterWeaponParts[equippedIndex].isEquipped = false;
            }

            equippedIndex = currentPartIndex;
            rightOuterWeaponParts[equippedIndex].isEquipped = true;
            rightOuterWeaponParts[equippedIndex].isOwned = true;
            rightOuterWeaponParts[equippedIndex].isPurchasable = false;
        }

        // Equip the new part from UI
        partUIs[equippedIndex].transform.Find("Price").GetComponent<TextMeshProUGUI>().text = "Equipped";
        partUIs[equippedIndex].transform.Find("Price").GetComponent<TextMeshProUGUI>().color = Color.green;
        UpdateActiveSlot();
    }
}

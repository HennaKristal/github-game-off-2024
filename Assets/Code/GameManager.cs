using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance ??= FindFirstObjectByType<GameManager>();
    private InputController inputController;
    private Fading fading;

    [SerializeField] private PlayerStats playerStats;

    [Header("Parts")]
    public List<PlaneStats> allPlaneCoreParts;
    public List<EngineStats> allEngineParts;
    public List<GeneratorStats> allGeneratorParts;
    public List<CoolerStats> allCoolerParts;
    public List<TokenStats> allTokens;
    public List<BadgeStats> allBadges;
    public List<WeaponStats> allMainWeaponsParts;
    public List<WeaponStats> allLeftInnerWeaponParts;
    public List<WeaponStats> allLeftOuterWeaponParts;
    public List<WeaponStats> allRightInnerWeaponParts;
    public List<WeaponStats> allRightOuterWeaponParts;

    [Header("Missions")]
    public MissionStats[] missions;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        inputController = GetComponent<InputController>();
        fading = GetComponent<Fading>();
    }

    public void Start()
    {
        fading.StartFadeIn(2f);
    }

    public void ExitApplication()
    {
        Application.Quit();
    }

    public void OpenLink(string url)
    {
        Application.OpenURL(url);
    }

    public InputController GetInputController()
    {
        return inputController;
    }

    public void LoadSceneByName(string sceneName) => StartCoroutine(ChangeScene(sceneName));
    private IEnumerator ChangeScene(string sceneName)
    {
        fading.StartFadeOut(2f);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneName);
    }

    public void ResetSaveData()
    {
        foreach (PlaneStats part in allPlaneCoreParts)
        {
            part.isOwned = part.isOwnedByDefault;
            part.isEquipped = part.isEquippedByDefault;
            part.isPurchasable = part.isPurchasableByDefault;
        }

        foreach (EngineStats part in allEngineParts)
        {
            part.isOwned = part.isOwnedByDefault;
            part.isEquipped = part.isEquippedByDefault;
            part.isPurchasable = part.isPurchasableByDefault;
        }

        foreach (GeneratorStats part in allGeneratorParts)
        {
            part.isOwned = part.isOwnedByDefault;
            part.isEquipped = part.isEquippedByDefault;
            part.isPurchasable = part.isPurchasableByDefault;
        }

        foreach (CoolerStats part in allCoolerParts)
        {
            part.isOwned = part.isOwnedByDefault;
            part.isEquipped = part.isEquippedByDefault;
            part.isPurchasable = part.isPurchasableByDefault;
        }

        foreach (TokenStats token in allTokens)
        {
            token.isOwned = token.isOwnedByDefault;
            token.isEquipped = token.isEquippedByDefault;
        }

        foreach (BadgeStats badge in allBadges)
        {
            badge.isOwned = badge.isOwnedByDefault;
            badge.isEquipped = badge.isEquippedByDefault;
        }

        foreach (WeaponStats weapon in allMainWeaponsParts)
        {
            weapon.isOwned = weapon.isOwnedByDefault;
            weapon.isEquipped = weapon.isEquippedByDefault;
            weapon.isPurchasable = weapon.isPurchasableByDefault;
        }

        foreach (WeaponStats weapon in allLeftInnerWeaponParts)
        {
            weapon.isOwned = weapon.isOwnedByDefault;
            weapon.isEquipped = weapon.isEquippedByDefault;
            weapon.isPurchasable = weapon.isPurchasableByDefault;
        }

        foreach (WeaponStats weapon in allLeftOuterWeaponParts)
        {
            weapon.isOwned = weapon.isOwnedByDefault;
            weapon.isEquipped = weapon.isEquippedByDefault;
            weapon.isPurchasable = weapon.isPurchasableByDefault;
        }

        foreach (WeaponStats weapon in allRightInnerWeaponParts)
        {
            weapon.isOwned = weapon.isOwnedByDefault;
            weapon.isEquipped = weapon.isEquippedByDefault;
            weapon.isPurchasable = weapon.isPurchasableByDefault;
        }

        foreach (WeaponStats weapon in allRightOuterWeaponParts)
        {
            weapon.isOwned = weapon.isOwnedByDefault;
            weapon.isEquipped = weapon.isEquippedByDefault;
            weapon.isPurchasable = weapon.isPurchasableByDefault;
        }

        foreach (MissionStats mission in missions)
        {
            mission.isCompleted = false;
            mission.score = 0;
        }

        playerStats.progressStep = 0;
        playerStats.selectedLevel = "Tutorial";
        playerStats.money = 0;
        CalculatePlayerDefaultStats();
    }

    private void CalculatePlayerDefaultStats()
    {
        float _energyConsumption = 0f;
        float _energyOutput = 0f;
        float _repairCost = 0f;
        float _currentLiftWeight = 0f;
        float _maxLiftWeight = 0f;
        float _maxCarryWeight = 0f;
        float _currentCarryWeight = 0f;

        foreach (PlaneStats part in allPlaneCoreParts)
        {
            if (part.isEquipped)
            {
                playerStats.playerPrefab = part.playerPrefab;
                playerStats.maxHealth = part.maxHealth;
                playerStats.physicalDefence = part.physicalDefence;
                playerStats.energyDefence = part.energyDefence;
                playerStats.maxHeatTolerance = part.maxHeatTolerance;
                playerStats.idleHeat = part.idleHeat;

                _maxCarryWeight = part.maxCarryWeight;
                _energyConsumption += part.energyConsumption;
                _currentLiftWeight += part.weight;
                _repairCost += part.repairCost;

                break;
            }
        }

        foreach (EngineStats part in allEngineParts)
        {
            if (part.isEquipped)
            {
                playerStats.horizontalSpeed = part.horizontalSpeed;
                playerStats.verticalSpeed = part.verticalSpeed;

                _maxLiftWeight = part.maxLiftWeight;
                _energyConsumption += part.energyConsumption;
                _currentLiftWeight += part.weight;
                _repairCost += part.repairCost;

                break;
            }
        }

        foreach (GeneratorStats part in allGeneratorParts)
        {
            if (part.isEquipped)
            {
                playerStats.maxEnergy = part.maxEnergy;
                playerStats.energyRecharge = part.energyRecharge;

                _energyOutput = part.energyOutput;
                _energyConsumption += part.energyConsumption;
                _currentLiftWeight += part.weight;
                _repairCost += part.repairCost;

                break;
            }
        }

        foreach (CoolerStats part in allCoolerParts)
        {
            if (part.isEquipped)
            {
                playerStats.coolingEfficiency = part.coolingEfficiency;
                playerStats.overHeatcoolingEfficiency = part.overHeatcoolingEfficiency;

                _energyConsumption += part.energyConsumption;
                _currentLiftWeight += part.weight;
                _repairCost += part.repairCost;

                break;
            }
        }

        foreach (WeaponStats weapon in allMainWeaponsParts)
        {
            if (weapon.isEquipped)
            {
                _energyConsumption += weapon.energyConsumption;
                _currentCarryWeight += weapon.weight;
                _currentLiftWeight += weapon.weight;

                break;
            }
        }

        foreach (WeaponStats weapon in allLeftInnerWeaponParts)
        {
            if (weapon.isEquipped)
            {
                _energyConsumption += weapon.energyConsumption;
                _currentCarryWeight += weapon.weight;
                _currentLiftWeight += weapon.weight;

                break;
            }
        }

        foreach (WeaponStats weapon in allLeftOuterWeaponParts)
        {
            if (weapon.isEquipped)
            {
                _energyConsumption += weapon.energyConsumption;
                _currentCarryWeight += weapon.weight;
                _currentLiftWeight += weapon.weight;

                break;
            }
        }

        foreach (WeaponStats weapon in allRightInnerWeaponParts)
        {
            if (weapon.isEquipped)
            {
                _energyConsumption += weapon.energyConsumption;
                _currentCarryWeight += weapon.weight;
                _currentLiftWeight += weapon.weight;

                break;
            }
        }

        foreach (WeaponStats weapon in allRightOuterWeaponParts)
        {
            if (weapon.isEquipped)
            {
                _energyConsumption += weapon.energyConsumption;
                _currentCarryWeight += weapon.weight;
                _currentLiftWeight += weapon.weight;
                break;
            }
        }

        playerStats.currentCarryWeight = _currentCarryWeight;
        playerStats.maxCarryWeight = _maxCarryWeight;
        playerStats.currentLiftWeight = _currentLiftWeight;
        playerStats.maxLiftWeight = _maxLiftWeight;
        playerStats.energyConsumption = _energyConsumption;
        playerStats.energyOutput = _energyOutput;
        playerStats.repairCost = _repairCost;
    }

    public void SaveData()
    {
        foreach (PlaneStats part in allPlaneCoreParts)
        {
            PlayerPrefs.SetInt("PlaneStats-" + SanitizeName(part.name) + "-isOwned", part.isOwned ? 1 : 0);
            PlayerPrefs.SetInt("PlaneStats-" + SanitizeName(part.name) + "-isEquipped", part.isEquipped ? 1 : 0);
            PlayerPrefs.SetInt("PlaneStats-" + SanitizeName(part.name) + "-isPurchasable", part.isPurchasable ? 1 : 0);
        }

        foreach (EngineStats part in allEngineParts)
        {
            PlayerPrefs.SetInt("EngineStats-" + SanitizeName(part.name) + "-isOwned", part.isOwned ? 1 : 0);
            PlayerPrefs.SetInt("EngineStats-" + SanitizeName(part.name) + "-isEquipped", part.isEquipped ? 1 : 0);
            PlayerPrefs.SetInt("EngineStats-" + SanitizeName(part.name) + "-isPurchasable", part.isPurchasable ? 1 : 0);
        }

        foreach (GeneratorStats part in allGeneratorParts)
        {
            PlayerPrefs.SetInt("GeneratorStats-" + SanitizeName(part.name) + "-isOwned", part.isOwned ? 1 : 0);
            PlayerPrefs.SetInt("GeneratorStats-" + SanitizeName(part.name) + "-isEquipped", part.isEquipped ? 1 : 0);
            PlayerPrefs.SetInt("GeneratorStats-" + SanitizeName(part.name) + "-isPurchasable", part.isPurchasable ? 1 : 0);
        }

        foreach (CoolerStats part in allCoolerParts)
        {
            PlayerPrefs.SetInt("CoolerStats-" + SanitizeName(part.name) + "-isOwned", part.isOwned ? 1 : 0);
            PlayerPrefs.SetInt("CoolerStats-" + SanitizeName(part.name) + "-isEquipped", part.isEquipped ? 1 : 0);
            PlayerPrefs.SetInt("CoolerStats-" + SanitizeName(part.name) + "-isPurchasable", part.isPurchasable ? 1 : 0);
        }

        foreach (TokenStats token in allTokens)
        {
            PlayerPrefs.SetInt("TokenStats-" + SanitizeName(token.name) + "-isOwned", token.isOwned ? 1 : 0);
            PlayerPrefs.SetInt("TokenStats-" + SanitizeName(token.name) + "-isEquipped", token.isEquipped ? 1 : 0);
        }

        foreach (BadgeStats badge in allBadges)
        {
            PlayerPrefs.SetInt("BadgeStats-" + SanitizeName(badge.name) + "-isOwned", badge.isOwned ? 1 : 0);
            PlayerPrefs.SetInt("BadgeStats-" + SanitizeName(badge.name) + "-isEquipped", badge.isEquipped ? 1 : 0);
        }

        foreach (WeaponStats weapon in allMainWeaponsParts)
        {
            PlayerPrefs.SetInt("WeaponStats-Main-" + SanitizeName(weapon.name) + "-isOwned", weapon.isOwned ? 1 : 0);
            PlayerPrefs.SetInt("WeaponStats-Main-" + SanitizeName(weapon.name) + "-isEquipped", weapon.isEquipped ? 1 : 0);
            PlayerPrefs.SetInt("WeaponStats-Main-" + SanitizeName(weapon.name) + "-isPurchasable", weapon.isPurchasable ? 1 : 0);
        }

        foreach (WeaponStats weapon in allLeftInnerWeaponParts)
        {
            PlayerPrefs.SetInt("WeaponStats-LeftInner-" + SanitizeName(weapon.name) + "-isOwned", weapon.isOwned ? 1 : 0);
            PlayerPrefs.SetInt("WeaponStats-LeftInner-" + SanitizeName(weapon.name) + "-isEquipped", weapon.isEquipped ? 1 : 0);
            PlayerPrefs.SetInt("WeaponStats-LeftInner-" + SanitizeName(weapon.name) + "-isPurchasable", weapon.isPurchasable ? 1 : 0);
        }

        foreach (WeaponStats weapon in allLeftOuterWeaponParts)
        {
            PlayerPrefs.SetInt("WeaponStats-LeftOuter-" + SanitizeName(weapon.name) + "-isOwned", weapon.isOwned ? 1 : 0);
            PlayerPrefs.SetInt("WeaponStats-LeftOuter-" + SanitizeName(weapon.name) + "-isEquipped", weapon.isEquipped ? 1 : 0);
            PlayerPrefs.SetInt("WeaponStats-LeftOuter-" + SanitizeName(weapon.name) + "-isPurchasable", weapon.isPurchasable ? 1 : 0);
        }

        foreach (WeaponStats weapon in allRightInnerWeaponParts)
        {
            PlayerPrefs.SetInt("WeaponStats-RightInner-" + SanitizeName(weapon.name) + "-isOwned", weapon.isOwned ? 1 : 0);
            PlayerPrefs.SetInt("WeaponStats-RightInner-" + SanitizeName(weapon.name) + "-isEquipped", weapon.isEquipped ? 1 : 0);
            PlayerPrefs.SetInt("WeaponStats-RightInner-" + SanitizeName(weapon.name) + "-isPurchasable", weapon.isPurchasable ? 1 : 0);
        }

        foreach (WeaponStats weapon in allRightOuterWeaponParts)
        {
            PlayerPrefs.SetInt("WeaponStats-RightOuter-" + SanitizeName(weapon.name) + "-isOwned", weapon.isOwned ? 1 : 0);
            PlayerPrefs.SetInt("WeaponStats-RightOuter-" + SanitizeName(weapon.name) + "-isEquipped", weapon.isEquipped ? 1 : 0);
            PlayerPrefs.SetInt("WeaponStats-RightOuter-" + SanitizeName(weapon.name) + "-isPurchasable", weapon.isPurchasable ? 1 : 0);
        }

        foreach (MissionStats mission in missions)
        {
            PlayerPrefs.SetInt("MissionStats-" + SanitizeName(mission.sceneName) + "-isCompleted", mission.isCompleted ? 1 : 0);
            PlayerPrefs.SetInt("MissionStats-" + SanitizeName(mission.sceneName) + "-score", mission.score);
        }

        PlayerPrefs.SetInt("PlayerData-money", playerStats.money);
        PlayerPrefs.SetInt("PlayerData-progressStep", playerStats.progressStep);
        PlayerPrefs.SetString("PlayerData-selectedLevel", playerStats.selectedLevel);
        PlayerPrefs.SetFloat("PlayerData-maxHealth", playerStats.maxHealth);
        PlayerPrefs.SetFloat("PlayerData-physicalDefence", playerStats.physicalDefence);
        PlayerPrefs.SetFloat("PlayerData-energyDefence", playerStats.energyDefence);
        PlayerPrefs.SetFloat("PlayerData-horizontalSpeed", playerStats.horizontalSpeed);
        PlayerPrefs.SetFloat("PlayerData-verticalSpeed", playerStats.verticalSpeed);
        PlayerPrefs.SetFloat("PlayerData-maxHeatTolerance", playerStats.maxHeatTolerance);
        PlayerPrefs.SetFloat("PlayerData-idleHeat", playerStats.idleHeat);
        PlayerPrefs.SetFloat("PlayerData-coolingEfficiency", playerStats.coolingEfficiency);
        PlayerPrefs.SetFloat("PlayerData-overHeatcoolingEfficiency", playerStats.overHeatcoolingEfficiency);
        PlayerPrefs.SetFloat("PlayerData-currentCarryWeight", playerStats.currentCarryWeight);
        PlayerPrefs.SetFloat("PlayerData-maxCarryWeight", playerStats.maxCarryWeight);
        PlayerPrefs.SetFloat("PlayerData-currentLiftWeight", playerStats.currentLiftWeight);
        PlayerPrefs.SetFloat("PlayerData-maxLiftWeight", playerStats.maxLiftWeight);
        PlayerPrefs.SetFloat("PlayerData-energyOutput", playerStats.energyOutput);
        PlayerPrefs.SetFloat("PlayerData-maxEnergy", playerStats.maxEnergy);
        PlayerPrefs.SetFloat("PlayerData-energyRecharge", playerStats.energyRecharge);
        PlayerPrefs.SetFloat("PlayerData-energyConsumption", playerStats.energyConsumption);
        PlayerPrefs.SetFloat("PlayerData-repairCost", playerStats.repairCost);

        PlayerPrefs.SetInt("SafeFileFound", 1);

        PlayerPrefs.Save();
    }

    public void LoadData()
    {
        foreach (PlaneStats part in allPlaneCoreParts)
        {
            part.isOwned = PlayerPrefs.GetInt("PlaneStats-" + SanitizeName(part.name) + "-isOwned", 0) == 1;
            part.isEquipped = PlayerPrefs.GetInt("PlaneStats-" + SanitizeName(part.name) + "-isEquipped", 0) == 1;
            part.isPurchasable = PlayerPrefs.GetInt("PlaneStats-" + SanitizeName(part.name) + "-isPurchasable", 0) == 1;

            if (part.isEquipped)
            {
                playerStats.playerPrefab = part.playerPrefab;
            }
        }

        foreach (EngineStats part in allEngineParts)
        {
            part.isOwned = PlayerPrefs.GetInt("EngineStats-" + SanitizeName(part.name) + "-isOwned", 0) == 1;
            part.isEquipped = PlayerPrefs.GetInt("EngineStats-" + SanitizeName(part.name) + "-isEquipped", 0) == 1;
            part.isPurchasable = PlayerPrefs.GetInt("EngineStats-" + SanitizeName(part.name) + "-isPurchasable", 0) == 1;
        }

        foreach (GeneratorStats part in allGeneratorParts)
        {
            part.isOwned = PlayerPrefs.GetInt("GeneratorStats-" + SanitizeName(part.name) + "-isOwned", 0) == 1;
            part.isEquipped = PlayerPrefs.GetInt("GeneratorStats-" + SanitizeName(part.name) + "-isEquipped", 0) == 1;
            part.isPurchasable = PlayerPrefs.GetInt("GeneratorStats-" + SanitizeName(part.name) + "-isPurchasable", 0) == 1;
        }

        foreach (CoolerStats part in allCoolerParts)
        {
            part.isOwned = PlayerPrefs.GetInt("CoolerStats-" + SanitizeName(part.name) + "-isOwned", 0) == 1;
            part.isEquipped = PlayerPrefs.GetInt("CoolerStats-" + SanitizeName(part.name) + "-isEquipped", 0) == 1;
            part.isPurchasable = PlayerPrefs.GetInt("CoolerStats-" + SanitizeName(part.name) + "-isPurchasable", 0) == 1;
        }

        foreach (TokenStats token in allTokens)
        {
            token.isOwned = PlayerPrefs.GetInt("TokenStats-" + SanitizeName(token.name) + "-isOwned", 0) == 1;
            token.isEquipped = PlayerPrefs.GetInt("TokenStats-" + SanitizeName(token.name) + "-isEquipped", 0) == 1;
        }

        foreach (BadgeStats badge in allBadges)
        {
            badge.isOwned = PlayerPrefs.GetInt("BadgeStats-" + SanitizeName(badge.name) + "-isOwned", 0) == 1;
            badge.isEquipped = PlayerPrefs.GetInt("BadgeStats-" + SanitizeName(badge.name) + "-isEquipped", 0) == 1;
        }

        foreach (WeaponStats weapon in allMainWeaponsParts)
        {
            weapon.isOwned = PlayerPrefs.GetInt("WeaponStats-Main-" + SanitizeName(weapon.name) + "-isOwned", 0) == 1;
            weapon.isEquipped = PlayerPrefs.GetInt("WeaponStats-Main-" + SanitizeName(weapon.name) + "-isEquipped", 0) == 1;
            weapon.isPurchasable = PlayerPrefs.GetInt("WeaponStats-Main-" + SanitizeName(weapon.name) + "-isPurchasable", 0) == 1;
        }

        foreach (WeaponStats weapon in allLeftInnerWeaponParts)
        {
            weapon.isOwned = PlayerPrefs.GetInt("WeaponStats-LeftInner-" + SanitizeName(weapon.name) + "-isOwned", 0) == 1;
            weapon.isEquipped = PlayerPrefs.GetInt("WeaponStats-LeftInner-" + SanitizeName(weapon.name) + "-isEquipped", 0) == 1;
            weapon.isPurchasable = PlayerPrefs.GetInt("WeaponStats-LeftInner-" + SanitizeName(weapon.name) + "-isPurchasable", 0) == 1;
        }

        foreach (WeaponStats weapon in allLeftOuterWeaponParts)
        {
            weapon.isOwned = PlayerPrefs.GetInt("WeaponStats-LeftOuter-" + SanitizeName(weapon.name) + "-isOwned", 0) == 1;
            weapon.isEquipped = PlayerPrefs.GetInt("WeaponStats-LeftOuter-" + SanitizeName(weapon.name) + "-isEquipped", 0) == 1;
            weapon.isPurchasable = PlayerPrefs.GetInt("WeaponStats-LeftOuter-" + SanitizeName(weapon.name) + "-isPurchasable", 0) == 1;
        }

        foreach (WeaponStats weapon in allRightInnerWeaponParts)
        {
            weapon.isOwned = PlayerPrefs.GetInt("WeaponStats-RightInner-" + SanitizeName(weapon.name) + "-isOwned", 0) == 1;
            weapon.isEquipped = PlayerPrefs.GetInt("WeaponStats-RightInner-" + SanitizeName(weapon.name) + "-isEquipped", 0) == 1;
            weapon.isPurchasable = PlayerPrefs.GetInt("WeaponStats-RightInner-" + SanitizeName(weapon.name) + "-isPurchasable", 0) == 1;
        }

        foreach (WeaponStats weapon in allRightOuterWeaponParts)
        {
            weapon.isOwned = PlayerPrefs.GetInt("WeaponStats-RightOuter-" + SanitizeName(weapon.name) + "-isOwned", 0) == 1;
            weapon.isEquipped = PlayerPrefs.GetInt("WeaponStats-RightOuter-" + SanitizeName(weapon.name) + "-isEquipped", 0) == 1;
            weapon.isPurchasable = PlayerPrefs.GetInt("WeaponStats-RightOuter-" + SanitizeName(weapon.name) + "-isPurchasable", 0) == 1;
        }

        foreach (MissionStats mission in missions)
        {
            mission.isCompleted = PlayerPrefs.GetInt("MissionStats-" + SanitizeName(mission.sceneName) + "-isCompleted", 0) == 1;
            mission.score = PlayerPrefs.GetInt("MissionStats-" + SanitizeName(mission.sceneName) + "-score", 0);
        }

        playerStats.money = PlayerPrefs.GetInt("PlayerData-money", 0);
        playerStats.progressStep = PlayerPrefs.GetInt("PlayerData-progressStep", 0);
        playerStats.selectedLevel = PlayerPrefs.GetString("PlayerData-selectedLevel", "");
        playerStats.maxHealth = PlayerPrefs.GetFloat("PlayerData-maxHealth", 0);
        playerStats.physicalDefence = PlayerPrefs.GetFloat("PlayerData-physicalDefence", 0);
        playerStats.energyDefence = PlayerPrefs.GetFloat("PlayerData-energyDefence", 0);
        playerStats.horizontalSpeed = PlayerPrefs.GetFloat("PlayerData-horizontalSpeed", 0);
        playerStats.verticalSpeed = PlayerPrefs.GetFloat("PlayerData-verticalSpeed", 0);
        playerStats.maxHeatTolerance = PlayerPrefs.GetFloat("PlayerData-maxHeatTolerance", 0);
        playerStats.idleHeat = PlayerPrefs.GetFloat("PlayerData-idleHeat", 0);
        playerStats.coolingEfficiency = PlayerPrefs.GetFloat("PlayerData-coolingEfficiency", 0);
        playerStats.overHeatcoolingEfficiency = PlayerPrefs.GetFloat("PlayerData-overHeatcoolingEfficiency", 0);
        playerStats.currentCarryWeight = PlayerPrefs.GetFloat("PlayerData-currentCarryWeight", 0);
        playerStats.maxCarryWeight = PlayerPrefs.GetFloat("PlayerData-maxCarryWeight", 0);
        playerStats.currentLiftWeight = PlayerPrefs.GetFloat("PlayerData-currentLiftWeight", 0);
        playerStats.maxLiftWeight = PlayerPrefs.GetFloat("PlayerData-maxLiftWeight", 0);
        playerStats.energyOutput = PlayerPrefs.GetFloat("PlayerData-energyOutput", 0);
        playerStats.maxEnergy = PlayerPrefs.GetFloat("PlayerData-maxEnergy", 0);
        playerStats.energyRecharge = PlayerPrefs.GetFloat("PlayerData-energyRecharge", 0);
        playerStats.energyConsumption = PlayerPrefs.GetFloat("PlayerData-energyConsumption", 0);
        playerStats.repairCost = PlayerPrefs.GetFloat("PlayerData-repairCost", 0);
    }

    private string SanitizeName(string name)
    {
        return Regex.Replace(name, @"[^a-zA-Z0-9]", "_");
    }
}

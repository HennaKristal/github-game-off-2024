using UnityEngine;

[CreateAssetMenu(menuName = "Create New Plane Core Part")]
public class PlaneStats : ScriptableObject
{
    public string partName = "";
    public string saveName = "";
    public string manufacturer = "";
    [TextArea(3, 50)] public string description = "";

    public Sprite icon;
    public Sprite sprite;
    public GameObject playerPrefab;

    [Header("Ownership")]
    public bool isOwned = false;
    public bool isEquipped = false;
    public bool isPurchasable = false;
    public bool isOwnedByDefault = false;
    public bool isEquippedByDefault = false;
    public bool isPurchasableByDefault = false;
    public int purchasePrice = 0;
    public int sellPrice = 0;

    [Header("Health")]
    public float maxHealth = 0f;
    public float physicalDefence = 0f;
    public float energyDefence = 0f;

    [Header("Heating")]
    public float maxHeatTolerance = 0f;
    public float idleHeat = 0f;

    [Header("Weight")]
    public float maxCarryWeight = 0f;
    public float weight = 0f;

    [Header("Energy")]
    public float energyConsumption = 0f;

    [Header("Cost")]
    public float repairCost = 0f;
}

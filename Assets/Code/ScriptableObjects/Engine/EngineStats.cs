using UnityEngine;

[CreateAssetMenu(menuName = "Create New Engine Part")]
public class EngineStats : ScriptableObject
{
    public string partName = "";
    public string saveName = "";
    public string manufacturer = "";
    [TextArea(3, 50)] public string description = "";

    public Sprite icon;

    [Header("Ownership")]
    public bool isOwned = false;
    public bool isEquipped = false;
    public bool isPurchasable = false;
    public bool isOwnedByDefault = false;
    public bool isEquippedByDefault = false;
    public bool isPurchasableByDefault = false;
    public int purchasePrice = 0;
    public int sellPrice = 0;

    [Header("Speed")]
    public float horizontalSpeed = 0f;
    public float verticalSpeed = 0f;

    [Header("Weight")]
    public float maxLiftWeight = 0f;
    public float weight = 0f;

    [Header("Energy")]
    public float energyConsumption = 0f;

    [Header("Cost")]
    public float repairCost = 0f;
}

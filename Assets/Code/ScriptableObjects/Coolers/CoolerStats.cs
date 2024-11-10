using UnityEngine;

[CreateAssetMenu(menuName = "Create New Cooler Part")]
public class CoolerStats : ScriptableObject
{
    public string partName = "";
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

    [Header("Heating")]
    public float coolingEfficiency = 0f;
    public float overHeatcoolingEfficiency = 0f;

    [Header("Weight")]
    public float weight = 0f;

    [Header("Energy")]
    public float energyConsumption = 0f;

    [Header("Cost")]
    public float repairCost = 0f;
}
